using System;
using UnityEngine;

public interface IShowable
{
    public bool IsShow { get; }
    public event Action<IShowable> Showed;
    public void Show();
}
