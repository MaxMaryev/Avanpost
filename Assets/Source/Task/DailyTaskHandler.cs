using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailyTaskHandler : MonoBehaviour
{
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private List<DailyTask> _dailyTasks = new List<DailyTask>();
    [SerializeField] private ObjectsPool[] _objectsPools;

    private List<ITaskActivable> _tasksActivable = new List<ITaskActivable>();
    private Coroutine _coroutine;
    private Coroutine _coroutineWaitDeathAllEnemy;

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay is LateNight)
        {
            CleareActivableTask();

            if (_coroutineWaitDeathAllEnemy != null)
                StopCoroutine(_coroutineWaitDeathAllEnemy);

            _coroutineWaitDeathAllEnemy = StartCoroutine(WaitDeathAllEnemy());
        } 
        else
        {
            if (_coroutineWaitDeathAllEnemy != null)
                StopCoroutine(_coroutineWaitDeathAllEnemy);

            _coroutineWaitDeathAllEnemy = null;
        }

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_coroutineWaitDeathAllEnemy == null)
            _coroutine = StartCoroutine(CheckTasksForStart());
    }

    private IEnumerator CheckTasksForStart()
    {
        yield return new WaitForEndOfFrame();
       
        for (int i = 0; i < _dailyTasks.Count; i++)
        {
            if (_dailyTasks[i].CanActivate())
            {
                for (int j = 0; j < _dailyTasks[i].Taskables.Count; j++)
                {
                    if (_tasksActivable.Contains(_dailyTasks[i].Taskables[j]) == false)
                    {
                        if (_dailyTasks[i].Taskables[j].TryActivate())
                        {
                            _taskBarHandler.AddTask(_dailyTasks[i].Taskables[j]);
                            _tasksActivable.Add(_dailyTasks[i].Taskables[j]);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator WaitDeathAllEnemy()
    {
        yield return new WaitUntil(() => GetLenghtPools() == 0);
        _coroutine = StartCoroutine(CheckTasksForStart());
        _coroutineWaitDeathAllEnemy = null;
    }

    private int GetLenghtPools() => _objectsPools.Where(x => x.Enabled.Count > 0).Count();

    private void CleareActivableTask()
    {
        _tasksActivable.Clear();
    }
}
