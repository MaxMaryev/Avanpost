using System.Collections.Generic;
using UnityEngine;

public class GunShotSoundHandler : MonoBehaviour
{
    [SerializeField] private FiringWeapon _firingWeapon;
    [SerializeField] private List<AudioClip> _audioClips;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable() => _firingWeapon.Fired += OnWeaponFired;

    private void OnDisable() => _firingWeapon.Fired -= OnWeaponFired;

    private void OnWeaponFired(FiringWeapon firingWeapon)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Count)];
        _audioSource.Play();
    }
}
