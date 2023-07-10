using System.Collections.Generic;
using UnityEngine;

public class EnemyOptimizer : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _blood;
    [SerializeField] private AnimationCurve _bloodLifeTime;
    [SerializeField] private Enemy _enemy;

    public void Optimize(int countOfEnemiesAroundPlayer)
    {
        if (_enemy.IsAlive == false)
            return;

        foreach (var blood in _blood)
        {
            ParticleSystem.MainModule main = blood.main;
            main.startLifetime = _bloodLifeTime.Evaluate(countOfEnemiesAroundPlayer * 0.6f);
        }

        _enemy.SetDyingDuration(_bloodLifeTime.Evaluate(countOfEnemiesAroundPlayer));
    }
}
