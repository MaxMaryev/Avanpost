using System;
using System.Collections;
using UnityEngine;

public class FirstDawnStage : TutorialStage
{
    [SerializeField] private GarbagePointHandler _garbageHandler;
    [SerializeField] private GarbagePoint _garbagePoint;

    private Coroutine _coroutine;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _garbageHandler.gameObject.SetActive(true);

        _coroutine = StartCoroutine(GarbagePointActivateDelay());
    }

    protected override void OnExit()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator GarbagePointActivateDelay()
    {
        yield return new WaitForEndOfFrame();

        if (_garbagePoint.gameObject.activeSelf == false)
            _garbageHandler.ActivateGarbagePoint(_garbagePoint);

        Completed?.Invoke(this);
    }
}
