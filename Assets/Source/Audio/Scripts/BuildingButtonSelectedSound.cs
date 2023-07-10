using System;
using UnityEngine;

public class BuildingButtonSelectedSound : PlayClickSound
{
    //[SerializeField] private BuildingSlotButton _buildingSlotButton;
    [SerializeField] private BuildingsPanel _buildingsPanel;

    private void OnEnable()
    {
        //_buildingSlotButton.Clicked += OnBuildingSlotClicked;
        foreach (var slot in _buildingsPanel.SlotButtons)
            slot.Clicked += OnClicked;
    }

    private void OnDisable()
    {
        foreach (var slot in _buildingsPanel.SlotButtons)
            slot.Clicked -= OnClicked;
        //_buildingSlotButton.Clicked -= OnBuildingSlotClicked;
    }

    private void OnClicked() => PlaySound();

    private void OnBuildingSlotClicked() => PlaySound();
}
