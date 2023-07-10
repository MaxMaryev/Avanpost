using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBreakBuildingStage : TutorialStage
{
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private string _message;
    [SerializeField] private BlockDestructionBuilding _blockDestructionBuilding;

    private Coroutine _coroutine;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _coroutine = StartCoroutine(ShowDelay());
        _hintHandler.Add(_message);
    }

    protected override void OnExit()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator ShowDelay()
    {
        yield return new WaitForEndOfFrame();

        foreach (var building in _blockDestructionBuilding.Buildings)
        {
            if (building != null)
            {
                //building.IsBlockDestruction = false;
                //building.ShowBreakButton(true);
            }
        }

        yield return new WaitUntil(() => _hintHandler.IsAvailable == false);

        Completed?.Invoke(this);
    }
}
