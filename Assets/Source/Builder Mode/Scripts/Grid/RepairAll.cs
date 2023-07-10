using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairAll : MonoBehaviour
{
    [SerializeField] private RepairButton _repairButton;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private WalletPresenter _walletPresenter;
    [SerializeField] private BuildingSlotsDatasSO _buildingSlotsDatas;
    [SerializeField] private BuildingsSaver _buildingsSaver;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private float _partOfTotalBuildingCost;
    [SerializeField] private Color _availableRepairColor;
    [SerializeField] private Color _notAvailableRepairColor;
    [SerializeField] private ContainersHandler _containersHandler;

    private int _totalRepairCost;
    private IReadOnlyList<Building> _buildings;

    public event Action Done;

    private void OnEnable()
    {
        _repairButton.Clicked += OnRepairButtonClicked;
        _buildingRemoving.Done += OnBuildingRemoved;
        StartCoroutine(SetTotalRepairCostAfterBuildingsLoaded());
    }

    private void OnDisable()
    {
        _repairButton.Clicked -= OnRepairButtonClicked;
        _buildingRemoving.Done -= OnBuildingRemoved;
    }

    private void Start()
    {
        _hintText.text = LeanLocalization.CurrentTranslations["RepairAll"].Data.ToString();
    }

    private void OnRepairButtonClicked()
    {
        if (_totalRepairCost == 0)
            return;

        if (_walletPresenter.Value < _totalRepairCost)
            return;

        _walletPresenter.SpendResource(_totalRepairCost);

        _buildings = _containersHandler.GetBuildings();

        foreach (var building in _buildings)
            if (building.CurrentSafetyMargin < building.MaxSafetyMargin)
                    building.Repair();

        Done?.Invoke();
        _priceText.text = GetTotalRepairCost().ToString();
    }

    public int GetTotalRepairCost()
    {
        _totalRepairCost = 0;

        _buildings = _containersHandler.GetBuildings();

        foreach (var building in _buildings)
            if (building.CurrentSafetyMargin < building.MaxSafetyMargin)
                _totalRepairCost += GetRepairCost(building);

        return _totalRepairCost;
    }

    private IEnumerator SetTotalRepairCostAfterBuildingsLoaded()
    {
        yield return new WaitUntil(() => _buildingsSaver.HasLoaded);
        _priceText.text = GetTotalRepairCost().ToString();

        if (_walletPresenter.Value < _totalRepairCost)
            _priceText.color = _notAvailableRepairColor;
        else
            _priceText.color = _availableRepairColor;
    }

    private int GetRepairCost(Building building)
    {
        float damageShare = 1 - building.CurrentSafetyMargin / building.MaxSafetyMargin;
        return Mathf.RoundToInt(_buildingSlotsDatas.GetSlotBy(building.Name).JunkNeeded * damageShare * _partOfTotalBuildingCost);

    }

    private void OnBuildingRemoved() => _priceText.text = GetTotalRepairCost().ToString();
}
