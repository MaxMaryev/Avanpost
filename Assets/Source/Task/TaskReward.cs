using System;
using UnityEngine;

public class TaskReward : MonoBehaviour
{
    [SerializeField] private TaskStatus _taskStatus;
    [SerializeField] WalletPresenter _walletPresenter;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private int _taskRewardMultiplier;
    [SerializeField] private int _baseValueReward;
    [SerializeField] private int _dailyMultiplier;

    private void OnEnable()
    {
        _taskStatus.Changed += OnTaskStatusChanged;
    }

    private void OnDisable()
    {
        _taskStatus.Changed -= OnTaskStatusChanged;
    }

    private void OnTaskStatusChanged(TaskStatusType taskStatus)
    {
        if (taskStatus == TaskStatusType.Completed)
            Reward();
    }

    private void Reward()
    {
        _walletPresenter.AddResource(GetAmountReward());
    }

    public int GetAmountReward() => (_baseValueReward + (_dailyMultiplier * _dayCycleManager.CurrentDay)) * _taskRewardMultiplier;
}
