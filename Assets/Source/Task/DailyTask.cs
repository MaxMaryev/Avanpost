using System.Collections.Generic;
using UnityEngine;

public class DailyTask : MonoBehaviour
{
    [SerializeField] private int _minDayToStart;
    [SerializeField] private List<TimeDay> _timeDaysToStart = new List<TimeDay>();
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private List<Task> _tasks = new List<Task>();

    public IReadOnlyList<ITaskActivable> Taskables => _tasks;

    public bool CanActivate()
    {
        return _dayCycleManager.CurrentDay >= _minDayToStart && _timeDaysToStart.Contains(_dayCycleManager.CurrentTimeDay);
    }
}
