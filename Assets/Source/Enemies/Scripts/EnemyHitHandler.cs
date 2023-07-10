using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitHandler : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private List<CriticalHit> _criticalHits = new List<CriticalHit>();

    private bool _isBurn;
    private WaitForSeconds _waitBurningCooldown = new WaitForSeconds(0.2f);

    public GameObject GameObject => gameObject;

    public event Action<WeaponType> Hit;

    private void OnDisable()
    {
        _isBurn = false;
    }

    public void Visit(Pistol pistol, float damage)
    {
        ApplyHit(pistol, damage, WeaponType.Pistol);
    }

    public void Visit(Shotgun shotgun, float damage)
    {
        ApplyHit(shotgun, damage, WeaponType.Shotgun);
    }

    public void Visit(SniperRifle sniperRifle, float damage)
    {
        ApplyHit(sniperRifle, damage, WeaponType.SniperRifle);
    }

    public void Visit(AK aK, float damage)
    {
        ApplyHit(aK, damage, WeaponType.AK);
    }

    public void Visit(ShotgunTrap shotgunTrap, float damage)
    {
        ApplyHit(shotgunTrap, damage, WeaponType.Shotgun);
    }

    public void Visit(TwoBarrelsGun twoBarrelsGun, float damage)
    {
        ApplyHit(twoBarrelsGun, damage, WeaponType.Shotgun);
    }

    public void Visit(Hammer hammer, float damage)
    {
        ApplyHit(hammer, damage, WeaponType.Hammer);
    }

    public void Visit(Minigun minigun, float damage)
    {
        ApplyHit(minigun, damage, WeaponType.Minigun);
    }

    public void Visit(Flamethrower flamethrower, float damage)
    {
        ApplyHit(flamethrower, damage, WeaponType.Flamethrower);

        if (_isBurn == false)
            StartCoroutine(Burning());

        IEnumerator Burning()
        {
            _isBurn = true;
            flamethrower.BurnTarget(_enemy.SpineRigTransform);
            _enemy.EnemyLightInfluence.IncreaseLigthsCount();

            while (_isBurn)
            {
                ApplyHit(flamethrower, damage * 100, WeaponType.Flamethrower);
                yield return _waitBurningCooldown;
            }
        }
    }

    public void Visit(Mine mine, float damage) => TakeCriticalDamage(damage);

    private void ApplyHit(Weapon weapon, float damage, WeaponType weaponType)
    {
        Hit?.Invoke(weaponType);

        int random = UnityEngine.Random.Range(0, 100);

        if (random <= weapon.BonusToCritChance && _criticalHits.Count > 0)
            TakeCriticalDamage(damage);
        else
            TakeDamage(damage);

    }

    private void TakeCriticalDamage(float damage)
    {
        int random = UnityEngine.Random.Range(0, _criticalHits.Count);

        CriticalHit criticalDamage = _criticalHits[random];

        if (criticalDamage.TryTakeCriticalHit())
            TakeDamage(damage * criticalDamage.DamageMultiplier);
        else
            TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        _enemy.TakeDamage(damage);
    }

}
