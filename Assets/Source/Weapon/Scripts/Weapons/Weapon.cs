using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField] public WeaponType WeaponType { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; protected set; }
    [field: SerializeField] public float MaxDamage { get; private set; }
    [field: SerializeField] public float ShotDistance { get; private set; }
    [field: SerializeField] public float BonusToCritChance { get; private set; }
    [field: SerializeField] protected float AttackCooldown { get; private set; }
    [field: SerializeField] protected PlayerAnimationsController PlayerAnimationsController { get; private set; }

    public abstract void Fire();

    public virtual void Accept(IWeaponVisitor weaponVisitor, float damage) { }
}
