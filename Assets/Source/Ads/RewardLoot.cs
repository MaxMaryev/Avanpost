using System;
using System.Collections;
using UnityEngine;

public class RewardLoot : RewardAdvertising
{
    [SerializeField] private int _bonus;
    [SerializeField] private float _duration;
    [SerializeField] private int _dayNumberToActivate;

    private Coroutine _coroutineActivateButton;
    private bool _canShowButton => DayCycleManager.CurrentTimeDay is Night == false &&
                                    DayCycleManager.CurrentTimeDay is LateNight == false &&
                                    DayCycleManager.CurrentDay >= _dayNumberToActivate;

    public event Action<int, float> RewardActivated;

    protected override void OnEnable()
    {
        base.OnEnable();

        DayCycleManager.TimeDayChanged += OnTimeDayChanged;

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        DayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    protected override void Start()
    {
        OnTimeDayChanged(DayCycleManager.CurrentTimeDay);
    }

    protected override void OnReward()
    {
        RewardActivated?.Invoke(_bonus, _duration);

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
