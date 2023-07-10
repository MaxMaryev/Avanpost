using UnityEngine;

public class StateIdle : BaseState
{
    private EnemyAnimationParameters _enemyHitAnimationParameters;
    private EnemyHitHandler _enemyHitHandler;

    public StateIdle(Enemy enemy, EnemyAnimationsController animationsController, EnemyHitHandler enemyHitHandler,EnemyAnimationParameters enemyHitAnimationParameters) : base(enemy, animationsController) 
    {
        _enemyHitAnimationParameters = enemyHitAnimationParameters;
        _enemyHitHandler = enemyHitHandler;
    }

    public override void Enter()
    {
        base.Enter();
        _enemyHitHandler.Hit += OnEnemyHit;
    }

    public override void Exit()
    {
        base.Exit();
        _enemyHitHandler.Hit -= OnEnemyHit;
    }

    private void OnEnemyHit(WeaponType weaponType)
    {
        if (_enemyHitAnimationParameters.TryGet(weaponType, out HitAnimationParametes animationParametes))
        {
            AnimationsController.PlayAnimationHit(animationParametes.RandomAction, (int)animationParametes.WeaponType);
        }
    }
}
