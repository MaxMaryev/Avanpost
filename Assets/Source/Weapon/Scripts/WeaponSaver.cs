using System.Collections.Generic;
using UnityEngine;

public class WeaponSaver : MonoBehaviour
{
    [SerializeField] private WeaponPanelSlots _weaponPanelSlots;
    [SerializeField] private WeaponSlotsDatasSO _buildingSlots;

    private void OnEnable() => Load();

    public void Save(IReadOnlyList<WeaponType> weaponTypes)
    {
        WeaponsSaveData weaponsSaveData = new WeaponsSaveData();

        foreach (var weapon in weaponTypes)
            weaponsSaveData.Add(weapon);

        weaponsSaveData.Save();
    }

    private void Load()
    {
        WeaponsSaveData weaponsSaveData = new WeaponsSaveData();
        weaponsSaveData.Load();

        for (int i = 1; i < weaponsSaveData.WeaponTypes.Count; i++)
        {
            WeaponType weaponType = (WeaponType)weaponsSaveData.WeaponTypes[i];
            Sprite icon = _buildingSlots.GetIconBy(weaponType);
            _weaponPanelSlots.WeaponSlots[i].Init(icon, weaponType);
            _weaponPanelSlots.WeaponSlots[i].gameObject.SetActive(true);
        }
    }
}

public class WeaponsSaveData : SavedObject<WeaponsSaveData>
{
    [field: SerializeField] public List<int> WeaponTypes { get; private set; } = new List<int>();

    public WeaponsSaveData() : base(nameof(WeaponsSaveData)) { }

    public void Add(WeaponType weaponType) => WeaponTypes.Add((int)weaponType);

    protected override void OnLoad(WeaponsSaveData loadedObject)
    {
        WeaponTypes = loadedObject.WeaponTypes;
    }
}
