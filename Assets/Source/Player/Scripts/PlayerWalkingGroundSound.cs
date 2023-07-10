using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingGroundSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _audioClips;
    [SerializeField] private AudioSource _audioSource;

    private void PlaySound()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Count)];
        _audioSource.Play();
    }
}
