using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private OnTriggerHandler _triggerHandler;

    private List<ITargetable> _buildings = new List<ITargetable>();
    private Coroutine _checkBuildingInPath;
    private Coroutine _waitingTargetCoroutine;
    private StateMove _stateMove;
    private DayCycleManager _dayCycleManager;
    private ITargetable _player;
    private ITargetable _currentTarget;

    public ITargetable PlayerTarget => _player;
    public ITargetable CurrentTarget => _currentTarget;

    public event Action<ITargetable> Detected;

    private void OnEnable()
    {
        _enemy.Killed += OnEnemyKilled;
        _triggerHandler.TriggerEntered += TriggerEntered;
        _triggerHandler.TriggerExited += TriggerExited;
    }

    private void OnDisable()
    {
        if (_waitingTargetCoroutine != null)
            StopCoroutine(_waitingTargetCoroutine);

        _enemy.Killed -= OnEnemyKilled;
        _currentTarget = null;
        _triggerHandler.TriggerEntered -= TriggerEntered;
        _triggerHandler.TriggerExited += TriggerExited;
    }

    public void Init(ITargetable player, StateMove stateMove, DayCycleManager dayCycleManager)
    {
        _player = player;
        _stateMove = stateMove;
        _dayCycleManager = dayCycleManager;

        if (dayCycleManager.CurrentTimeDay is Night)
            _currentTarget = _player;
        else
            _waitingTargetCoroutine = StartCoroutine(WaitingTarget());
    }

    private void TriggerEntered(Collider other)
    {
        Building building = other.GetComponentInParent<Building>();

        if (building != null && building.Class != BuildingClass.Trap && building.Class != BuildingClass.Light)
        {
            _buildings.Add(building);

            if (_currentTarget == _player)
                SetTarget(building);

            if (_checkBuildingInPath == null)
                _checkBuildingInPath = StartCoroutine(CheckBuildingsInPath());
        }
    }

    private void TriggerExited(Collider other)
    {
        Building building = other.GetComponentInParent<Building>();

        if (building != null)
        {
            if (_buildings.Contains(building))
            {
                if (building is ITargetable targetable)
                    if (targetable == _currentTarget)
                        SetTarget(_player);

                _buildings.Remove(building);
            }
        }
    }

    private IEnumerator CheckBuildingsInPath()
    {
        _enemy.NavMeshAgent.speed = 0.5f;

        while (_buildings.Count > 0)
        {
            bool isFrontBuilding = false;

            foreach (var building in _buildings)
            {
                if (building != null && building.IsAlive)
                {
                    if (IsInFront(building, 0.1f) && IsInFront(_player, 0) && IsPriorityTarget(building))
                    {
                        SetTarget(building);
                        isFrontBuilding = true;
                        break;
                    }
                }
            }

            if (isFrontBuilding == false)
                SetTarget(_player);

            yield return new WaitForSeconds(0.2f);
        }

        _enemy.NavMeshAgent.speed = _stateMove.CurrentSpeed;
        SetTarget(_player);
        _checkBuildingInPath = null;
    }

    private IEnumerator WaitingTarget()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while (_currentTarget != _player)
        {
            if ((_player.Position - transform.position).SqrMagnitudeXZ() < 300)
                SetTarget(_player);

            yield return waitForSeconds;
        }
    }

    protected virtual bool IsPriorityTarget(ITargetable targetable)
    {
        return DistanceToTarget(_player) > DistanceToTarget(targetable);
    }

    protected float DistanceToTarget(ITargetable targetable) => (targetable.Position - transform.position).SqrMagnitudeXZ();

    private bool IsInFront(ITargetable target, float range)
    {
        float dot = Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(target.Position));
        return dot > range;
    }

    private void SetTarget(ITargetable targetable)
    {
        if (targetable != null)
            _currentTarget = targetable;
        else
            _currentTarget = _player;

        Detected?.Invoke(_currentTarget);
    }

    private void OnEnemyKilled(Enemy enemy, float _dyingDuration)
    {
        _buildings.Clear();
    }
}
