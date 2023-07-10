using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BuildingInstallation : MonoBehaviour
{
    [SerializeField] private ContainersHandler _containersHandler;
    [SerializeField] private BuildingsPanel _buildingsPanel;
    [SerializeField] private BuildingsPanelView _buildingsPanelView;
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private BuildingsSaver _buildingsSaver;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private WalletPresenter _walletPresenter;

    private BuildingSlotData _selectedSlot;
    private Vector2Int _currentIndex;
    private Building _building;

    public event Action<BuildingSlotData> Crafted;
    public event Action<Building> Done;

    public Vector2Int CurrentIndex => _currentIndex;

    private void OnEnable()
    {
        _buildingsPanelView.ShowInstallationMode();
        StartCoroutine(SubscribeAfterGridInitialization());
        StartCoroutine(SubscribeAfterSlotsInitialization());
    }

    private void OnDisable()
    {
        foreach (var slotButon in _buildingsPanel.SlotButtons)
            slotButon.Selected -= OnSlotButtonSelected;

        foreach (var gridCell in _constructorGrid.Cells)
        {
            gridCell.PointerUped -= OnPointerUped;
            gridCell.PointerEntered -= OnPointerEntered;
            gridCell.PointerDowned -= OnPointerDowned;
        }
    }

    private void OnSlotButtonSelected(BuildingSlotData slotData)
    {
        _selectedSlot = slotData;
    }

    private void OnPointerUped()
    {
        if (_building == null)
            return;

        InitializeBuilding();

        if (_constructorGrid.IsAreaEmpty(_currentIndex, _selectedSlot.Name))
        {
            _walletPresenter.SpendResource(_selectedSlot.JunkNeeded);
            _constructorGrid.FillArea(_currentIndex, _building);

            Done?.Invoke(_building);
            Crafted?.Invoke(_selectedSlot);

            if (_building is Fence)
                (_building as Fence).TryAttach();

            if (_building is Gate)
            {
                if ((_building as Gate).TryAttach() == false)
                {
                    _buildingsPanelView.ShowGateBuildWarning();
                    _buildingsSaver.DeleteFromSaveList(_building);
                    _walletPresenter.AddResource(_selectedSlot.JunkNeeded);
                    Destroy(_building.gameObject);
                }
            }
        }
        else
        {
            _buildingsSaver.DeleteFromSaveList(_building);
            Destroy(_building.gameObject);
        }

        _building = null;
        _constructorGrid.UpdateColors();
    }

    private void OnPointerEntered(Vector3 position, Vector2Int index)
    {
        if (_building == null)
            return;

        _currentIndex = index;
        _building.transform.position = position;
        _constructorGrid.UpdateColors();

        if (_constructorGrid.IsAreaEmpty(_currentIndex, _selectedSlot.Name))
            _constructorGrid.ShowFillArea(_currentIndex, _selectedSlot.Name);
        else
            _constructorGrid.ShowBlockArea(_currentIndex, _selectedSlot.Name);
    }

    private void OnPointerDowned(Vector3 position, Vector2Int index)
    {
        if (_walletPresenter.Value < _selectedSlot.JunkNeeded)
        {
            _buildingsPanelView.ShowJunklessMessage();
            return;
        }

        _currentIndex = index;

        if (_constructorGrid.GetCellBy(index).Building == null)
            _building = Instantiate(_selectedSlot.BuildingTemplate, position, Quaternion.identity);
    }

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

    private IEnumerator SubscribeAfterSlotsInitialization()
    {
        yield return new WaitUntil(() => _buildingsPanel.SlotsCreated);

        foreach (var slotButton in _buildingsPanel.SlotButtons)
            slotButton.Selected += OnSlotButtonSelected;
    }

    private void InitializeBuilding()
    {
        _building.Init(_selectedSlot.Name, _selectedSlot.Class, _constructorGrid.GetCellBy(_currentIndex), _selectedSlot.MaxSafetyMargin, _buildingRemoving);
        _building.InitBuildingsSaver(_buildingsSaver);
        Transform containerForCrafted = _containersHandler.GetContainerFor(_building);
        _building.Place(containerForCrafted);
    }

    public Building CreateBuilding(BuildingSlotData buildingSlotData, Vector3 position, Vector2Int index, Quaternion rotation = default)
    {
        _currentIndex = index;
        _selectedSlot = buildingSlotData;
        _building = Instantiate(_selectedSlot.BuildingTemplate, position, rotation);
        _building.transform.position = position;
        InitializeBuilding();
        Done?.Invoke(_building);
        _constructorGrid.FillArea(_currentIndex, _building);
        _constructorGrid.UpdateColors();
        _constructorGrid.ShowFillArea(_currentIndex, _selectedSlot.Name);

        return _building;
    }

    public void CleareCurrentTargetBuilding() => _building = null;
}
