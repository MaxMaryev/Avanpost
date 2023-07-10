using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Gate : Building
{
    [SerializeField] private GameObject _short;
    [SerializeField] private GameObject _long;
    [SerializeField] private GateLock _longGateLock;
    [SerializeField] private GateLock _shortGateLock;
    [SerializeField] private Transform _leftSide;
    [SerializeField] private Transform _rightSide;

    private List<Fence> _attachedFences = new List<Fence>();
    private Vector3 _housePosition;

    public IReadOnlyList<Fence> AttachedFences => _attachedFences;

    protected bool IsDiagonally { get; private set; }

    public event Action<Gate> Opened;
    public event Action<Gate> Closed;

    private void OnEnable()
    {
        _longGateLock.Opened += OnGateLockOpened;
        _longGateLock.Closed += OnGateLockClosed;
        _shortGateLock.Opened += OnGateLockOpened;
        _shortGateLock.Closed += OnGateLockClosed;
    }

    private void OnDisable()
    {
        _longGateLock.Opened -= OnGateLockOpened;
        _longGateLock.Closed -= OnGateLockClosed;
        _shortGateLock.Opened -= OnGateLockOpened;
        _shortGateLock.Closed -= OnGateLockClosed;
    }

    public void InitHousePosition(Vector3 housePosition) => _housePosition = housePosition;

    public bool TryAttach()
    {
        List<Fence> neighborFences = GetNeighborFenses();

        if (neighborFences.Count < 2)
            return false;

        //Fence nearestLeftFence = neighborFences.OrderBy(x => (x.RightSide.position - _leftSide.position).SqrMagnitudeXZ()).FirstOrDefault();
        //Fence nearestRightFence = neighborFences.Where(x => x != nearestLeftFence).OrderBy(x => (x.LeftSide.position - _rightSide.position).SqrMagnitudeXZ()).FirstOrDefault();
        Fence nearestLeftFence = neighborFences.OrderBy(x => (x.Cell.transform.position - transform.position).SqrMagnitudeXZ()).FirstOrDefault();
        Fence nearestRightFence = neighborFences.Where(x => x != nearestLeftFence).OrderBy(x => (x.Cell.transform.position - transform.position).SqrMagnitudeXZ()).FirstOrDefault();

        _attachedFences.Add(nearestLeftFence);
        _attachedFences.Add(nearestRightFence);

        AlignWithNeighbors();

        return true;
    }

    private void AlignWithNeighbors()
    {
        _attachedFences.OrderBy(x => x.Cell.Index.x);
        Vector3 directionToFence = (_attachedFences[0].Cell.transform.position - _attachedFences[1].Cell.transform.position).normalized;
        IsDiagonally = IsAttachDiagonally(directionToFence);
        SetPosition();
        UpdateModel(IsDiagonally);
        SetRotation(directionToFence);

        UpdateNeigborsAttachs();
    }

    private void OnGateLockOpened(BoxCollider collider)
    {
        Opened?.Invoke(this);
        GateLockOpened(collider);
    }

    private void OnGateLockClosed(BoxCollider collider)
    {
        Closed?.Invoke(this);
        GateLockClosed(collider);
    }

    protected abstract void GateLockOpened(BoxCollider collider);

    protected abstract void GateLockClosed(BoxCollider collider);

    private void SetRotation(Vector3 directionToFence)
    {
        Vector3 directionRelativeToHouse = _housePosition - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToFence, Vector3.up) * Quaternion.Euler(0f, -90f, 0f);
        bool isLookAtHouse = Vector3.Dot(transform.forward, directionRelativeToHouse) > 0;

        if (isLookAtHouse)
            transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
    }

    private void SetPosition() => transform.position = Cell.transform.position;

    private void UpdateModel(bool isDiagonally)
    {
        if (isDiagonally)
            _long.gameObject.SetActive(true);
        else
            _short.gameObject.SetActive(true);
    }

    private List<Fence> GetNeighborFenses()
    {
        List<Fence> neighborFences = new List<Fence>();

        for (int i = 0; i < Cell.Neighbors.Count; i++)
        {
            if (CheckNeighborForAttachable(Cell.Neighbors[i]) == false)
                continue;

            neighborFences.Add(Cell.Neighbors[i].Building as Fence);
        }

        return neighborFences;
    }

    private bool CheckNeighborForAttachable(ConstructorCell cell)
    {
        return cell.Building is Fence fence && fence.AttachsCount < 2;
    }

    private bool IsAttachDiagonally(Vector3 neighborDirection) => Vector3.Angle(neighborDirection, Vector3.forward) % 90 != 0;

    private void UpdateNeigborsAttachs()
    {
        for (int i = 0; i < _attachedFences.Count; i++)
        {
            _attachedFences[i].AddNeighbor(this);
            _attachedFences[i].Destroying += OnAttachedFenceDestroying;
        }
    }

    protected override void OnStartDestroying()
    {
        for (int i = 0; i < _attachedFences.Count; i++)
            _attachedFences[i].Destroying -= OnAttachedFenceDestroying;

        _longGateLock.gameObject.SetActive(false);
        _shortGateLock.gameObject.SetActive(false);
    }

    private void OnAttachedFenceDestroying(Building building) => StartCoroutine(StartDestroying());
}
