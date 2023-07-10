using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TaskView : MonoBehaviour, IShowable, IHideable
{
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TaskRewardPanelView _rewardPanelView;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _timeShow;
    [SerializeField] private Transform _panel;

    private Coroutine _coroutineShowDealy;
    private Coroutine _coroutineHideDelay;
    private ITaskable _taskable;
    private bool _isShow = false;
    private bool _isHide = true;
    private float _currentTime = 0;

    public bool IsShow => _isShow;
    public ITaskable Taskable => _taskable;

    public event Action<IHideable> Hided;
    public event Action<IShowable> Showed;

    public void Init(ITaskable taskable)
    {
        _taskable = taskable;
        _taskable.TaskInfo.Updated += OnTaskInfoUpdated;
        _rewardPanelView.Init(_taskable);
        _panel.transform.localPosition = Vector3.zero + new Vector3(_animationCurve.Evaluate(_currentTime / _timeShow), 0, 0);
        OnTaskInfoUpdated();
    }

    public void Show()
    {

        if (_coroutineShowDealy != null)
            StopCoroutine(_coroutineShowDealy);

        _coroutineShowDealy = StartCoroutine(ShowDelay());
    }

    public void Hide()
    {
        if (_coroutineHideDelay != null)
            StopCoroutine(_coroutineHideDelay);

        _coroutineHideDelay = StartCoroutine(HideDelay());
    }

    private void OnTaskInfoUpdated()
    {
        _description.text = _taskable.TaskInfo.ShowInfo();
    }

    private IEnumerator ShowDelay()
    {
        Vector3 localPosition = Vector3.zero;

        while (_currentTime < _timeShow)
        {
            _currentTime += Time.deltaTime;
            _panel.transform.localPosition = localPosition + new Vector3(_animationCurve.Evaluate(_currentTime / _timeShow), 0, 0);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _coroutineShowDealy = null;
        _isShow = true;
        Showed?.Invoke(this);
    }

    private IEnumerator HideDelay()
    {
        Vector3 localPosition = Vector3.zero;

        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _panel.transform.localPosition = localPosition + new Vector3(_animationCurve.Evaluate(_currentTime / _timeShow), 0, 0);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _isShow = false;
        _coroutineHideDelay = null;
        Hided?.Invoke(this);
    }
}
