using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "BuildingSlotsDatasSO", menuName = "Slots/Buildings", order = 66)]
public class BuildingSlotsDatasSO : ScriptableObject
{
    [SerializeField] private List<BuildingSlotData> _datas;

    public IReadOnlyList<BuildingSlotData> SlotsDatas => _datas;

    public BuildingSlotData GetSlotBy(BuildingName buildingName)
    {
        foreach (var slotData in _datas)
            if (slotData.Name == buildingName)
                return slotData;

        return null;
    }
}