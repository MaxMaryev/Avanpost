using UnityEngine;

public class CriticalHitLeg : CriticalHit
{
    [SerializeField] private EnemyAnimationsController _animationsController;

    public override bool TryTakeCriticalHit()
    {
        _animationsController.SetMoveType();
        return base.TryTakeCriticalHit();
    }
}
