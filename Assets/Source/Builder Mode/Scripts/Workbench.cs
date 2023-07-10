using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private OnTriggerHandler _onTriggerHandler;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Collider _triggerEnter;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private List<ObjectsPool> _objectsPools;

    private float _lastSpeedMultiplier;
    private Coroutine _coroutineActivateDelay;

    public event Action OpenedCraftMenu;
    public event Action CloseCraftMenu;

    private void OnEnable()
    {
        _onTriggerHandler.TriggerEntered += OnTriggerEntered;
        _onTriggerHandler.TriggerExited += OnTriggerExited;
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _onTriggerHandler.TriggerEntered -= OnTriggerEntered;
        _onTriggerHandler.TriggerExited -= OnTriggerExited;
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    private void OnTriggerEntered(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            Open();
    }

    private void OnTriggerExited(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            CLose();
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (_coroutineActivateDelay != null)
            StopCoroutine(_coroutineActivateDelay);

        if (timeDay as Night)
            _onTriggerHandler.gameObject.SetActive(false);
        else if (timeDay as LateNight)
            _coroutineActivateDelay = StartCoroutine(ActivateDelay());
        else
            _onTriggerHandler.gameObject.SetActive(true);
    }

    private void Open()
    {
        _lastSpeedMultiplier = _dayCycleManager.TimeSpeedMultiplier;

        if (_lastSpeedMultiplier != 0)
            _dayCycleManager.FreezeTime();

        OpenedCraftMenu?.Invoke();
    }

    private void CLose()
    {
        if (_lastSpeedMultiplier != 0)
            _dayCycleManager.UnFreezeTime();

        CloseCraftMenu?.Invoke();
    }

    private IEnumerator ActivateDelay()
    {
        yield return new WaitUntil(() => GetLenghtPools() == 0);
        _onTriggerHandler.gameObject.SetActive(true);
    }

    private int GetLenghtPools() => _objectsPools.Where(x => x.Enabled.Count > 0).Count();
}

