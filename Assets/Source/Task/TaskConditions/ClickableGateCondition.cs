using System;
using UnityEngine;

public class ClickableGateCondition : MonoBehaviour, ITaskCondition
{
    [SerializeField] private ContainersHandler _containersHandler;

    private Gate _gate;
    private bool _isGateOppened;

    public event Action Updated;

    private void OnEnable()
    {
        if (_gate != null)
            _gate.Opened += OnGateOppened;

        _isGateOppened = false;
    }

    private void OnDisable()
    {
        if (_gate != null)
            _gate.Opened -= OnGateOppened;
    }

    public bool CompleteConditionMet()
    {
        return _isGateOppened;
    }

    public bool FailConditionMet()
    {
        return _gate == null;
    }

    public bool StartConditionMet()
    {
        _gate = _containersHandler.GetComponentInChildren<Gate>();
        return _gate != null;
    }

    private void OnGateOppened(Gate gate)
    {
        gate.Opened -= OnGateOppened;
        gate = null;
        _isGateOppened = true;
        Updated?.Invoke();
    }
}
