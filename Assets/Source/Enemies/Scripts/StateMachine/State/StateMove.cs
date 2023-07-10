using System.Collections;
using UnityEngine;

public class StateMove : BaseState
{
    private EnemyHitHandler _hitHandler;
    private EnemyAnimationParameters _enemyAnimationParameters;
    private Coroutine _coroutine;
    
    public float CurrentSpeed { get; private set; }

    public StateMove(Enemy enemy, EnemyAnimationsController animationsController, EnemyHitHandler enemyHitHandler, EnemyAnimationParameters enemyAnimationParameters) : base(enemy, animationsController)
    {
        _hitHandler = enemyHitHandler;
        _enemyAnimationParameters = enemyAnimationParameters;
        BaseSpeed = enemy.NavMeshAgent.speed;
    }

    public float BaseSpeed { get; private set; }
    public float CollisionSpeed { get; private set; } = 1f;

    public override void Enter()
    {
        base.Enter();
        CurrentSpeed = BaseSpeed;
        Enemy.NavMeshAgent.speed = CurrentSpeed;
        _hitHandler.Hit += OnEnemyHit;
        Enemy.NavMeshAgent.isStopped = false;
        AnimationsController.PlayAnimationMove(_enemyAnimationParameters.RandomValueMove);
        _coroutine = Enemy.StartCoroutine(CheckTypeMove());
    }

    public override void Exit()
    {
        base.Exit();
        _hitHandler.Hit -= OnEnemyHit;
        Enemy.NavMeshAgent.isStopped = true;
        AnimationsController.StopAnimationMove();

        if (_coroutine != null)
            Enemy.StopCoroutine(_coroutine);
    }

    public override void Tick()
    {
        if (Enemy.NavMeshAgent.isStopped == false)
            Enemy.NavMeshAgent.SetDestination(Enemy.Target.Position);
    }

    private void OnEnemyHit(WeaponType weaponType)
    {
        if (_enemyAnimationParameters.TryGet(weaponType, out HitAnimationParametes animationParametes))
            AnimationsController.PlayAnimationHit(animationParametes.RandomAction, (int)animationParametes.WeaponType);
    }

    private IEnumerator CheckTypeMove()
    {
        yield return new WaitUntil(() => AnimationsController.Animator.GetFloat(Constants.AnimatorParameters.TypeMove) == 1);

        CurrentSpeed = BaseSpeed / 2;
        Enemy.NavMeshAgent.speed = CurrentSpeed;
        Enemy.NavMeshAgent.isStopped = true;

        //yield return new WaitUntil(() => AnimationsController.Animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.AnimatorStates.EnemyMove) == false);
        //yield return new WaitUntil(() => AnimationsController.Animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.AnimatorStates.EnemyMove));

        yield return new WaitForSeconds(AnimationsController.Animator.GetCurrentAnimatorStateInfo(0).length);

        Enemy.NavMeshAgent.isStopped = false;
    }
}
