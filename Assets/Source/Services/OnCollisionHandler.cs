using System;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    public event Action<Collision> CollisionEntered;
    public event Action<Collision> CollisionExited;
    public event Action<Collision> CollisionStaying;

    private void OnCollisionEnter(Collision collision) => CollisionEntered?.Invoke(collision);

    private void OnCollisionExit(Collision collision) => CollisionExited?.Invoke(collision);

    private void OnCollisionStay(Collision collision) => CollisionStaying?.Invoke(collision);
}
