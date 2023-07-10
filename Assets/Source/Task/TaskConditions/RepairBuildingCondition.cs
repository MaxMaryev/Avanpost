using System;
using System.Collections.Generic;
using UnityEngine;

public class RepairBuildingCondition : MonoBehaviour, ITaskCondition
{
    [SerializeField] private RepairAll _repairAll;
    [SerializeField] private DayCycleManager _dayCycleManager;

    public event Action Updated;

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        _repairAll.Done += OnRepairAll;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
        _repairAll.Done -= OnRepairAll;
    }

    public bool StartConditionMet()
    {
        return _repairAll.GetTotalRepairCost() > 0;
    }

    public bool CompleteConditionMet()
    {
        return _repairAll.GetTotalRepairCost() == 0;
    }

    public bool FailConditionMet()
    {
        return _dayCycleManager.CurrentTimeDay is Night;
    }

    public void Stop()
    {
        
    }

    private void OnTimeDayChanged(TimeDay timeDay) => Updated?.Invoke();

    private void OnRepairAll()
    {
        Updated?.Invoke();
    }
}
