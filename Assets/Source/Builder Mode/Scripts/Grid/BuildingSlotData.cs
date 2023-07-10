using System;
using UnityEngine;

[Serializable]
public class BuildingSlotData
{
    [field: SerializeField] public BuildingName Name { get; private set; } = BuildingName.None;
    [field: SerializeField] public BuildingClass Class { get; private set; } = BuildingClass.None;
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int JunkNeeded { get; private set; }
    [field: SerializeField] public int MaxSafetyMargin { get; private set; }
    [field: SerializeField] public Building BuildingTemplate { get; private set; }
}
