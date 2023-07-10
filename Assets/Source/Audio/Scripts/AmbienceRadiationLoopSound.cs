using UnityEngine;

public class AmbienceRadiationLoopSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void Start()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
