using System;
using UnityEngine;

public abstract class RewardWeapon : RewardAdvertising
{
    [SerializeField] private float _duration;
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private WeaponPanelSlots _weaponPanelSlots;

    protected abstract WeaponType WeaponType { get; }

    public event Action<WeaponType, Sprite, float> GotReward;

    protected override void ShowButton()
    {
        if (_weaponPanelSlots.TryGetWeaponSlotBy(WeaponType))
            return;

        base.ShowButton();
    }

    protected override void OnReward()
    {
        GotReward?.Invoke(WeaponType, _weaponSprite, _duration);
        Invoke(nameof(ActivateButton), Cooldown);
    }

    private void OnWeaponBuyed(WeaponType buyedWeaponType, Sprite _)
    {
        if(WeaponType == buyedWeaponType)
            HideButton();
    }
}
