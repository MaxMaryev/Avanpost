using System;
using UnityEngine;

public interface IAttacker
{
    public event Action Attacked;
    public float Range { get; }
    public int Damage { get; }
    public void Attack(IDamageable damageable);
}
