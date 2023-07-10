using System;
using UnityEngine;

public class Wallet : SavedObject<Wallet>
{
    public Wallet(int value, string guid) : base(guid) { Value = value; }

    public event Action ValueChanged;

    [field: SerializeField] public int Value { get; protected set; }

    public void Add(int value)
    {
        Value += value;
        ValueChanged?.Invoke();
    }

    public void Spend(int value)
    {
        Value -= value;

        if (Value < 0)
            Value = 0;

        ValueChanged?.Invoke();
    }

    protected override void OnLoad(Wallet loadedObject) => Value = loadedObject.Value;
}
