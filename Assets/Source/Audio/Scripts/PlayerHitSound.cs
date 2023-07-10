using System.Collections.Generic;
using UnityEngine;

public class PlayerHitSound : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClipsHit;
    [SerializeField] private AudioClip _audioDeath;

    private float _lastValue;

    private void OnDisable() 
    { 
        _player.Health.ValueChanged -= OnValueChanged;
        _player.Health.UnderZero -= OnPlayerDeath;
    }

    private void Start()
    {
        _player.Health.ValueChanged += OnValueChanged;
        _player.Health.UnderZero += OnPlayerDeath;
    }

    private void OnPlayerDeath() => PlayClip(_audioDeath);

    private void OnValueChanged(float maxValue, float currentValue)
    {
        if (currentValue < maxValue)
        {
            if (_lastValue > currentValue)
                PlayClip(_audioClipsHit[Random.Range(0, _audioClipsHit.Count)]);

            _lastValue = currentValue;
        }
    }

    private void PlayClip(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
