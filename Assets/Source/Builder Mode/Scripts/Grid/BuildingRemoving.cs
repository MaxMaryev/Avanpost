using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRemoving : MonoBehaviour
{
    [SerializeField] private BuildingSlotsDatasSO _buildingSlotsDatas;
    [SerializeField] private BuildingsPanel _buildingsPanel;
    [SerializeField] private BuildingsPanelView _buildingsPanelView;
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private BuildingsSaver _buildingsSaver;
    [SerializeField] private FencesHandler _fencesHandler;
    [SerializeField] private WalletPresenter _walletPresenter;
    [SerializeField] private float _refundShare;
    [SerializeField] private Confirmer _confirmer;

    private Vector2Int _currentIndex;
    private List<Vector2Int> _allowedBreakIndex = new List<Vector2Int>();

    public event Action Done;

    private void OnEnable()
    {
        _buildingsPanelView.ShowBreakMode();
        StartCoroutine(SubscribeAfterGridInitialization());
    }

    private void OnDisable()
    {
        foreach (var gridCell in _constructorGrid.Cells)
        {
            gridCell.PointerUped -= OnPointerUped;
            gridCell.PointerEntered -= OnPointerEntered;
            gridCell.PointerDowned -= OnPointerDowned;
        }
    }

    private void OnPointerUped()
    {
        Building building = _constructorGrid.GetCellBy(_currentIndex).Building;

        if (building == null || building is Empty)
            return;

        if (_allowedBreakIndex.Count > 0 && _allowedBreakIndex.Contains(_currentIndex) == false)
            return;

        if (building.Class == BuildingClass.Turret)
        {
            _confirmer.AskConfirmation(() => OnAproved());

            void OnAproved() => StartCoroutine(building.StartDestroying());
        }
        else
        {
            StartCoroutine(building.StartDestroying());
        }
    }

    private void OnPointerEntered(Vector3 position, Vector2Int index) => _currentIndex = index;

    private void OnPointerDowned(Vector3 position, Vector2Int index) => _currentIndex = index;

    private IEnumerator SubscribeAfterGridInitialization()
    {
        yield return new WaitUntil(() => _constructorGrid.IsCreated);

        foreach (var gridCell in _constructorGrid.Cells)
        {
            gridCell.PointerUped += OnPointerUped;
            gridCell.PointerEntered += OnPointerEntered;
            gridCell.PointerDowned += OnPointerDowned;
        }
    }

    public void AddIndexAllowedBreak(Vector2Int index)
    {
        if (_allowedBreakIndex.Contains(index) == false)
            _allowedBreakIndex.Add(index);
    }

    public void RemoveIndexAllowedBreak(Vector2Int index)
    {
        if (_allowedBreakIndex.Contains(index))
            _allowedBreakIndex.Remove(index);
    }

    public void Remove(Building building)
    {
        _constructorGrid.ClearArea(building.Cell.Index);
        _buildingsSaver.DeleteFromSaveList(building);

        if (building is Fence fence)
        {
            if (fence.FenceChain.Fences.Count >= 3 && fence.OrderNumber != 0 && fence.OrderNumber != fence.FenceChain.Fences.Count - 1)
                _fencesHandler.SplitChain(building as Fence);
            else if (fence.FenceChain.Fences.Count == 1)
                _fencesHandler.DeleteChain(fence.FenceChain);
            else
                fence.FenceChain.Remove(fence);

            for (int i = 0; i < fence.NeighborFences.Count; i++)
                fence.NeighborFences[i].TryRemoveThisNeighbor(fence);
        }
        else if (building is Gate gate)
        {
            for (int i = 0; i < gate.AttachedFences.Count; i++)
                gate.AttachedFences[i].TryRemoveThisNeighbor(gate);
        }

        RefundFor(building);
        Done?.Invoke();
    }

    private void RefundFor(Building building)
    {
        int refund = Mathf.RoundToInt(_buildingSlotsDatas.GetSlotBy(building.Name).JunkNeeded * _refundShare);
        _walletPresenter.AddResource(refund);
    }
}
