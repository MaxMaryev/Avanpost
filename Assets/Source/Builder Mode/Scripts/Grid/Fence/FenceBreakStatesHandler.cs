using System;
using UnityEngine;

[Serializable]
public class FenceBreakStatesHandler : MonoBehaviour
{
    [SerializeField] private Fence _fence;
    [SerializeField] private BreakState _safetyMargin100;
    [SerializeField] private BreakState _safetyMargin75;
    [SerializeField] private BreakState _safetyMargin50;
    [SerializeField] private BreakState _safetyMargin25;

    private float _currentSafetyMarginShare;
    private BreakState _currentBreakState;

    private void OnEnable() => _fence.SafetyMarginChanged += OnSafetyMarginChanged;

    private void OnDisable() => _fence.SafetyMarginChanged -= OnSafetyMarginChanged;


    public void UpdateBreakState(float maxValue, float currentValue)
    {
        if (_safetyMargin75 == null)
            return;

        _currentSafetyMarginShare = currentValue / maxValue;

        if (_currentSafetyMarginShare > 0.75f)
            TrySetNewBreakState(_safetyMargin100);
        else if (_currentSafetyMarginShare <= 0.75f && _currentSafetyMarginShare > 0.5f)
            TrySetNewBreakState(_safetyMargin75);
        else if (_currentSafetyMarginShare <= 0.5f && _currentSafetyMarginShare > 0.25f)
            TrySetNewBreakState(_safetyMargin50);
        else
            TrySetNewBreakState(_safetyMargin25);

        void TrySetNewBreakState(BreakState breakState)
        {
            if (breakState != _currentBreakState)
            {
                if (_currentBreakState != null)
                    _currentBreakState.gameObject.SetActive(false);

                breakState.gameObject.SetActive(true);
                _currentBreakState = breakState;

                if (breakState != _safetyMargin100)
                    _safetyMargin100.gameObject.SetActive(false);
            }
        }
    }

    private void OnSafetyMarginChanged(float maxValue, float currentValue, Building building = null) => UpdateBreakState(maxValue, currentValue);
}