using System;
using UnityEngine;

public abstract class Task : MonoBehaviour, ITaskable, ITaskActivable
{
    [SerializeField] private MonoBehaviour _taskCondition;
    [SerializeField] private TaskInfo _taskInfo;
    [SerializeField] private TaskStatus _taskStatus;
    [SerializeField] private TaskReward _taskReward;

    public TaskStatus TaskStatus => _taskStatus;
    public TaskInfo TaskInfo => _taskInfo;
    public TaskReward TaskReward => _taskReward;
    protected ITaskCondition Condition => (ITaskCondition)_taskCondition;

    public event Action<ITaskable> Finished;

    private void OnValidate()
    {
        if (_taskCondition && !(_taskCondition is ITaskCondition))
        {
            Debug.LogError(nameof(_taskCondition) + " needs to implement " + nameof(ITaskCondition));
            _taskCondition = null;
        }
    }

    protected virtual void Awake()
    {
        _taskCondition.gameObject.SetActive(false);
    }

    public bool TryActivate()
    {
        if (Condition.StartConditionMet() == false)
            return false;

        _taskStatus.Changed += OnTaskStatusChanged;
        Activate();
        gameObject.SetActive(true);
        return true;
    }

    protected abstract void Activate();

    private void OnTaskStatusChanged(TaskStatusType taskStatusType)
    {
        _taskStatus.Changed -= OnTaskStatusChanged;
        Finished?.Invoke(this);
        gameObject.SetActive(false);
    }
}
