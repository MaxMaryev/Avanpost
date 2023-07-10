using System;
using UnityEngine;

public class OnTriggerHandler : MonoBehaviour
{
    private Collider _collider;

    public event Action<Collider> TriggerEntered;
    public event Action<Collider> TriggerExited;
    public event Action<Collider> TriggerStaying;

    public Collider Collider => _collider;

    private void Awake() => _collider = GetComponent<Collider>();

    private void OnTriggerEnter(Collider other) => TriggerEntered?.Invoke(other);

    private void OnTriggerExit(Collider other) => TriggerExited?.Invoke(other);

    private void OnTriggerStay(Collider other) => TriggerStaying?.Invoke(other);
}
