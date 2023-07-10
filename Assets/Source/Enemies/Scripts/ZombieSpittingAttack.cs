using System;
using UnityEngine;

public class ZombieSpittingAttack : MonoBehaviour, IAttacker
{
    [SerializeField] private int _damage;
    [SerializeField] private float _range;
    [SerializeField] private Spit _spit;
    [SerializeField] private float _speedSpit;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private SphereCollider _sphereCollider;

    private float _offset = 1f;

    public event Action Attacked;

    public float Range => _range;
    public int Damage => _damage;

    private void Start()
    {
        _sphereCollider.radius = _range / 10;
    }

    public void Attack(IDamageable damageable)
    {
        ITargetable targetable = (ITargetable)damageable;

        Vector3 direction = (targetable.Position + new Vector3(0, _offset, 0) - transform.position).normalized;
        _spit.transform.rotation = Quaternion.LookRotation(direction.With(y: 0));
        _spit.Init(targetable.Position + new Vector3(0, _offset, 0), Damage, _speedSpit);
    }
}
