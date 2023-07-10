using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "SO/EnemyParameters/Attack", order = 51)]
public class AttackParameters : ScriptableObject
{
    [field: SerializeField] public float HitAnimationMoment { get; private set; } = 0.2f;
    [field: SerializeField] public float EndAnimation { get; private set; } = 0.8f;

    [field: SerializeField] public float SpeedRotate { get; private set; } = 500f;
}
