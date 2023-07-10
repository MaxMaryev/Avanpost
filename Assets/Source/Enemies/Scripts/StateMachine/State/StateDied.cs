using System.Collections;
using UnityEngine;

public class StateDied : BaseState
{
    private Vector3 _lastTargetPosition;
    private EnemyAnimationParameters _animationParameters;

    public StateDied(Enemy enemy, EnemyAnimationsController animationsController, EnemyAnimationParameters animationParameters) : base(enemy, animationsController)
    {
        _animationParameters = animationParameters;
    }

    
    public override void Tick()
    {
        Vector3 rotation = (_lastTargetPosition - Enemy.transform.position).normalized;
        Enemy.transform.rotation = Quaternion.LookRotation(rotation.With(y: 0));
    }

    public override void Enter()
    {
        AnimationsController.PlayAnimationDeath(_animationParameters.RandomValueDeath);
        Enemy.NavMeshAgent.enabled = false;
        _lastTargetPosition = Enemy.Target == null ? Enemy.transform.position : Enemy.Target.Position;
    }

    public override void Exit() { }
}
