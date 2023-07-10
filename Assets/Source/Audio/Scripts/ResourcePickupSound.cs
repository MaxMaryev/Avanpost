using System;
using UnityEngine;

public class ResourcePickupSound : MonoBehaviour
{
    [SerializeField] private GarbageSpawner _garbageSpawner;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        _garbageSpawner.ResourceSpawned += OnResourceSpawned;
    }

    private void OnDisable()
    {
        _garbageSpawner.ResourceSpawned -= OnResourceSpawned;
    }

    private void OnResourceSpawned(Resource resource)
    {
        resource.Picked += OnPicked;
    }

    private void OnPicked(Resource resource)
    {
        resource.Picked -= OnPicked;

        PlaySound(_audioClip);
    }

    private void PlaySound(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
