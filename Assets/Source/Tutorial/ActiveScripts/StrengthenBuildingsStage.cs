using System;
using UnityEngine;

public class StrengthenBuildingsStage : TutorialStage
{
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private Task _task;
    [SerializeField] private GameObject _rootCraftMenuPoint;

    private ITaskActivable _taskActivable => _task;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _taskActivable.TryActivate();
        _taskBarHandler.AddTask(_taskActivable);
        _taskActivable.Finished += OnTaskFinished;
        _rootCraftMenuPoint.gameObject.SetActive(true);
    }

    private void OnTaskFinished(ITaskable taskable)
    {
        taskable.Finished -= OnTaskFinished;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        
    }
}
