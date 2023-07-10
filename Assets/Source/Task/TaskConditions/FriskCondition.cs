using System;
using System.Linq;
using UnityEngine;

public class FriskCondition : MonoBehaviour, ITaskCondition, ICountable
{
    [SerializeField] private DayCycleManager _dayCycleManager;

    private int _targetNumberOfFrisk;
    private int _currentNumberFrisked;
    private IFriskableHandler _friskableHandler;

    public int TargetNumber => _targetNumberOfFrisk;

    public int CurrentNumber => _currentNumberFrisked;

    public event Action Updated;

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
        _currentNumberFrisked = 0;

        if (_friskableHandler == null)
            return;

        foreach (var friskable in _friskableHandler.Friskables)
            friskable.Frisked += OnFrisked;

        _currentNumberFrisked = _friskableHandler.NumbersOfAvailableFriskable - _friskableHandler.Friskables.Where(x => x.GameObject.activeSelf).Count();
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;

        if (_friskableHandler == null)
            return;

        foreach (var friskable in _friskableHandler.Friskables)
            friskable.Frisked -= OnFrisked;
    }

    public void Init(IFriskableHandler friskableHandler, int targetNumberOfFrisk)
    {
        _friskableHandler = friskableHandler;
        _targetNumberOfFrisk = targetNumberOfFrisk;
    }

    public bool CompleteConditionMet()
    {
        return _currentNumberFrisked >= _targetNumberOfFrisk;
    }

    public bool FailConditionMet()
    {
        return _dayCycleManager.CurrentTimeDay is Night;
    }

    public bool StartConditionMet()
    {
        return _friskableHandler.Friskables.Where(x => x.GameObject.activeSelf).Count() > 0;
    }

    private void OnFrisked(IFriskable friskable)
    {
        friskable.Frisked -= OnFrisked;
        _currentNumberFrisked++;

        Updated?.Invoke();
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        Updated?.Invoke();
    }
}
