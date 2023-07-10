using System;
using UnityEngine;

public class CraftWeapon : CraftItem
{
    [SerializeField] private WeaponPanelSlots _weaponPanelSlots;
    [SerializeField] private WeaponCraftPanel _weaponCraftPanel;

    public event Action<WeaponSlotData> Crafted;

    protected override void OnCraftedItem(ICraftableItem craftableItem)
    {
        if (craftableItem is WeaponSlotData)
        {
            WeaponSlotData weaponSlotData = (WeaponSlotData)craftableItem;
            _weaponPanelSlots.AddWeapon(weaponSlotData.WeaponType, weaponSlotData.Icon);
            _weaponCraftPanel.RemoveButton(craftableItem);
            Crafted?.Invoke(weaponSlotData);
        }
    }
}
