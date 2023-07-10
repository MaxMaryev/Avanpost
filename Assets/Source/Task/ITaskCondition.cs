using System;
using UnityEngine;

public interface ITaskCondition 
{
    public event Action Updated;
    public bool StartConditionMet();
    public bool CompleteConditionMet();
    public bool FailConditionMet();
}
