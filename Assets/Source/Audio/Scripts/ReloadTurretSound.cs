using System;
using UnityEngine;

public class ReloadTurretSound : MonoBehaviour
{
    [SerializeField] private FiringWeapon _firingWeapon;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void OnEnable()
    {
        _firingWeapon.ReloadStateChanged += OnReloadStateChanged;
    }

    private void OnDisable()
    {
        _firingWeapon.ReloadStateChanged -= OnReloadStateChanged;
    }

    private void Start()
    {
        _audioSource.clip = _audioClip;
    }

    private void OnReloadStateChanged(bool state)
    {
        if (state && _audioSource.isPlaying == false)
            _audioSource.Play();
        else if (state == false && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
