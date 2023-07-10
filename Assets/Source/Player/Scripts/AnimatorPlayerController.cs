using UnityEngine;

public class AnimatorPlayerController : MonoBehaviour
{
    public static class Params
    {
        public const string IsWalking = nameof(IsWalking);
        public const string IsWalkBack = nameof(IsWalkBack);
        public const string IsStrafe = nameof(IsStrafe);
        public const string IsShooting = nameof(IsShooting);
        public const string IsRunning = nameof(IsRunning);
    }

    public static class States
    {
        public const string Idle = nameof(Idle);
        public const string WalkForward = nameof(WalkForward);
        public const string WalkBack = nameof(WalkBack);
        public const string Strafe = nameof(Strafe);
        public const string Fire = nameof(Fire);
        public const string Run = nameof(Run);
    }
}
