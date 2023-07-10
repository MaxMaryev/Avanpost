using System;
using UnityEngine;

public class ZombieHandAttack : MonoBehaviour, IAttacker
{
    [SerializeField] private int _minDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _range;

    public event Action Attacked;

    public float Range => _range;
    public int Damage => UnityEngine.Random.Range(_minDamage, _maxDamage);


    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(Damage);
        Attacked?.Invoke();
    }
}