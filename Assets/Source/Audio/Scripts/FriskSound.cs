using UnityEngine;

public class FriskSound : MonoBehaviour
{
    [SerializeField] private GarbagePoint _garbagePoint;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void OnEnable()
    {
        _garbagePoint.FriskStateChanged += OnFriskStateChanged;
    }

    private void OnDisable()
    {
        _garbagePoint.FriskStateChanged -= OnFriskStateChanged;
    }

    private void Start()
    {
        _audioSource.clip = _audioClip;
    }

    private void OnFriskStateChanged(bool state)
    {
        if (state && _audioSource.isPlaying == false)
            _audioSource.Play();
        else if (state == false && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
