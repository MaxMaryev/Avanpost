using System;
using System.Collections;
using UnityEngine;

public class FirstEveningStage : NavigationStage
{
    [SerializeField] private Player _player;
    [SerializeField] private float _minDistanceToStartStage;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private string _messageFastComplete;

    private Coroutine _coroutine;

    protected override void OnEnter()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay is Evening)
        {
            _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;

            if (GetDistanceToTarget(_player.transform.position) > _minDistanceToStartStage)
                base.OnEnter();
            else
                FastComplete();
        }
    }

    protected override void OnExit()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        base.OnExit();
    }

    private void FastComplete()
    {
        HintHandler.Add(_messageFastComplete);
        Complete();
    }
}
