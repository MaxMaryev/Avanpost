using UnityEngine;

public abstract class PlayClickSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    protected void PlaySound()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
