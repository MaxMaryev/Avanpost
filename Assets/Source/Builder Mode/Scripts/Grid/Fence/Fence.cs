using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class Fence : Building
{
    [SerializeField] private Column _column;
    [SerializeField] private List<Short> _shorts;
    [SerializeField] private Long _long;
    [SerializeField] private List<Fence> _neighborFences;
    [SerializeField] private List<Gate> _neighborGates;

    private FencesHandler _fencesHandler;
    private Vector3 _housePosition;

    [field: SerializeField] public Transform LeftSide { get; private set; }
    [field: SerializeField] public Transform RightSide { get; private set; }
    public IReadOnlyList<Fence> NeighborFences => _neighborFences;
    public IReadOnlyList<Gate> NeighborGates => _neighborGates;
    public FenceModel Model { get; private set; }
    public FenceBreakStatesHandler CurrentState { get; private set; }
    public int ChainId { get; private set; }
    public FenceChain FenceChain { get; private set; }
    public int OrderNumber { get; private set; }
    public bool IsColumnEnabled => _column.gameObject.activeSelf;
    public int AttachsCount => _neighborFences.Count + _neighborGates.Count;

    private void Awake() => CurrentState = _column;

    public void InitChainId(int chainId) => ChainId = chainId;

    public void InitFenceChain(FenceChain fenceChain) => FenceChain = fenceChain;

    public void InitFencesHandler(FencesHandler fencesHandler) => _fencesHandler = fencesHandler;

    public void InitOrderNumber(int orderNumber) => OrderNumber = orderNumber;

    public void InitHousePosition(Vector3 housePosition) => _housePosition = housePosition;

    public void TryAttach()
    {
        StartCoroutine(Trying());

        IEnumerator Trying()
        {
            yield return new WaitUntil(() => _fencesHandler != null);
            SetNeighbors();

            if (_neighborFences.Count == 0)
            {
                FenceChain = _fencesHandler.CreateNewChain(fences: this);
                OrderNumber = 0;
                yield break;
            }

            for (int i = 0; i < _neighborFences.Count; i++)
            {
                if (HasNeighbor(_neighborFences[i]))
                {
                    _neighborFences.Remove(_neighborFences[i]);
                    continue;
                }

                FenceChain = _neighborFences[i].FenceChain;

                if (ChainId == 0)
                {
                    if (_neighborFences[i].OrderNumber == 0)
                        FenceChain.ReverseOrderNumbers();

                    FenceChain.Add(this);
                    OrderNumber = _neighborFences[i].OrderNumber + 1;
                }

                if (FenceChain.Id != ChainId)
                    _fencesHandler.CombineTwoChains(ChainId, FenceChain.Id, _neighborFences[i].OrderNumber);

                _neighborFences[i].AddNeighbor(this);
                _fencesHandler.GetChain(ChainId).Update(isNeedEdjesUpdate: true);
            }
        }
    }

    public void ShowModel(FenceModel model)
    {
        if (Model == model)
            return;

        CurrentState.gameObject.SetActive(false);

        CurrentState = model switch
        {
            FenceModel.Column => _column,
            FenceModel.Short => _shorts[Random.Range(0, _shorts.Count)],
            FenceModel.Long => _long,
            _ => CurrentState = null,
        };

        CurrentState.gameObject.SetActive(true);
        CurrentState.UpdateBreakState(MaxSafetyMargin, CurrentSafetyMargin);

        Model = model;
    }

    public void ShowEdje(bool state)
    {
        if (state)
        {
            _column.transform.position = Cell.transform.position.With(y: _column.transform.position.y);
            _column.transform.eulerAngles = Vector3.forward;
        }

        _column.gameObject.SetActive(state);
        _column.UpdateBreakState(MaxSafetyMargin, CurrentSafetyMargin);
    }

    public void AlignWithNeighbor(Fence neighborFence)
    {
        Vector3 neighborCellDirection = (Cell.transform.position - neighborFence.Cell.transform.position).normalized;
        bool isDiagonally = IsAttachDiagonally(neighborCellDirection);
        SetPosition(isDiagonally, neighborCellDirection);
        UpdateModel(isDiagonally);
        SetRotation(neighborCellDirection);
    }

    public void SetNeighbors()
    {
        for (int i = 0; i < Cell.Neighbors.Count; i++)
        {
            if (CheckNeighborForAttachable(Cell.Neighbors[i]) == false)
                continue;

            if (Cell.Neighbors[i].Building is Fence fence)
            {
                if (_neighborFences.Contains(fence) == false)
                    _neighborFences.Add(fence);
            }
            else if (Cell.Neighbors[i].Building is Gate gate)
            {
                if (_neighborGates.Contains(gate) == false)
                    _neighborGates.Add(gate);
            }
        }
    }

    public void TryRemoveThisNeighbor(Fence fence)
    {
        if (_neighborFences.Contains(fence))
            _neighborFences.Remove(fence);
    }

    public void TryRemoveThisNeighbor(Gate gate)
    {
        if (_neighborGates.Contains(gate))
            _neighborGates.Remove(gate);
    }

    public void AddNeighbor(Fence fence) => _neighborFences.Add(fence);

    public void AddNeighbor(Gate gate) => _neighborGates.Add(gate);

    protected override void OnStartDestroying()
    {
        for (int i = 0; i < _neighborFences.Count; i++)
            _neighborFences[i].TryRemoveThisNeighbor(this);

        SetDustEffectPosition(CurrentState.transform.position);
    }

    private bool HasNeighbor(Fence fence) => fence.NeighborFences.FirstOrDefault(x => x == fence);

    private bool CheckNeighborForAttachable(ConstructorCell cell)
    {
        return cell.Building != null && cell.Building.GetType() == GetType() && (cell.Building as Fence).AttachsCount < 2;
    }

    private void SetRotation(Vector3 directionToFence)
    {
        Vector3 directionRelativeToHouse = _housePosition - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToFence, Vector3.up) * Quaternion.Euler(0f, -90f, 0f);
        bool isLookAtHouse = Vector3.Dot(transform.forward, directionRelativeToHouse) > 0;

        if (isLookAtHouse)
            transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
    }

    private void SetPosition(bool isDiagonally, Vector3 neighborDirection)
    {
        if (isDiagonally)
            transform.position = Cell.transform.position - neighborDirection * Constants.Sqrt2;
        else
            transform.position = Cell.transform.position - neighborDirection * 0.5f;
    }

    private void UpdateModel(bool isDiagonally)
    {
        if (isDiagonally)
            ShowModel(FenceModel.Long);
        else
            ShowModel(FenceModel.Short);
    }

    private bool IsAttachDiagonally(Vector3 neighborDirection) => Vector3.Angle(neighborDirection, Vector3.forward) % 90 != 0;
}
