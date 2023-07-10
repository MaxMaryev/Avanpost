using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbagePoint : MonoBehaviour, IFriskable
{
    [SerializeField] private List<Transform> _garbageTargetPoints = new List<Transform>();
    [SerializeField] private OnTriggerHandler _triggerHandler;
    [SerializeField] private float _friskDelay = 2f;
    [SerializeField] private PlayerMover _playerMover;

    private int _countGarbage;
    private IFriskAnimationsHandler _friskAnimationsHandler;
    private Coroutine _coroutine;
    private bool _isFrisking;

    public bool IsFilled => _countGarbage > 0;
    public int NumberOfElements => _countGarbage;
    public GameObject GameObject => gameObject;
    public Transform Transform => transform;

    public event Action<bool> FriskStateChanged;
    public event Action<IFriskable> Frisked;

    private void OnEnable()
    {
        _triggerHandler.TriggerEntered += OnTriggerEntered;
        _triggerHandler.TriggerExited += OnTriggerExited;
    }

    private void OnDisable()
    {
        _triggerHandler.TriggerEntered -= OnTriggerEntered;
        _triggerHandler.TriggerExited -= OnTriggerExited;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _friskAnimationsHandler.StopAnimationFrisk();
    }

    public void Init(int countGarbage, IFriskAnimationsHandler friskAnimationsHandler)
    {
        _countGarbage = countGarbage;
        _friskAnimationsHandler = friskAnimationsHandler;
    }

    public void Frisk()
    {
        Frisked?.Invoke(this);
    }

    public Vector3 GetTargetPositionForSpawnElement()
    {
        Vector3 targetPosition = _garbageTargetPoints[UnityEngine.Random.Range(0, _garbageTargetPoints.Count)].position;
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(0f, 1f), 0, UnityEngine.Random.Range(0f, 1f));
        return targetPosition + randomOffset;
    }

    private IEnumerator FriskDealy()
    {
        float time = 0;

        while (time < _friskDelay)
        {   
            if (_playerMover.IsMoving)
            {
                time = 0;
                _isFrisking = false;
                FriskStateChanged?.Invoke(_isFrisking);
                _friskAnimationsHandler.StopAnimationFrisk();
            } 
            else
            {
                time += Time.deltaTime;
                _isFrisking = true;
                FriskStateChanged?.Invoke(_isFrisking);
                _friskAnimationsHandler.PlayAnimationFrisk();
            }

            yield return null;
        }

        _isFrisking = false;
        FriskStateChanged?.Invoke(_isFrisking);
        Frisk();
    }

    private void OnTriggerEntered(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _coroutine = StartCoroutine(FriskDealy());
        }
    }

    private void OnTriggerExited(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _friskAnimationsHandler.StopAnimationFrisk();
            _isFrisking = false;
            FriskStateChanged?.Invoke(_isFrisking);
        }
    }
}
