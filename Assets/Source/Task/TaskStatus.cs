using System;
using UnityEngine;

public class TaskStatus : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _taskCondition;

    private ITaskCondition _condition => (ITaskCondition) _taskCondition;

    public TaskStatusType Type { get; private set; }
    
    public event Action<TaskStatusType> Changed;

    private void OnValidate()
    {
        if (_taskCondition && !(_taskCondition is ITaskCondition))
        {
            Debug.LogError(nameof(_taskCondition) + " needs to implement " + nameof(ITaskCondition));
            _taskCondition = null;
        }
    }

    private void OnEnable()
    {
        _condition.Updated += OnUpdated;
        Type = TaskStatusType.InProgress;
    }

    private void OnDisable()
    {
        _condition.Updated -= OnUpdated;
    }

    private void OnUpdated()
    {
        if (Type != TaskStatusType.InProgress)
            return;
        if (_condition.CompleteConditionMet())
            Change(TaskStatusType.Completed);
        else if (_condition.FailConditionMet())
            Change(TaskStatusType.Failed);
    }

    private void Change(TaskStatusType status)
    {
        Debug.Log($"Changed {status}. Condition: "+_taskCondition);
        Type = status;
        Changed?.Invoke(Type);
    }
}
