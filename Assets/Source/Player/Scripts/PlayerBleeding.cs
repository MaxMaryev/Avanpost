using UnityEngine;

public class PlayerBleeding : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _speed;

    private Health _health;

    private void OnDisable() => _health.ValueChanged -= OnHealthValueChanged;

    public void Init(Health health)
    {
        _health = health;
        _health.ValueChanged += OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float maxValue, float currentValue)
    {
        ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
        emissionModule.rateOverTime = (1 - currentValue / maxValue) * _speed;
    }
}
