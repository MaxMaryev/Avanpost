using System;
using UnityEngine;

public class FriskGarbageStage : TutorialStage
{
    [SerializeField] private GarbagePoint _garbagePoint;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _garbagePoint.Frisked += OnFrisked;
    }

    protected override void OnExit()
    {
        
    }

    private void OnFrisked(IFriskable garbagePoint)
    {
        _garbagePoint.Frisked -= OnFrisked;
        Completed?.Invoke(this);
    }
}
