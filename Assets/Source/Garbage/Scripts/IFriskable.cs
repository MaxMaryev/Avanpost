using System;
using UnityEngine;

public interface IFriskable
{
    public void Frisk();
    public GameObject GameObject { get; }
    public Transform Transform { get; }
    public int NumberOfElements { get; }
    public Vector3 GetTargetPositionForSpawnElement();
    public bool IsFilled { get; }
    public event Action<IFriskable> Frisked;
}
