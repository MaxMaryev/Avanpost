using System;
using UnityEngine;

public class Health : SavedObject<Health>, IBarDisplayable
{
    [field: SerializeField] public float MaxValue { get; private set; }
    [field: SerializeField] public float CurrentValue { get; private set; }

    public Health(float maxValue) : base(nameof(Health)) 
    {
        MaxValue = maxValue;
        CurrentValue = maxValue;
    }

    public event Action UnderZero;
    public event Action<float, float> ValueChanged;

    public void Decrease(float value)
    {
        CurrentValue -= value;
        ValueChanged?.Invoke(MaxValue, CurrentValue);

        if(CurrentValue <= 0)
        {
            UnderZero?.Invoke();
        }
    }

    public void Increase(float value)
    {
        CurrentValue = Mathf.Clamp(CurrentValue + value, 0, MaxValue);
        ValueChanged?.Invoke(MaxValue, CurrentValue);
    }

    protected override void OnLoad(Health loadedObject)
    {
        CurrentValue = loadedObject.CurrentValue;
    }
}
