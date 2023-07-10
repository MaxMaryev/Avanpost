using System;
using UnityEngine;

public class FirstAttackZombieStage : TutorialStage
{
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private string _messageForHint;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private DayCycleManager _dayCycleManager;

    private float _targetNormalizedTime = 0.74f;

    public override event Action<TutorialStage> Completed;

    private void Awake() => _enemiesSpawner.enabled = false;

    protected override void OnEnter()
    {
        _dayCycleManager.RewindTime(_targetNormalizedTime);
        _hintHandler.Add(_messageForHint);
        _enemiesSpawner.enabled = true;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        
    }
}
