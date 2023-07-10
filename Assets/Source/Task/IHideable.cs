using System;
using UnityEngine;

public interface IHideable 
{
    public event Action<IHideable> Hided;
    public void Hide();
}
