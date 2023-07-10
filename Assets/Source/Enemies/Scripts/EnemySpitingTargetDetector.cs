using UnityEngine;

[RequireComponent(typeof(IAttacker))]
public class EnemySpitingTargetDetector : EnemyTargetDetector
{
    private IAttacker _attacker;

    private void Start()
    {
        _attacker = GetComponent<IAttacker>();
    }

    protected override bool IsPriorityTarget(ITargetable targetable)
    {
        return (PlayerTarget.Position - transform.position).SqrMagnitudeXZ() > _attacker.Range / 4f;
    }
}
