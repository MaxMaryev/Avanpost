using System;
using System.Collections;
using UnityEngine;

public class FinishZombiesStage : TutorialStage
{
    [SerializeField] private string _message;
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private ZombiesPool _zombiesPool;

    private Coroutine _coroutine;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _coroutine = StartCoroutine(CompleteDelay());
    }

    private IEnumerator CompleteDelay()
    {
        yield return new WaitUntil(() => _zombiesPool.Enabled.Count == 0);

        _hintHandler.Add(_message);
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        
    }
}
