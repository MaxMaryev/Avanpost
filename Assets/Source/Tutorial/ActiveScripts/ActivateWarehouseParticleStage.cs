using System;
using UnityEngine;

public class ActivateWarehouseParticleStage : TutorialStage
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _craftMenuPoint;
    [SerializeField] private OnTriggerHandler _onTriggerHandler;
    [SerializeField] private GameObject _rootCraftMenuPoint;
    [SerializeField] private DayCycleManager _dayCycleManager;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay is Night)
            return;

        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
        _rootCraftMenuPoint.SetActive(true);
        _craftMenuPoint.SetActive(true);
        _particleSystem.gameObject.SetActive(true);
        _particleSystem.Play();
        _onTriggerHandler.enabled = true;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        
    }
}
