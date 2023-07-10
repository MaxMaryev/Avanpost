using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField] private GameModeHandler _gameModeHandler;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private List<ObjectsPool> _pools;
    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private Vector3 _checkBox;
    [SerializeField] private PlayerBleeding _playerBleeding;

    private TargetFinder _targetFinder;
    private bool _isTargetOnFireLine;

    public ITargetable Target { get; private set; }
    public int CountOfEnemiesAround { get; private set; }
    public bool CanShoot { get; private set; }
    public bool IsOnShotDistance { get; private set; }

    public event Action Initialized;

    private void OnDrawGizmos()
    {
        Vector3 offset = new Vector3(0, 1, 5);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, _checkBox);
    }

    private void Awake() => _targetFinder = new TargetFinder(_pools.ToArray());

    private void Start()
    {
        _playerBleeding.Init(_player.Health);
        _playerDeath.Init(_player.Health);
        Initialized?.Invoke();
        StartCoroutine(CheckingEnemiesAround());
    }

    private void OnEnable() => _player.TriedShoot += OnTriedShoot;

    private void OnDisable() => _player.TriedShoot -= OnTriedShoot;

    private void FixedUpdate()
    {
        if (_player.IsAlive == false)
            return;

        if (_gameModeHandler.CurrentGameMode == GameModes.CombatMode && Target != null && Target.IsAlive && IsOnShotDistance)
        {
            _playerMover.LookAtEnemy();
        }
        else
        {
            CanShoot = false;
            IsOnShotDistance = false;
            _playerMover.LookForward();
        }
    }

    public ITargetable GetNearestEnemy() => _targetFinder.TryFindNearestTo(transform);

    private void OnTriedShoot()
    {
        if (_gameModeHandler.CurrentGameMode != GameModes.CombatMode)
            return;

        Target = GetNearestEnemy();
        _isTargetOnFireLine = IsTargetOnFireLine();

        if (Target == null || Target.IsAlive == false)
            return;

        float distanceToTarget = (transform.position - Target.Position).SqrMagnitudeXZ();
        IsOnShotDistance = distanceToTarget <= _weaponHandler.CurrentWeapon.ShotDistance && (Target as IVisible).IsVisible;
        CanShoot = IsOnShotDistance && _playerMover.IsLookingAtEnemy && _isTargetOnFireLine;

        if (CanShoot)
            _weaponHandler.CurrentWeapon.Fire();
    }

    private bool IsTargetOnFireLine()
    {
        if (Target == null)
            return false;

        Ray ray = new Ray(_weaponHandler.CurrentWeapon.Aim.position, _weaponHandler.CurrentWeapon.Aim.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, Layers.Enemy))
            if (hit.collider != null)
                return true;

        return false;
    }

    private IEnumerator CheckingEnemiesAround()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(2);
        Vector3 offset = new Vector3(0, 1, 5);

        while (_player.IsAlive)
        {
            Collider[] _enemiesColliders = Physics.OverlapBox(transform.position + offset, _checkBox / 2, Quaternion.identity, Layers.Enemy);

            for (int i = 0; i < _enemiesColliders.Length; i++)
            {
                if (_enemiesColliders[i].TryGetComponent(out EnemyOptimizer enemyOptimizer))
                    enemyOptimizer.Optimize(_enemiesColliders.Length);

                yield return null;
            }

            yield return waitForSeconds;
        }
    }
}