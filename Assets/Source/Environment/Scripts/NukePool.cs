using System.Collections;
using UnityEngine;

public class NukePool : MonoBehaviour
{
    [SerializeField] private float _damage;

    private WaitForSeconds _waitDamageCooldown = new WaitForSeconds(0.5f);
    private Coroutine _damagingCoroutine;

    private void OnDisable()
    {
        if (_damagingCoroutine != null)
            StopCoroutine(_damagingCoroutine);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _damagingCoroutine = StartCoroutine(Damaging(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            if(_damagingCoroutine != null)
                StopCoroutine(_damagingCoroutine);
    }

    private IEnumerator Damaging(IDamageable target)
    {
        while(target.IsAlive)
        {
            target.TakeDamage(_damage);
            yield return _waitDamageCooldown;
        }
    }
}
