using System;
using UnityEngine;

public interface IExplodable
{
    public event Action<IExplodable> Exploded;
    public Transform Transform { get; }
}
