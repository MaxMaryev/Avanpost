using System.Collections.Generic;
using UnityEngine;

public class BuildingsInitializer : MonoBehaviour
{
    [SerializeField] private PlayerAnimationsController _playerAnimationsController;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private LightHandler _lightHandler;
    [SerializeField] private List<ObjectsPool> _zombiePools;
    [SerializeField] private Transform _houseTransform;

    private void OnEnable() => _buildingInstallation.Done += OnBuildingInstallationDone;

    private void OnDisable() => _buildingInstallation.Done -= OnBuildingInstallationDone;

    public void OnBuildingInstallationDone(Building building)
    {
        if (building.TryGetComponent(out ShotgunTrap shotgunTrap))
        {
            shotgunTrap.InitReloadAnimationsHandler(_playerAnimationsController);
            shotgunTrap.InitZombiesPool(_zombiePools.ToArray());
        }

        if (building.TryGetComponent(out TwoBarrelsGun twoBarrelsGun))
        {
            twoBarrelsGun.InitReloadAnimationsHandler(_playerAnimationsController);
            twoBarrelsGun.InitZombiesPool(_zombiePools.ToArray());
        }

        if (building.TryGetComponent(out RocketLauncher rocketLauncher))
        {
            rocketLauncher.InitReloadAnimationsHandler(_playerAnimationsController);
            rocketLauncher.InitZombiesPool(_zombiePools.ToArray());
        }

        if (building.TryGetComponent(out OilLamp oilLamp))
            oilLamp.Init(_lightHandler);

        if (building.TryGetComponent(out Gate gate))
            gate.InitHousePosition(_houseTransform.position);

        if (building.TryGetComponent(out Fence fence))
            fence.InitHousePosition(_houseTransform.position);
    }
}
