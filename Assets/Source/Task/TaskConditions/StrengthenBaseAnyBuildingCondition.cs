using System;
using UnityEngine;

public class StrengthenBaseAnyBuildingCondition : MonoBehaviour, ITaskCondition
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private BuildingInstallation _buildingInstallation;

    private bool _isPlaced;

    public event Action Updated;

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        _buildingInstallation.Done += OnBuildingPlaced;
        _isPlaced = false;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
        _buildingInstallation.Done -= OnBuildingPlaced;
    }

    public bool CompleteConditionMet()
    {
        return _isPlaced;
    }

    public bool FailConditionMet()
    {
        return _dayCycleManager.CurrentTimeDay is Night;
    }

    public bool StartConditionMet()
    {
        return true;
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        Updated?.Invoke();
    }

    private void OnBuildingPlaced(Building building)
    {
        _isPlaced = true;
        Updated?.Invoke();
    }
}
