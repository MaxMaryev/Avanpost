using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponPanelSlots : MonoBehaviour
{
    [SerializeField] private List<WeaponPanelSlot> _weaponSlots;
    [SerializeField] private WeaponSaver _weaponSaver;
    [SerializeField] private RewardMinigun _rewardMinigun;
    [SerializeField] private RewardFlamethrower _rewardFlamethrower;
    [SerializeField] private WeaponHandler _weaponHandler;

    //public event Action RewardWeaponTimeEnded;

    public IReadOnlyList<WeaponPanelSlot> WeaponSlots => _weaponSlots;

    private void OnEnable()
    {
        _rewardMinigun.GotReward += OnGotRewardWeapon;
        _rewardFlamethrower.GotReward += OnGotRewardWeapon;
    }

    private void OnDisable()
    {
        _rewardMinigun.GotReward -= OnGotRewardWeapon;
        _rewardFlamethrower.GotReward -= OnGotRewardWeapon;
    }

    public WeaponPanelSlot TryGetWeaponSlotBy(WeaponType weaponType)
    {
        return _weaponSlots.FirstOrDefault(slot => slot.WeaponType == weaponType);
    }

    public IReadOnlyList<WeaponType> GetAvailableWeapons()
    {
        return _weaponSlots.Where(slot => slot.gameObject.activeSelf == true && slot.IsReward == false).Select(weapon => weapon.WeaponType).ToList();
    }

    public void AddWeapon(WeaponType weaponType, Sprite icon)
    {
        WeaponPanelSlot weaponSlot = _weaponSlots.FirstOrDefault(slot => slot.gameObject.activeSelf == false);
        weaponSlot.Init(icon, weaponType);
        weaponSlot.gameObject.SetActive(true);
        _weaponSaver.Save(GetAvailableWeapons());
    }

    private void OnGotRewardWeapon(WeaponType weaponType, Sprite icon, float duration)
    {
        if (TryGetWeaponSlotBy(weaponType))
            return;

        WeaponPanelSlot weaponSlot = _weaponSlots.FirstOrDefault(slot => slot.gameObject.activeSelf == false);
        weaponSlot.Init(icon, weaponType, isReward: true);
        weaponSlot.Select();
        weaponSlot.gameObject.SetActive(true);

        StartCoroutine(ClearSlot(weaponSlot.WeaponType, duration));
    }

    private IEnumerator ClearSlot(WeaponType deletingWeaponType, float duration)
    {
        yield return new WaitForSeconds(duration);

        WeaponPanelSlot currentSlot = TryGetWeaponSlotBy(_weaponHandler.CurrentWeapon.WeaponType);
        WeaponPanelSlot deletingSlot = TryGetWeaponSlotBy(deletingWeaponType);

        if (currentSlot.WeaponType == deletingSlot.WeaponType)
            TryGetWeaponSlotBy(_weaponHandler.LastWeapon.WeaponType).Select();

        deletingSlot.Delete();
        deletingSlot.gameObject.SetActive(false);
        //RewardWeaponTimeEnded?.Invoke();
    }
}
