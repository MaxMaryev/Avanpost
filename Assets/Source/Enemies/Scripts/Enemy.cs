using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAnimationsController), typeof(EnemyHitHandler), typeof(Collider))]
[RequireComponent(typeof(IAttacker))]
public abstract class Enemy : MonoBehaviour, IRewardGiver, IVisible, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private EnemyAnimationParameters _enemyHitAnimationParameters;
    [SerializeField] private AttackParameters _attackParameters;
    [SerializeField] private EnemyTargetDetector _targetSetter;

    private float _dyingDuration;
    private EnemyAnimationsController _animationsController;
    private EnemyHitHandler _enemyHitHandler;
    private StateMachine _stateMachine;
    private BaseState _defaultState;
    private Collider _collider;
    private Coroutine _updatingCoroutine;
    private StateMove _stateMove;
    private StateDied _stateDied;
    private IAttacker _attacker;

    public event Action<Enemy, float> Killed;
    public event Action<float> HealthChanged;
    public event Action Initialized;

    [field: SerializeField] public Pocket Pocket { get; private set; }
    [field: SerializeField] public float Reward { get; private set; }
    [field: SerializeField] public EnemyLightInfluence EnemyLightInfluence { get; private set; }
    [field: SerializeField] public Transform SpineRigTransform { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

    public bool IsAlive { get; private set; }
    public float CurrentHealth { get; private set; }
    public ITargetable Target => _targetSetter.CurrentTarget;
    public Vector3 Position => transform.position;
    public bool IsVisible => EnemyLightInfluence.IsHide == false;

    private void Awake()
    {
        _animationsController = GetComponent<EnemyAnimationsController>();
        _enemyHitHandler = GetComponent<EnemyHitHandler>();
        _collider = GetComponent<Collider>();
        _attacker = GetComponent<IAttacker>();
        CurrentHealth = _maxHealth;

        _stateMachine = new StateMachine();

        StateIdle stateIdle = new StateIdle(this, _animationsController, _enemyHitHandler, _enemyHitAnimationParameters);
        StateAttack stateAttack = new StateAttack(this, _animationsController, _enemyHitHandler, _enemyHitAnimationParameters, _attackParameters, _attacker);
        _stateDied = new StateDied(this, _animationsController, _enemyHitAnimationParameters);
        StateBiting stateBiting = new StateBiting(this, _animationsController);
        _stateMove = new StateMove(this, _animationsController, _enemyHitHandler, _enemyHitAnimationParameters);

        _stateMachine.AddTransition(stateIdle, _stateMove, () => _targetSetter.CurrentTarget != null && IsNearTarget() == false);
        _stateMachine.AddTransition(stateIdle, stateAttack, () => _targetSetter.CurrentTarget.IsAlive && IsNearTarget());
        _stateMachine.AddTransition(stateIdle, stateBiting, () => _targetSetter.CurrentTarget is Player && _targetSetter.CurrentTarget.IsAlive == false && IsNearTarget());
        _stateMachine.AddTransition(stateIdle, _stateDied, () => IsAlive == false);
        _stateMachine.AddTransition(_stateMove, stateIdle, () => _targetSetter.CurrentTarget == null || IsNearTarget());
        _stateMachine.AddTransition(_stateMove, _stateDied, () => IsAlive == false);
        _stateMachine.AddTransition(stateAttack, stateIdle, () => _targetSetter.CurrentTarget == null || (_targetSetter.CurrentTarget.IsAlive == false || IsNearTarget() == false));
        _stateMachine.AddTransition(stateAttack, _stateDied, () => IsAlive == false);

        _defaultState = stateIdle;
    }

    private void OnDisable()
    {
        if (_updatingCoroutine != null)
            StopCoroutine(_updatingCoroutine);
    }

    public void Init(ITargetable targetable, DayCycleManager dayCycleManager)
    {
        _targetSetter.Init(targetable, _stateMove, dayCycleManager);
        IsAlive = true;
        NavMeshAgent.enabled = true;
        _collider.enabled = true;
        CurrentHealth = _maxHealth;
        _stateMachine.ChangeState(_defaultState);
        EnemyLightInfluence.Init(dayCycleManager);
        _updatingCoroutine = StartCoroutine(Updating());

        Initialized?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        if (IsAlive == false)
            return;

        CurrentHealth -= damage;
        HealthChanged?.Invoke(CurrentHealth / _maxHealth);

        if (CurrentHealth <= 0)
            Dying();
    }

    public void SetDyingDuration(float dyingDuration) => _dyingDuration = dyingDuration;

    private IEnumerator Updating()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

        while (true)
        {
            if (_targetSetter.CurrentTarget != null)
                _stateMachine?.Tick();

            yield return waitForSeconds;
        }
    }

    private void Dying()
    {
        IsAlive = false;
        _collider.enabled = false;
        _stateMachine.ChangeState(_stateDied);

        if (_dyingDuration == 0)
            Killed?.Invoke(this, 15);
        else
            Killed?.Invoke(this, _dyingDuration);
    }

    private bool IsNearTarget()
    {
        return (_targetSetter.CurrentTarget.Position - transform.position).SqrMagnitudeXZ() <= _attacker.Range;
    }
}
