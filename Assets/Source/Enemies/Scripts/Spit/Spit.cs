using System;
using System.Collections;
using UnityEngine;

public class Spit : MonoBehaviour
{
    [SerializeField] private float _durationTakeDamage;
    [SerializeField] private float _frequencyTakeDamage;
    [SerializeField] private ParticleSystem _spitEffect;

    private int _damage;
    private Vector3 _targetPosition;
    private float _speed;
    private Coroutine _periodicallyTakeDamage;
    private Coroutine _takeDamageDelay;

    public void Init(Vector3 targetPosition, int damage, float speed)
    {
        _damage = damage;
        _targetPosition = targetPosition;
        _speed = speed;
        float distance = Vector3.Distance(_targetPosition, transform.position);
        var mainParticle = _spitEffect.main;
        mainParticle.startSpeed = _speed;
        var shape = _spitEffect.shape;
        float angle = ((distance * Physics.gravity.y) / Mathf.Pow(_speed, 2) * Mathf.Rad2Deg) / 2;
        shape.rotation = new Vector3(angle, 0, 0);
        _spitEffect.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null && damageable.IsAlive)
        {
            if (_periodicallyTakeDamage != null)
                StopCoroutine(_periodicallyTakeDamage);

            _periodicallyTakeDamage = StartCoroutine(PeriodicallyDamage(damageable));
        }
    }
    private IEnumerator PeriodicallyDamage(IDamageable damageable)
    {
        float time = 0;

        _takeDamageDelay = StartCoroutine(DamageDelay(damageable));

        while (damageable != null && damageable.IsAlive && time <= _durationTakeDamage)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _periodicallyTakeDamage = null;

        StopCoroutine(_takeDamageDelay);
    }

    private IEnumerator DamageDelay(IDamageable damageable)
    {
        while (true)
        {
            damageable.TakeDamage(_damage);
            yield return new WaitForSeconds(_frequencyTakeDamage);
        }
    }
}
