using UnityEngine;

public class FlameShotSound : MonoBehaviour
{
    [SerializeField] private FiringWeapon _firingWeapon;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable() => _firingWeapon.Fired += OnWeaponFired;

    private void OnDisable() => _firingWeapon.Fired -= OnWeaponFired;

    private void Start()
    {
        _audioSource.clip = _audioClip;
    }

    private void OnWeaponFired(FiringWeapon firingWeapon)
    {
        if (_audioSource.clip != _audioClip)
        {
            _audioSource.Stop();
            _audioSource.clip = _audioClip;
        }

        if (_audioSource.isPlaying == false)
            _audioSource.Play();
    }
}
