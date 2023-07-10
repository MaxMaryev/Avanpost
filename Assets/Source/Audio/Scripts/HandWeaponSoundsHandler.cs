using UnityEngine;
using System;
using System.Collections.Generic;

public class HandWeaponSoundsHandler : MonoBehaviour
{
    //    [SerializeField] private Pistol _pistol;
    //    [SerializeField] private Shotgun _shotgun;
    //    [SerializeField] private SniperRifle _SniperRifle;
    //    [SerializeField] private AK _AK;
    //    [SerializeField] private Minigun _Minigun;
    //    [SerializeField] private Flamethrower _Flamethrower;
    //    [SerializeField] private BuildingInstallation _buildingInstallation;



    //[SerializeField] EventReference pistolEvent;
    //FMOD.Studio.EventInstance pistolInstance;

    //[SerializeField] EventReference shotgunEvent;
    //FMOD.Studio.EventInstance shotgunInstance;

    //[SerializeField] EventReference SniperRifleEvent;
    //FMOD.Studio.EventInstance SniperRifleInstance;

    //[SerializeField] EventReference AKEvent;
    //FMOD.Studio.EventInstance AKInstance;

    //[SerializeField] EventReference minigunEvent;
    //FMOD.Studio.EventInstance minigunInstance;

    //[SerializeField] EventReference FlamethrowerEvent;
    //FMOD.Studio.EventInstance FlamethrowerInstance;

    //[SerializeField] EventReference ShotguntrapEvent;
    //FMOD.Studio.EventInstance ShotguntrapInstance;

    //[SerializeField] EventReference BarrelGunEvent;
    //FMOD.Studio.EventInstance BarrelGunInstance;

    //[SerializeField] EventReference RocketLauncherEvent;
    //FMOD.Studio.EventInstance RocketLauncherInstance;

    //[SerializeField] EventReference MineEvent;
    //FMOD.Studio.EventInstance MineInstance;



    //    void Start()
    //    {
    //        pistolInstance = FMODUnity.RuntimeManager.CreateInstance(pistolEvent);

    //        shotgunInstance = FMODUnity.RuntimeManager.CreateInstance(shotgunEvent);

    //        SniperRifleInstance = FMODUnity.RuntimeManager.CreateInstance(SniperRifleEvent);

    //        AKInstance = FMODUnity.RuntimeManager.CreateInstance(AKEvent);

    //        minigunInstance = FMODUnity.RuntimeManager.CreateInstance(minigunEvent);

    //        FlamethrowerInstance = FMODUnity.RuntimeManager.CreateInstance(FlamethrowerEvent);

    //        ShotguntrapInstance = FMODUnity.RuntimeManager.CreateInstance(ShotguntrapEvent);

    //        BarrelGunInstance = FMODUnity.RuntimeManager.CreateInstance(BarrelGunEvent);

    //        RocketLauncherInstance = FMODUnity.RuntimeManager.CreateInstance(RocketLauncherEvent);

    //        MineInstance = FMODUnity.RuntimeManager.CreateInstance(MineEvent);
    //    }


    //    private void OnEnable()
    //    {
    //        _buildingInstallation.Done += OnBuildingDone;
    //        _pistol.Shot += OnPistolShot;
    //        _shotgun.Shot += OnShotgunShot;
    //        _SniperRifle.Shot += OnSniperRifleShot;
    //        _AK.Shot += OnAKShot;
    //        _Minigun.Shot += OnMinigunShot;
    //        _Flamethrower.Shot += OnFlamethrowerShot;
    //    }

    //    private void OnDisable()
    //    {
    //        _buildingInstallation.Done -= OnBuildingDone;
    //        _pistol.Shot -= OnPistolShot;
    //        _shotgun.Shot -= OnShotgunShot;
    //        _SniperRifle.Shot -= OnSniperRifleShot;
    //        _AK.Shot -= OnAKShot;
    //        _Minigun.Shot -= OnMinigunShot;
    //        _Flamethrower.Shot -= OnFlamethrowerShot;
    //    }

    //    private void OnBuildingDone(Building building)
    //    {

    //        if (building is Turret turret)
    //        {
    //            Debug.Log("text1");
    //            if (turret.FiringWeapon is ShotgunTrap shotgunTrap)
    //            {
    //                shotgunTrap.Shot1 += OnShotguntrapShot;
    //                building.Destroying += OnBuildingDestroying;
    //            }
    //            else if (turret.FiringWeapon is TwoBarrelsGun barrelsGun)
    //            {
    //                barrelsGun.Shot1 += OnBarrelGunShot;
    //                building.Destroying += OnBuildingDestroying;
    //            }
    //            else if (turret.FiringWeapon is RocketLauncher rocketLauncher)
    //            {
    //                rocketLauncher.Shot1 += OnRocketLauncherShot;
    //                building.Destroying += OnBuildingDestroying;
    //            }
    //        }
    //        else if (building is Trap trap)
    //            trap.Mine.Exploded += OnMineExploded;
    //    }

    //    private void OnBuildingDestroying(Building building)
    //    {
    //        building.Destroying -= OnBuildingDestroying;

    //        if (building is Turret turret)
    //        {
    //            if (turret.FiringWeapon is ShotgunTrap shotgunTrap)
    //                shotgunTrap.Shot -= OnShotguntrapShot;
    //            else if (turret.FiringWeapon is TwoBarrelsGun barrelsGun)
    //                barrelsGun.Shot -= OnBarrelGunShot;
    //        }
    //    }

    //    private void OnMineExploded(Mine mine)
    //    {
    //        mine.Exploded -= OnMineExploded;

    //        FMODUnity.RuntimeManager.PlayOneShot(MineEvent, mine.transform.position);
    //        //mine.transform.position;
    //    }

    //    private void OnPistolShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(pistolEvent, gameObject.transform.position);
    //    }

    //    private void OnShotgunShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(shotgunEvent, gameObject.transform.position);
    //    }

    //    private void OnSniperRifleShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(SniperRifleEvent, gameObject.transform.position);
    //    }

    //    private void OnAKShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(AKEvent, gameObject.transform.position);
    //    }
    //    private void OnMinigunShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(minigunEvent, gameObject.transform.position);
    //    }

    //    private void OnFlamethrowerShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(FlamethrowerEvent, gameObject.transform.position);
    //    }
    //    private void OnShotguntrapShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(ShotguntrapEvent, firingWeapon.transform.position);
    //        Debug.Log("text");
    //    }

    //    private void OnBarrelGunShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(BarrelGunEvent, firingWeapon.transform.position);
    //    }

    //    private void OnRocketLauncherShot(FiringWeapon firingWeapon)
    //    {
    //        FMODUnity.RuntimeManager.PlayOneShot(RocketLauncherEvent, firingWeapon.transform.position);
    //    }
}
