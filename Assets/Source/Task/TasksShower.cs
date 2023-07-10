using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TasksShower : MonoBehaviour
{
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private ButtonTask _buttonTask;
    [SerializeField] private TasksHider _tasksHider;

    private Queue<IShowable> _showablesInQueue = new Queue<IShowable>();
    private List<IShowable> _processingShow = new List<IShowable>();
    private List<IShowable> _showables = new List<IShowable>();

    private float _showDelay = 0.15f;
    private float _autoHideDelay = 10f;
    private Coroutine _coroutineShowDelay;
    private Coroutine _coroutineHideDelay;

    public event Action AllHided;
    public event Action AllShowed;

    private void OnEnable()
    {
        _buttonTask.Clicked += OnButtonTaskClicked;
        _taskBarHandler.Added += OnTaskAdded;
        _taskBarHandler.Finished += OnTaskFinished;
        _taskBarHandler.Removing += OnTaskRemoving;
        _tasksHider.AllHided += OnHided;
    }

    private void OnDisable()
    {
        _tasksHider.AllHided -= OnHided;
        _taskBarHandler.Added -= OnTaskAdded;
        _buttonTask.Clicked -= OnButtonTaskClicked;
        _taskBarHandler.Finished -= OnTaskFinished;
        _taskBarHandler.Removing -= OnTaskRemoving;
    }

    private void OnHided()
    {
        
    }

    private void OnTaskFinished(IShowable showable)
    {
        foreach (var show in _showables)
        {
            if (_showablesInQueue.Contains(show) == false && _processingShow.Contains(show) == false)
                _showablesInQueue.Enqueue(show);
        }


        if (_coroutineShowDelay == null)
            _coroutineShowDelay = StartCoroutine(ShowDelay());
    }

    private void OnTaskRemoving(IShowable showable)
    {
        _showables.Remove(showable);
    }

    private void OnButtonTaskClicked()
    {
        foreach (var showable in _showables)
        {
            if (_showablesInQueue.Contains(showable) == false && _processingShow.Contains(showable) == false && showable.IsShow == false)
                _showablesInQueue.Enqueue(showable);
        }

        if (_showablesInQueue.Count > 0)
        {
            if (_coroutineShowDelay == null)
                _coroutineShowDelay = StartCoroutine(ShowDelay());
        }
    }

    private void OnTaskAdded(IShowable showable)
    {
        _showables.Add(showable);
        _showablesInQueue.Enqueue(showable);

        if (_coroutineShowDelay == null)
            _coroutineShowDelay = StartCoroutine(ShowDelay());
    }

    private IEnumerator ShowDelay()
    {
        yield return new WaitForEndOfFrame();

        while (_showablesInQueue.Count > 0)
        {
            Show(_showablesInQueue.Dequeue());
            yield return new WaitForSeconds(_showDelay);
        }

        _coroutineShowDelay = null;
    }

    private void Show(IShowable showable)
    {
        _processingShow.Add(showable);
        showable.Showed += OnShowed;
        showable.Show();
    }

    private void OnShowed(IShowable showable)
    {
        showable.Showed -= OnShowed;
        _processingShow.Remove(showable);

        if (_processingShow.Count == 0)
            AllShowed?.Invoke();
    }
}
