using UnityEngine;

public class WeaponCraftPanel : CraftPanelItems
{
    [SerializeField] private WeaponSlotsDatasSO _weaponSlotsDatas;
    [SerializeField] private WeaponPanelSlots _weaponPanelSlots;

    public override void Fill()
    {
        foreach (var slotData in _weaponSlotsDatas.SlotsDatas)
            if (_weaponPanelSlots.TryGetWeaponSlotBy(slotData.WeaponType) == false)
                CreateButton(slotData);
    }
}
