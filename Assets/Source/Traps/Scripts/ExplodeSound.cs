using UnityEngine;

public class ExplodeSound : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _explode;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private IExplodable _explodable => (IExplodable) _explode;

    private void OnValidate()
    {
        if (_explode && !(_explode is IExplodable))
        {
            Debug.LogError(nameof(_explode) + " needs to implement " + nameof(IExplodable));
            _explode = null;
        }
    }

    private void OnEnable() => _explodable.Exploded += OnExploded;

    private void OnDisable() => _explodable.Exploded -= OnExploded;

    private void Start()
    {
        _audioSource.clip = _audioClip;
    }

    private void OnExploded(IExplodable explodable)
    {
        _audioSource.Play();
    }
}
