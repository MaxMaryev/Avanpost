using System;
using System.Collections;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private ResourceSpawnerDeterminant _resourceSpawnerDeterminant;

    public event Action<ResourceBox> ReceivedRecource;

    private void OnEnable()
    {
        _enemiesSpawner.Spawned += OnEnemySpawned;
    }

    private void OnDisable()
    {
        _enemiesSpawner.Spawned -= OnEnemySpawned;
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        enemy.Killed += OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy, float dyingDelay)
    {
        if (enemy.Pocket.TryGetResource(out ResourceBox resourceBox))
        {
            ResourceSpawner resourceSpawner = _resourceSpawnerDeterminant.GetSpawner(resourceBox.Resource);

            if (resourceSpawner != null)
                resourceSpawner.SpawnResource(enemy.transform.position, enemy.transform.position, resourceBox.Amount);
        }
    }
}
