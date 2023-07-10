using System;
using System.Collections;
using UnityEngine;

public class TimeDayFreezeStage : TutorialStage
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private TimeDay _timeDay;
    [SerializeField] private bool _isFreeze = true;
    [SerializeField] private bool _isRewind = false;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        StartCoroutine(Wait());
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay == _timeDay)
        {
            _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
            Completed?.Invoke(this);
        }
    }

    protected override void OnExit()
    {
        if (_isFreeze)
            _dayCycleManager.FreezeTime();
        else
            _dayCycleManager.UnFreezeTime();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();

        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;

        if (_dayCycleManager.CurrentTimeDay != _timeDay)
        {
            if (_isRewind == false)
                _dayCycleManager.SkipDayBeforeTo(_timeDay);
            else
                _dayCycleManager.RewindTime(_timeDay.EndNormalizedTime);
        }
        else
        {
            OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
        }
    }
}
