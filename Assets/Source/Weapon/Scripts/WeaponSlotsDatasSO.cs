using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "WeaponSlotsDatasSO", menuName = "Slots/Weapons", order = 66)]
public class WeaponSlotsDatasSO : ScriptableObject
{
    [SerializeField] private List<WeaponSlotData> _datas;

    public IReadOnlyList<WeaponSlotData> SlotsDatas => _datas;

    public Sprite GetIconBy(WeaponType weaponType)
    {
        foreach (var slotData in _datas)
            if (slotData.WeaponType == weaponType)
                return slotData.Icon;

        return null;
    }
}