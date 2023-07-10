using UnityEngine;

public class WorkbenchOpenSound : MonoBehaviour
{
    [SerializeField] private Workbench _workbench;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void OnEnable()
    {
        _workbench.OpenedCraftMenu += OnOpened;
    }

    private void OnDisable()
    {
        _workbench.OpenedCraftMenu -= OnOpened;
    }

    private void OnOpened() => PlaySound(_audioClip);

    private void PlaySound(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
