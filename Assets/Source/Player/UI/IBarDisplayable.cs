using System;

public interface IBarDisplayable
{
    public float MaxValue { get; }
    public float CurrentValue { get; }

    public event Action<float, float> ValueChanged;
}
