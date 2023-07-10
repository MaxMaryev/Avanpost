using System;
using System.Collections;
using UnityEngine;

public class FirstOpenCraftMenuStage : TutorialStage
{
    [SerializeField] private Workbench _workbench;
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private string _message;
    [SerializeField] private ArrowIndicates _template;
    [SerializeField] private Player _player;

    private Coroutine _coroutine;
    private ArrowIndicates _arrowIndicates;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _arrowIndicates = Instantiate(_template, _workbench.transform);
        _arrowIndicates.transform.localPosition = new Vector3(0, 6, 0);
        _arrowIndicates.Init();
        _workbench.OpenedCraftMenu += OnCraftMenuOpened;
    }

    protected override void OnExit()
    {
        if (_arrowIndicates != null)
            Destroy(_arrowIndicates.gameObject);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnCraftMenuOpened()
    {
        _workbench.OpenedCraftMenu -= OnCraftMenuOpened;
        _hintHandler.SkipAllHitns();
        Completed?.Invoke(this);
    }
}
