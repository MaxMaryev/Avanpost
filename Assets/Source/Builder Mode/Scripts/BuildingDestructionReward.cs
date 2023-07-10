using UnityEngine;

public class BuildingDestructionReward : MonoBehaviour
{
    //[SerializeField] private GarbageHandler _garbageHandler;
    //[SerializeField] private BuildingSlotsDatasSO _buildingSlotsDatas;
    //[SerializeField] private WalletPresenter _walletPresenter;

    //private void OnBuildingSelected(Building building)
    //{
    //    building.Destroying += OnBuildingDestroying;
    //}

    //private void OnBuildingDestroying(Building building, bool isRefund)
    //{
    //    building.Destroying -= OnBuildingDestroying;
        
    //    if (isRefund)
    //    {
    //        int refund = _buildingSlotsDatas.GetSlotBy(building.Name).JunkNeeded / 2;
    //        _garbageHandler.SpawnGarbage(building.transform.position, building.transform.position, refund);
    //    }
    //}

    //private void OnBuildingCancelled(BuildingName buildingName)
    //{
    //    int refund = _buildingSlotsDatas.GetSlotBy(buildingName).JunkNeeded;
    //    _walletPresenter.AddResource(refund);
    //}
}
