using UnityEngine;

public class GateSound : MonoBehaviour
{
    [SerializeField] private Gate _gate;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClipOpen;
    [SerializeField] private AudioClip _audioClipClose;

    private void OnEnable()
    {
        _gate.Opened += OnOpened;
        _gate.Closed += OnClosed;
    }

    private void OnDisable()
    {
        _gate.Opened -= OnOpened;
        _gate.Closed -= OnClosed;
    }

    private void OnOpened(Gate gate) => PlaySound(_audioClipOpen);

    private void OnClosed(Gate gate) => PlaySound(_audioClipClose);

    private void PlaySound(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
