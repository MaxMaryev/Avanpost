using System;
using System.Collections;

public interface ISafetyMarginable
{
    public event Action<float, float, Building> SafetyMarginChanged;

    public float MaxSafetyMargin { get; }
    public float CurrentSafetyMargin { get; }

    //public IEnumerator Breaking();
    public void Repair();
}
