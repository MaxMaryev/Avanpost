using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _firstDestroyableElements;
    [SerializeField] private List<Rigidbody> _lastDestroyableElements;

    private float _damageTillDestroy = 100;
    private float _damageReceived;

    private void Start() => StartCoroutine(TakingDamage());

    private IEnumerator TakingDamage()
    {
        float delay = 1;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        while (_damageReceived < _damageTillDestroy)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1), transform.rotation, Layers.Enemy);

            foreach (var enemy in colliders)
                _damageReceived += 1;

            if (_damageTillDestroy / _damageReceived < 2)
                foreach (var wallElement in _firstDestroyableElements)
                    wallElement.constraints = RigidbodyConstraints.None;

            yield return waitForSeconds;
        }

        foreach (var wallElement in _lastDestroyableElements)
            wallElement.constraints = RigidbodyConstraints.None;
    }
}
