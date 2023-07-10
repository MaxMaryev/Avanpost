using System;
using UnityEngine;

public interface ITaskable
{
    public TaskStatus TaskStatus { get; }
    public TaskInfo TaskInfo { get; }
    public TaskReward TaskReward { get; }
    public event Action<ITaskable> Finished;
}
