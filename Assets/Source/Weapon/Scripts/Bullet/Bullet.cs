using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private FiringWeapon _weapon;
    [SerializeField] private ParticleSystem _particleSystem;

    private float _damagePerBullet;
    private float _totalDamage;
    private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake() => _damagePerBullet = _weapon.MaxDamage / _weapon.ClipCapacity;

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IWeaponVisitor visitor))
        {
            int collisionsCount = _particleSystem.GetCollisionEvents(visitor.GameObject, _collisionEvents);
            _totalDamage = collisionsCount * _damagePerBullet;
             _weapon.Accept(visitor, _totalDamage);
        }
    }
}
