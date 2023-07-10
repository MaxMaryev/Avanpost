using System;
using UnityEngine;

public interface INavigatable
{
    public event Action Reached;
    public void Navigate(Vector3 target);
}
