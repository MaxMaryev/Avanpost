using UnityEngine;

public class RewardMinigun : RewardWeapon
{
    [SerializeField] private Minigun _minigun;

    protected override WeaponType WeaponType => _minigun.WeaponType;

    protected override void OnRewardButtonClicked()
    {
        if (DayCycleManager.CurrentDay == 1)
        {
            OnReward();
            DeactivateButton();
            return;
        }

        base.OnRewardButtonClicked();
    }

    protected override void OnReward()
    {
        _minigun.ReloadInstanly();
        base.OnReward();
    }
}
