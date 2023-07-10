using System;
using System.Collections;
using UnityEngine;

public abstract class Resource : MonoBehaviour, IPickable
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] public Collider _collider;

    private int _amountReward;
    private bool _canPick;
    private Coroutine _coroutine;
    private Vector3 _targetPosition;
    private float _timeMoveToTarget = 0.5f;

    public event Action<Resource> Picked;

    public int Amount => _amountReward;

    public void Init(int amountReward, Vector3 targetPosition)
    {
        _amountReward = amountReward;
        _targetPosition = targetPosition;
        _canPick = false;
        _coroutine = StartCoroutine(MoveToTarget());
    }

    public void Pick()
    {
        if (_canPick)
            Picked?.Invoke(this);
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        float time = 0;
        _collider.enabled = false;

        while (time < _timeMoveToTarget)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, new Vector3(_targetPosition.x, _targetPosition.y + _animationCurve.Evaluate(time / _timeMoveToTarget), _targetPosition.z), time / _timeMoveToTarget);
            yield return null;
        }

        _collider.enabled = true;
        _canPick = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Pick();
        }
    }
}
