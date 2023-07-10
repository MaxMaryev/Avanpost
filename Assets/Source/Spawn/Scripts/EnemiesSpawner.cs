using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [Header("WaveConfigs")]
    [SerializeField] private SpawnDayWaveConfig _dayWaveConfig;
    [SerializeField] private SpawnNightWaveConfig _nightWaveConfig;
    [Header("Pools")]
    [SerializeField] private ZombiesPool _zombiesPool;
    [SerializeField] private HulkPool _dogsPool;
    [SerializeField] private MonoBehaviour _target;
    [SerializeField] private List<Transform> _spawnPointsDay;
    [SerializeField] private List<Transform> _spawnPointsNight;

    private ITargetable _targetable => (ITargetable)_target;

    private float _spawnDistance = 1;
    private Coroutine _spawnCoroutine;

    public event Action<Enemy> Spawned;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var point in _spawnPointsDay)
        {
            Gizmos.DrawSphere(point.position, 1);
        }

        Gizmos.color = Color.black;
        foreach (var point in _spawnPointsNight)
        {
            Gizmos.DrawSphere(point.position, 1);
        }
    }

    private void Awake()
    {
        WaveConfig wave = _dayWaveConfig.GetWaveBy(dayNumber: _dayCycleManager.CurrentDay);

        for (int i = 0; i < 50; i++)
        {
            Instantiate(wave.ZombiesTemplates[UnityEngine.Random.Range(0, wave.ZombiesTemplates.Count)], _zombiesPool.transform);
        }

        _zombiesPool.OnStartPoolInstantiated();
    }

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnDayTimeChanged;
        BeginSpawn();
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnDayTimeChanged;

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private void OnValidate()
    {
        if (_target && !(_target is ITargetable))
        {
            Debug.LogError(nameof(_target) + " needs to implement " + nameof(ITargetable));
            _target = null;
        }
    }

    private IEnumerator Spawning(SpawnWavesConfig wavesConfig)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2);

        WaveConfig wave = wavesConfig.GetWaveBy(dayNumber: _dayCycleManager.CurrentDay);

        WaitForSeconds wait = new(wave.SpawnDelay);
        var random = (min: 0, max: 100);
        int roll;
        bool isMaxEnemyCountReached;

        while (_dayCycleManager.CurrentNormalizedTime < 1)
        {
            isMaxEnemyCountReached = _zombiesPool.Enabled.Count + _dogsPool.Enabled.Count >= wave.MaxCount;

            if (isMaxEnemyCountReached)
            {
                if (wave.IsStopWhenMaxCountReached)
                    break;

                yield return wait;
                continue;
            }

            roll = UnityEngine.Random.Range(random.min, random.max);

            //if (roll < wave.HulkSpawnChanceInPercent)
            //    Spawn(wave.HulkTemplates[UnityEngine.Random.Range(0, wave.HulkTemplates.Count)]);
            //else
                Spawn(wave.ZombiesTemplates[UnityEngine.Random.Range(0, wave.ZombiesTemplates.Count)]);

            yield return wait;
        }
    }

    private void Spawn(Enemy enemy)
    {
        Enemy newEnemy;

        if (enemy is Zombie)
            SetEnemy(_zombiesPool);
        else
            SetEnemy(_dogsPool);

        newEnemy.transform.position = GetRandomPosition();
        newEnemy.gameObject.SetActive(true);
        newEnemy.Init(_targetable, _dayCycleManager);

        void SetEnemy(ObjectsPool pool)
        {
            newEnemy = pool.GetAvailable();

            if (newEnemy == null)
            {
                newEnemy = Instantiate(enemy, pool.transform);
                pool.AddNew(newEnemy);
            }
        }

        Spawned?.Invoke(newEnemy);
    }

    private void BeginSpawn()
    {
        if (_dayCycleManager.CurrentTimeDay is Dawn || _dayCycleManager.CurrentTimeDay is Day)
            _spawnCoroutine = StartCoroutine(Spawning(_dayWaveConfig));
        else if (_dayCycleManager.CurrentTimeDay is Night)
            _spawnCoroutine = StartCoroutine(Spawning(_nightWaveConfig));
    }

    private void OnDayTimeChanged(TimeDay day)
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        BeginSpawn();
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 spawnPoint;

        if (_dayCycleManager.CurrentTimeDay is Night)
            spawnPoint = _spawnPointsNight[UnityEngine.Random.Range(0, _spawnPointsNight.Count)].position;
        else
            spawnPoint = _spawnPointsDay[UnityEngine.Random.Range(0, _spawnPointsDay.Count)].position;

        var random = (min: 1, max: 360);
        int randomAngle = UnityEngine.Random.Range(random.min, random.max);
        Vector3 spread = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)) * _spawnDistance;
        return spawnPoint + spread;
    }
}
