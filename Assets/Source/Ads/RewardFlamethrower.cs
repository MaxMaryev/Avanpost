using UnityEngine;

public class RewardFlamethrower : RewardWeapon
{
    [SerializeField] private Flamethrower _flamethrower;
    [SerializeField] private float _firstShowDelay;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private int _dayToStart;

    protected override WeaponType WeaponType => _flamethrower.WeaponType;

    protected override void Start()
    {
        if (_dayCycleManager.CurrentDay >= _dayToStart)
        {
            ShowButton();
        }
        else
        {
            HideButton();
            _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        }
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (_dayCycleManager.CurrentDay >= _dayToStart)
        {
            ShowButton();
            _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
        }
    }

    protected override void OnReward()
    {
        _flamethrower.ReloadInstanly();
        base.OnReward();
    }
}
