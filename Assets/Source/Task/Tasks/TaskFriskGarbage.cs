using UnityEngine;

public class TaskFriskGarbage : Task
{
    [SerializeField] private GarbagePointHandler _garbageHandler;

    protected override void Awake()
    {
        base.Awake();

        FriskCondition friskCondition = (FriskCondition)Condition;
        friskCondition.Init(_garbageHandler, _garbageHandler.NumbersOfAvailableFriskable);
    }

    protected override void Activate()
    {
        
    }
}
