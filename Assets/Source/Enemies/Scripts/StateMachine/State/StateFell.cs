using System.Collections;
using UnityEngine;

public class StateFell : BaseState
{
    private EnemyAnimationParameters _animationParameters;
    private EnemyHitHandler _enemyHitHandler;

    public StateFell(Enemy enemy, EnemyAnimationsController animationsController, EnemyHitHandler enemyHitHandler, EnemyAnimationParameters enemyAnimationParameters) : base(enemy, animationsController)
    {
        _animationParameters = enemyAnimationParameters;
        _enemyHitHandler = enemyHitHandler;
    }

    public override void Enter()
    {
        base.Enter();
        AnimationsController.PlayAnimationFell(_animationParameters.RandomValueFell, 0);
        _enemyHitHandler.Hit += OnEnemyHit;
    }

    public override void Exit()
    {
        base.Exit();
        _enemyHitHandler.Hit -= OnEnemyHit;
    }

    public override void Tick()
    {
        AnimatorStateInfo animatorStateInfo = AnimationsController.Animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName(Constants.AnimatorStates.Up) && animatorStateInfo.normalizedTime > 0.9f)
            AnimationsController.Animator.SetBool(Constants.AnimatorParameters.IsFell, false);
    }

    private void OnEnemyHit(WeaponType weaponType)
    {
        if (_animationParameters.TryGet(weaponType, out HitAnimationParametes animationParametes))
            AnimationsController.PlayAnimationHit(animationParametes.RandomAction, (int)animationParametes.WeaponType);
    }
}
