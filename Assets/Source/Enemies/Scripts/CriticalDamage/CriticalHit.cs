using System;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHit : MonoBehaviour
{
    [SerializeField] private List<EnemyDestructiblePart> _enemyDistributionParts = new List<EnemyDestructiblePart>();

    [field: SerializeField] public float DamageMultiplier { get; private set; }

    public event Action<CriticalHit> Applied;

    private void OnDisable()
    {
        foreach (var part in _enemyDistributionParts)
            part.Activate();
    }

    private void Start()
    {
        if (_enemyDistributionParts.Count == 0)
        {
            Debug.LogError("Missing distribution parts!");
            enabled = false;
            return;
        }
    }

    public virtual bool TryTakeCriticalHit()
    {
        int randomPart = UnityEngine.Random.Range(0, _enemyDistributionParts.Count);

        if (_enemyDistributionParts[randomPart].CanDeactivate == false)
            return false;

        _enemyDistributionParts[randomPart].Deactivate();
        Applied?.Invoke(this);

        return true;
    }
}
