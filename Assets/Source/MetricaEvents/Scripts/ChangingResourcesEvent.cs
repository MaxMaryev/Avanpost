using GameAnalyticsSDK;
using System;
using System.Linq;
using UnityEngine;

public class ChangingResourcesEvent : MonoBehaviour
{
    [SerializeField] private GarbageReward _garbageReward;
    [SerializeField] private WeaponSlotsDatasSO _weaponSlotsDatas;
    [SerializeField] private BuildingSlotsDatasSO _buildingSlotsDatas;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private CraftWeapon _craftWeapon;

    private void OnEnable()
    {
        _craftWeapon.Crafted += OnWeaponCrafted;
        _garbageReward.Rewarded += OnGarbageRewarded;
        _buildingInstallation.Crafted += OnCrafted;
    }

    private void OnDisable()
    {
        _craftWeapon.Crafted -= OnWeaponCrafted;
        _garbageReward.Rewarded -= OnGarbageRewarded;
        _buildingInstallation.Crafted -= OnCrafted;
    }

    private void OnWeaponCrafted(WeaponSlotData weaponSlotData)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "trash", weaponSlotData.JunkNeeded, "CraftMenu", weaponSlotData.WeaponType.ToString());
    }

    private void OnCrafted(BuildingSlotData buildingSlotData)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "trash", buildingSlotData.JunkNeeded, "CraftMenu", buildingSlotData.Name.ToString());
        Debug.Log(buildingSlotData.JunkNeeded + " "+ buildingSlotData.Name.ToString());
    }

    private void OnGarbageRewarded(int value)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "trash", value, "loot", "garbage");
    }
}
