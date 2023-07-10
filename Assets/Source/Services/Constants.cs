using UnityEngine;

public class Constants : MonoBehaviour
{
    public const float Epsilon = 0.001f;
    public const float MaxBlendShapeValue = 100;
    public const float Sqrt2 = 0.7071068f;

    public static class AnimatorParameters
    {
        public const string IsDying = nameof(IsDying);
        public const string IsWalking = nameof(IsWalking);
        public const string IsWalkBack = nameof(IsWalkBack);
        public const string IsStrafe = nameof(IsStrafe);
        public const string IsShooting = nameof(IsShooting);
        public const string IsRunning = nameof(IsRunning);
        public const string IsMove = nameof(IsMove);
        public const string WeaponIndex = nameof(WeaponIndex);
        public const string Action = nameof(Action);
        public const string Trigger = nameof(Trigger);
        public const string TriggerNumber = nameof(TriggerNumber);
        public const string IsFell = nameof(IsFell);
        public const string Vertical = nameof(Vertical);
        public const string Horizontal = nameof(Horizontal);
        public const string Speed = nameof(Speed);
        public const string CanShoot = nameof(CanShoot);
        public const string IsReload = nameof(IsReload);
        public const string Health = nameof(Health);
        public const string AimToTarget = nameof(AimToTarget);
        public const string IsRepairing = nameof(IsRepairing);
        public const string ActionMove = nameof(ActionMove);
        public const string ActionDeath = nameof(ActionDeath);
        public const string ActionAttack = nameof(ActionAttack);
        public const string ActionHit = nameof(ActionHit);
        public const string ActionFell = nameof(ActionFell);
        public const string TypeMove = nameof(TypeMove);
        public const string IsWithoutLegs = nameof(IsWithoutLegs);
        public const string IsFrisk = nameof(IsFrisk);
        public const string IsAttack = nameof(IsAttack);
    }

    public static class AnimatorStates
    {
        public const string Up = nameof(Up);
        public const string Dying = nameof(Dying);
        public const string Hit = nameof(Hit);
        public const string Idle = nameof(Idle);
        public const string WalkForward = nameof(WalkForward);
        public const string WalkBack = nameof(WalkBack);
        public const string Strafe = nameof(Strafe);
        public const string Fire = nameof(Fire);
        public const string Run = nameof(Run);
        public const string AttackState = nameof(AttackState);
        public const string EnemyMove = nameof(EnemyMove);
        public const string Fell = nameof(Fell);
        public const string Reload = nameof(Reload);
    }
}
