using System.Collections.Generic;
using UnityEngine;

public class Submenu : MonoBehaviour
{
    private List<BuildingSlotButton> _buildingSlotButtons = new List<BuildingSlotButton> ();

    public IReadOnlyList<BuildingSlotButton> BuildingSlotButtons => _buildingSlotButtons;

    private void Start()
    {
        _buildingSlotButtons.AddRange(GetComponentsInChildren<BuildingSlotButton>());
        _buildingSlotButtons[0].SetToggle(true);
    }
}
