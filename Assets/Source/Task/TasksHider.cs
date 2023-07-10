using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksHider : MonoBehaviour
{
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private TasksShower _tasksShower;
    [SerializeField] private ButtonTask _buttonTask;

    private Queue<IHideable> _hideablesInQueue = new Queue<IHideable>();
    private List<IHideable> _processingHide = new List<IHideable>();
    private List<IHideable> _hideables = new List<IHideable>();

    private float _hideDelay = 0.15f;
    private float _autoHideDelay = 3f;
    private Coroutine _coroutineHideDelay;
    private Coroutine _coroutineAutoHideDelay;

    public event Action AllHided;

    private void OnEnable()
    {
        _buttonTask.Clicked += OnButtonTaskClicked;
        _taskBarHandler.Added += OnTaskAdded;
        _tasksShower.AllShowed += OnAllShowed;
        _taskBarHandler.Finished += OnTaskFinished;
        _taskBarHandler.Removing += OnTaskRemoving;
    }

    private void OnDisable()
    {
        _taskBarHandler.Added -= OnTaskAdded;
        _buttonTask.Clicked -= OnButtonTaskClicked;
        _tasksShower.AllShowed -= OnAllShowed;
        _taskBarHandler.Finished -= OnTaskFinished;
        _taskBarHandler.Removing -= OnTaskRemoving;
    }

    private void OnTaskFinished(IHideable hideable)
    {
        
    }

    private void OnTaskRemoving(IHideable hideable)
    {

    }

    private void OnAllShowed()
    {
        if (_coroutineAutoHideDelay != null)
            StopCoroutine(_coroutineAutoHideDelay);

        _coroutineAutoHideDelay = StartCoroutine(AutoHideDelay());
    }

    private IEnumerator AutoHideDelay()
    {
        yield return new WaitForSeconds(_autoHideDelay);

        foreach (var hideable in _hideables)
            if (_hideablesInQueue.Contains(hideable) == false)
                _hideablesInQueue.Enqueue(hideable);

        if (_coroutineHideDelay == null)
            _coroutineHideDelay = StartCoroutine(HideDelay());
    }

    private IEnumerator HideDelay()
    {
        yield return new WaitForEndOfFrame();

        while (_hideablesInQueue.Count > 0)
        {
            Hide(_hideablesInQueue.Dequeue());
            yield return new WaitForSeconds(_hideDelay);
        }

        _coroutineHideDelay = null;
    }

    private void Hide(IHideable hideable)
    {
        hideable.Hide();
    }

    private void OnTaskAdded(IHideable hideable)
    {
        _hideables.Add(hideable);
    }

    private void OnButtonTaskClicked()
    {

    }
}
