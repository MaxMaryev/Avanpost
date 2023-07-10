using System.Collections;
using UnityEngine;

public class SelfDamaging : MonoBehaviour
{
    [SerializeField] private OnTriggerHandler _breakTriggerHandler;
    [SerializeField] private Building _building;

    private Coroutine _breakingCoroutine;

    private void OnEnable()
    {
        _breakTriggerHandler.TriggerEntered += OnBreakTriggerEntered;
    }

    private void OnDisable()
    {
        _breakTriggerHandler.TriggerEntered -= OnBreakTriggerEntered;
    }

    private IEnumerator Breaking()
    {
        float delay = 0.25f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        while (_building.CurrentSafetyMargin > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(_breakTriggerHandler.Collider.transform.position, (_breakTriggerHandler.Collider as SphereCollider).radius, Layers.Enemy);

            if (colliders.Length == 0)
            {
                _breakingCoroutine = null;
                yield break;
            }

            _building.TakeDamage(colliders.Length);
            yield return waitForSeconds;
        }
    }

    private void OnBreakTriggerEntered(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
            if (_breakingCoroutine == null)
                _breakingCoroutine = StartCoroutine(Breaking());
    }
}
