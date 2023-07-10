using System.Collections;
using UnityEngine;

public class StateAttack : BaseState
{
    private int _attackAnimationIndex;
    private Coroutine _attackCoroutine;
    private EnemyHitHandler _enemyHitHandler;
    private EnemyAnimationParameters _enemyAnimationParameters;
    private Coroutine _rotateCoroutine;
    private AttackParameters _attackParameters;
    private IAttacker _attacker;

    public StateAttack(Enemy enemy, EnemyAnimationsController animationsController, EnemyHitHandler enemyHitHandler, EnemyAnimationParameters enemyAnimationParameters, AttackParameters attackParameters, IAttacker attacker) : base(enemy, animationsController)
    {
        _enemyHitHandler = enemyHitHandler;
        _enemyAnimationParameters = enemyAnimationParameters;
        _attackParameters = attackParameters;
        _attacker = attacker;
    }

    public override void Enter()
    {
        base.Enter();
        AnimationsController.SetStateAttack(true);
        _attackCoroutine = Enemy.StartCoroutine(TryAttackAnimation());
        _rotateCoroutine = Enemy.StartCoroutine(RotateToTargetDelay());
        _enemyHitHandler.Hit += OnEnemyHit;
    }

    public override void Exit()
    {
        base.Exit();

        if (_attackCoroutine != null)
            Enemy.StopCoroutine(_attackCoroutine);

        if (_rotateCoroutine != null)
            Enemy.StopCoroutine(_rotateCoroutine);

        AnimationsController.SetStateAttack(false);
        _enemyHitHandler.Hit -= OnEnemyHit;
    }

    private IEnumerator RotateToTargetDelay()
    {
        while (true)
        {
            Vector3 rotation = (Enemy.Target.Position - Enemy.transform.position).normalized;
            Enemy.transform.rotation = Quaternion.RotateTowards(Enemy.transform.rotation, Quaternion.LookRotation(rotation.With(y: 0)), Time.deltaTime * _attackParameters.SpeedRotate);
            yield return null;
        }
    }

    private void HitMoment()
    {
        if (Enemy.Target != null)
        {
            if (Enemy.Target is IDamageable damageable)
            {
                if ((Enemy.Target.Position - Enemy.transform.position).SqrMagnitudeXZ() <= _attacker.Range)
                    _attacker.Attack(damageable);
            }
        }
    }

    private IEnumerator TryAttackAnimation()
    {
        float cycleUpdateDelay = 0.1f;

        WaitUntil waitHitMoment = new WaitUntil(() => AnimationsController.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _attackParameters.HitAnimationMoment);
        WaitUntil waitEndAnimation = new WaitUntil(() => AnimationsController.Animator.GetAnimatorTransitionInfo(0).normalizedTime >= _attackParameters.EndAnimation);
        WaitUntil waitBeginAttackAnimation = new WaitUntil(() => AnimationsController.Animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.AnimatorStates.AttackState));
        WaitForSeconds waitCycleUpdate = new WaitForSeconds(cycleUpdateDelay);
        yield return new WaitUntil(() => Enemy.Target != null);

        while (Enemy.Target.IsAlive)
        {
            _attackAnimationIndex = _enemyAnimationParameters.RandomValueAttack;
            AnimationsController.PlayAnimationAttack(_attackAnimationIndex);
            yield return waitBeginAttackAnimation;
            yield return waitHitMoment;

            if (AnimationsController.Animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.AnimatorStates.AttackState))
                HitMoment();

            yield return waitEndAnimation;
            yield return waitCycleUpdate;
        }
    }

    private void OnEnemyHit(WeaponType weaponType)
    {
        if (_enemyAnimationParameters.TryGet(weaponType, out HitAnimationParametes animationParametes))
        {
            AnimationsController.PlayAnimationHit(animationParametes.RandomAction, (int)animationParametes.WeaponType);
        }
    }
}
