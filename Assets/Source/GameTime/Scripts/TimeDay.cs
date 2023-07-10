using System;
using UnityEngine;

public abstract class TimeDay : MonoBehaviour
{
    [SerializeField] private float _startNormalizedTime;
    [SerializeField] private float _endNormalizedTime;
    [SerializeField] private Color _fogColor;
    [SerializeField] private Color _targetFogColor;
    [SerializeField] private float _startFogDistance;
    [SerializeField] private float _targetFogDistance;

    private float _currentTime;
    private float _maxTime;

    public event Action Started;
    public event Action Ended;
    public event Action<TimeDay> Completed;

    public float StartNormalizedTime => _startNormalizedTime;
    public float EndNormalizedTime => _endNormalizedTime;
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] protected Light DirectionLight { get; private set; }
    [field: SerializeField] protected float StartIntensity { get; private set; }
    [field: SerializeField] protected float TargetIntensity { get; private set; }
    [field: SerializeField] protected Gradient DirectionalGradient { get; private set; }

    public virtual void Enter()
    {
        _maxTime = _endNormalizedTime - _startNormalizedTime;
        Started?.Invoke();
        RenderSettings.fogColor = _fogColor;
    }

    public virtual void Exit()
    {
        Ended?.Invoke();
    }

    public virtual void Tick(float normalizedTime)
    {
        if (IsTimeInRange(normalizedTime) == false)
        {
            Completed?.Invoke(this);
            return;
        }

        _currentTime = _maxTime - (_endNormalizedTime - normalizedTime);

        RenderSettings.ambientLight = DirectionalGradient.Evaluate(Mathf.Lerp(0, 1, _currentTime / _maxTime));

        DirectionLight.intensity = Mathf.Lerp(StartIntensity, TargetIntensity, _currentTime / _maxTime);

        if (_startFogDistance != _targetFogDistance)
            RenderSettings.fogEndDistance = Mathf.Lerp(_startFogDistance, _targetFogDistance, _currentTime / _maxTime);

        if (_fogColor != _targetFogColor)
            RenderSettings.fogColor = Color.Lerp(_fogColor, _targetFogColor, _currentTime / _maxTime);
    }

    public bool IsTimeInRange(float normalizedTime)
    {
        return normalizedTime >= _startNormalizedTime && normalizedTime < _endNormalizedTime;
    }
}
