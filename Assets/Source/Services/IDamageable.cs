using System;

public interface IDamageable
{
    public bool IsAlive { get; }

    void TakeDamage(float damage);
}
