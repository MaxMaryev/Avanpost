using System;
using System.Collections;
using UnityEngine;

public class Navigation : MonoBehaviour, INavigatable
{
    [SerializeField] private Transform _arrow;

    private Vector3 _currentTarget;
    private Coroutine _coroutine;

    public event Action Reached;

    public void Navigate(Vector3 target)
    {
        _currentTarget = target;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(LookAtTarget());
    }

    private IEnumerator LookAtTarget()
    {
        yield return new WaitForEndOfFrame();

        _arrow.gameObject.SetActive(true);

        while ((_currentTarget - transform.position).SqrMagnitudeXZ() > 3f)
        {
            transform.rotation = Quaternion.LookRotation((_currentTarget - transform.position), Vector3.up);
            yield return null;
        }

        Reached?.Invoke();
        _arrow.gameObject.SetActive(false);
    }
}
