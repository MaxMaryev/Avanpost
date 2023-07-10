using System;
using UnityEngine;
using UnityEngine.UI;

public class WarningAboutHidedTasks : MonoBehaviour
{
    [SerializeField] private Image _iconWarning;
    [SerializeField] private TasksShower _tasksShower;
    [SerializeField] private TaskBarHandler _taskBarHandler;

    private void OnEnable()
    {
        _tasksShower.AllShowed += OnAllShowed;
        _tasksShower.AllHided += OnAllHided;
        _taskBarHandler.Removed += OnTaskRemoved;
    }

    private void OnDisable()
    {
        _taskBarHandler.Removed -= OnTaskRemoved;
        _tasksShower.AllShowed -= OnAllShowed;
        _tasksShower.AllHided -= OnAllHided;
    }

    private void OnTaskRemoved()
    {
        if (_taskBarHandler.CountActiveTasks == 0)
            _iconWarning.enabled = false;
    }

    private void OnAllHided()
    {
        if (_taskBarHandler.CountActiveTasks > 0)
            _iconWarning.enabled = true;
    }

    private void OnAllShowed()
    {
        _iconWarning.enabled = false;
    }
}
