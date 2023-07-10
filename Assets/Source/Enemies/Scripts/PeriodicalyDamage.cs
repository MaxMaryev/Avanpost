using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicalyDamage : MonoBehaviour
{
    [SerializeField] private float _durationTakeDamage;
    [SerializeField] private float _frequencyTakeDamage;

    private Coroutine _periodicallyTakeDamage;
    private IEnumerator PeriodicallyTakeDamage(IDamageable damageable)
    {
        float time = 0;

        StartCoroutine(TakeDamageDelay(damageable));

        while (damageable != null && damageable.IsAlive || time <= _durationTakeDamage)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _periodicallyTakeDamage = null;

        Destroy(gameObject);
    }

    private IEnumerator TakeDamageDelay(IDamageable damageable)
    {
        while (_periodicallyTakeDamage != null)
        {
            Debug.Log("Damage");
            //damageable.TakeDamage(_damage);
            yield return new WaitForSeconds(_frequencyTakeDamage);
        }
    }
}
