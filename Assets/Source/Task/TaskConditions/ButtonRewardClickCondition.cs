using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRewardClickCondition : MonoBehaviour, ITaskCondition, ICountable
{
    [SerializeField] private List<RewardAdvertising> _rewards;
    [SerializeField] private int _targetValueClicks;
    [SerializeField] private DayCycleManager _dayCycleManager;

    private int _currentValueClicks;

    public int TargetNumber => _targetValueClicks;

    public int CurrentNumber => _currentValueClicks;

    public event Action Updated;

    private void OnEnable()
    {
        _currentValueClicks = 0;

        foreach (var reward in _rewards)
            reward.RewardClicked += OnRewardClicked;

        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    private void OnDisable()
    {
        foreach (var button in _rewards)
            button.RewardClicked -= OnRewardClicked;

        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    public bool CompleteConditionMet() => _currentValueClicks == _targetValueClicks;

    public bool FailConditionMet() => _dayCycleManager.CurrentTimeDay is LateNight;

    public bool StartConditionMet() => true;

    private void OnRewardClicked()
    {
        _currentValueClicks++;
        Updated?.Invoke();
    }

    private void OnTimeDayChanged(TimeDay timeDay) => Updated?.Invoke();
}
