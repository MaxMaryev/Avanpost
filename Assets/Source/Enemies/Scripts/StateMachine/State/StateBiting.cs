using UnityEngine;

public class StateBiting : BaseState
{
    public StateBiting(Enemy enemy, EnemyAnimationsController animationsController) : base(enemy, animationsController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AnimationsController.PlayAnimationBiting();
    }
}
