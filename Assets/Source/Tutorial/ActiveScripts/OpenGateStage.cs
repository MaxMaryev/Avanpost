using System;
using System.Collections;
using UnityEngine;

public class OpenGateStage : TutorialStage
{
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private string _hintMessage;
    [SerializeField] private ContainersHandler _containersHandler;
    [SerializeField] private ArrowIndicates _templateArrow;
    [SerializeField] private Task _task;
    [SerializeField] private BuildingsPanel _buildingsPanel;
    [SerializeField] private GameObject _rootCraftMenu;

    private ITaskActivable _taskActivable => _task;
    private Gate _gate;
    private ArrowIndicates _arrowIndicates;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        StartCoroutine(WaitEndOfFrame());
    }

    private IEnumerator WaitEndOfFrame()
    {
        yield return new WaitUntil(() => _buildingsPanel.gameObject.activeSelf == false);

        _rootCraftMenu.gameObject.SetActive(false);
        _gate = _containersHandler.GetComponentInChildren<Gate>();
        _arrowIndicates = Instantiate(_templateArrow, _gate.transform);
        _arrowIndicates.transform.localPosition = new Vector3(0, 4, 0);
        _arrowIndicates.Init();
        _hintHandler.Add(_hintMessage);
        _taskActivable.TryActivate();
        _taskBarHandler.AddTask(_taskActivable);
        _taskActivable.Finished += OnTaskFinished;
    }

    private void OnTaskFinished(ITaskable taskable)
    {
        taskable.Finished -= OnTaskFinished;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        Destroy(_arrowIndicates.gameObject);
    }
}
