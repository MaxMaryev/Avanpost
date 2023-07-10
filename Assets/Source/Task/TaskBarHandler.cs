using Lean.Localization;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskBarHandler : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private TaskView _template;
    [SerializeField] private TaskBar _taskBar;
    [SerializeField] private TMP_Text _taskLabel;
    [SerializeField] private ResourceRewardViewUI _templateRewardUI;
    [SerializeField] private float _offsetXForRewardUI;

    private Pool<ResourceRewardViewUI> _pools;
    private Dictionary<ITaskable, TaskView> _taskables = new Dictionary<ITaskable, TaskView>();
    private void Awake() => _taskBar.gameObject.SetActive(false);

    public int CountActiveTasks => _taskables.Count;

    public event Action<TaskView> Added;
    public event Action<TaskView> Finished;
    public event Action<TaskView> Removing;
    public event Action Removed;

    private void Start()
    {
        _pools = new Pool<ResourceRewardViewUI>(_templateRewardUI, transform, 2);
        _taskLabel.text = LeanLocalization.CurrentTranslations["TASKS"].Data.ToString();
    }

    public void AddTask(ITaskable taskable)
    {
        TaskView taskView = Instantiate(_template, _container);
        taskView.Init(taskable);
        _taskables.Add(taskable, taskView);
        taskable.Finished += OnTaskableFinished;

        if (_taskBar.gameObject.activeSelf == false)
            _taskBar.gameObject.SetActive(true);

        Added?.Invoke(taskView);
    }

    private void OnTaskableFinished(ITaskable taskable)
    {
        taskable.Finished -= OnTaskableFinished;
        _taskables[taskable].Showed += OnShowed;
        //_taskables[taskable].transform.SetAsFirstSibling();
        Finished?.Invoke(_taskables[taskable]);
    }

    private void OnShowed(IShowable showable)
    {
        showable.Showed -= OnShowed;

        ITaskable taskable = ((TaskView)showable).Taskable;

        Remove(taskable);
    }

    private void Remove(ITaskable taskable)
    {
        if (taskable.TaskReward.GetAmountReward() > 0)
        {
            if (taskable.TaskStatus.Type == TaskStatusType.Completed)
            {
                ResourceRewardViewUI resourceRewardViewUI = _pools.GetFreeElement();
                Vector3 position = _taskables[taskable].transform.position;
                resourceRewardViewUI.transform.position = position;
                resourceRewardViewUI.GetComponent<RectTransform>().anchoredPosition += new Vector2(_offsetXForRewardUI, 0);
                resourceRewardViewUI.Init(taskable.TaskReward.GetAmountReward());
                resourceRewardViewUI.gameObject.SetActive(true);
            }
        }

        Removing?.Invoke(_taskables[taskable]);
        Destroy(_taskables[taskable].gameObject);
        _taskables.Remove(taskable);

        Removed?.Invoke();

        if (_taskables.Count == 0)
            _taskBar.gameObject.SetActive(false);
    }
}
