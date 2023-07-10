using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Pistol _pistol;
    [SerializeField] private Shotgun _shotgun;
    [SerializeField] private SniperRifle _sniperRifle;
    [SerializeField] private Hammer _hammer;
    [SerializeField] private AK _ak;
    [SerializeField] private Minigun _minigun;
    [SerializeField] private Flamethrower _flamethrower;
    [SerializeField] private WeaponPanelSlots _weaponPanelSlots;
    [SerializeField] private GameModeHandler _gameModeHandler;

    public IHandWeapon LastWeapon { get; private set; }
    public IHandWeapon CurrentWeapon { get; private set; }

    private void Start()
    {
            LastWeapon = _pistol;
            CurrentWeapon = _pistol;
            CurrentWeapon.PutInHand(true);
    }

    private void OnEnable()
    {
        foreach (var slot in _weaponPanelSlots.WeaponSlots)
            slot.Selected += OnWeaponSlotSelected;
    }

    private void OnDisable()
    {
        foreach (var slot in _weaponPanelSlots.WeaponSlots)
            slot.Selected -= OnWeaponSlotSelected;
    }

    public IHandWeapon GetWeapon(WeaponType weapon) => weapon switch
    {
        WeaponType.Hammer => _hammer,
        WeaponType.Pistol => _pistol,
        WeaponType.Shotgun => _shotgun,
        WeaponType.AK => _ak,
        WeaponType.Minigun => _minigun,
        WeaponType.Flamethrower => _flamethrower,
        WeaponType.SniperRifle => _sniperRifle,
        _ => null
    };

    private void OnWeaponSlotSelected(WeaponType weaponType, bool isReward) => ChangeWeaponTo(weaponType, isReward);

    public void ChangeWeaponTo(WeaponType weaponType, bool isReward = false)
    {
        CurrentWeapon.PutInHand(false);
        CurrentWeapon = GetWeapon(weaponType);

        if (isReward == false)
            LastWeapon = CurrentWeapon;
        else
            (CurrentWeapon as IRewardWeapon).Reload();

        CurrentWeapon.PutInHand(true);
    }
}