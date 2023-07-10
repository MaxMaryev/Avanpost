using System;
using System.Collections;
using UnityEngine;

public class ZombieRot : MonoBehaviour
{
    [SerializeField] private OnTriggerHandler _onTriggerHandler;
    [SerializeField] private int _periodicalyDamage;
    [SerializeField] private float _frequencyTakeDamage;
    [SerializeField] private float _range;
    [SerializeField] private SphereCollider _sphereCollider;

    private Coroutine _periodicallyTakeDamage;

    private void Start()
    {
        _sphereCollider.radius = _range;
    }

    private void OnEnable()
    {
        _onTriggerHandler.TriggerEntered += TriggerEntered;
        _onTriggerHandler.TriggerExited += TriggerExited;
    }

    private void OnDisable()
    {
        _onTriggerHandler.TriggerEntered -= TriggerEntered;
        _onTriggerHandler.TriggerExited -= TriggerExited;

        if (_periodicallyTakeDamage != null)
            StopCoroutine(_periodicallyTakeDamage);
    }

    private void TriggerEntered(Collider collider)
    {
        IDamageable damageable = collider.GetComponentInParent<IDamageable>();

        if (damageable != null && damageable.IsAlive && damageable as Player)
        {
            if (_periodicallyTakeDamage != null)
                StopCoroutine(_periodicallyTakeDamage);

            _periodicallyTakeDamage = StartCoroutine(PeriodicallyTakeDamage(damageable));
        }
    }

    private void TriggerExited(Collider collider)
    {
        if (_periodicallyTakeDamage != null)
            StopCoroutine(_periodicallyTakeDamage);
    }

    private IEnumerator PeriodicallyTakeDamage(IDamageable damageable)
    {
        while (damageable != null && damageable.IsAlive)
        {
            damageable.TakeDamage(_periodicalyDamage);
            yield return new WaitForSeconds(_frequencyTakeDamage);
        }
    }
}
