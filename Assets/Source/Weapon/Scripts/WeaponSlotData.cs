using UnityEngine;

[System.Serializable]
public class WeaponSlotData : ICraftableItem
{
    [field: SerializeField] public WeaponType WeaponType { get; private set; } = WeaponType.None;
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int JunkNeeded { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Poster { get; private set; }
    [field: SerializeField] public string Damage { get; private set; }
    [field: SerializeField] public string RateFire { get; private set; }
    [field: SerializeField] public string StoreCapacity { get; private set; }
    [field: SerializeField] public string Reload { get; private set; }

    public string Name => WeaponType.ToString();
}
