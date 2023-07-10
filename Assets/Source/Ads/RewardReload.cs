using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardReload : RewardAdvertising
{
    [SerializeField] private ContainersHandler _containersHandler;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private int _minDayToStart = 2;

    private Coroutine _coroutineActivateButton;

    private bool _canShowButton => _dayCycleManager.CurrentTimeDay is Night && _dayCycleManager.CurrentDay >= _minDayToStart;

    protected override void OnEnable()
    {
        base.OnEnable();
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    protected override void Start()
    {
        base.Start();
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    protected override void OnReward()
    {
        IReadOnlyList<Turret> turrets = _containersHandler.GetTurrets();

        foreach (var turret in turrets)
            turret.FiringWeapon.ReloadInstanly();

        if (_coroutineActivateButton == null)
            _coroutineActivateButton = StartCoroutine(ActivateButtonDelay());
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (_canShowButton == false)
        {
            HideButton();
            return;
        }

        if (IsShowed == false)
            ShowButton();
    }

    private IEnumerator ActivateButtonDelay()
    {
        yield return new WaitForSeconds(Cooldown);

        ActivateButton();

        _coroutineActivateButton = null;
    }
}
