using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IAttacker))]
public class ZombieSound : MonoBehaviour
{
    [SerializeField] private Zombie _zombie;
    [SerializeField] private IAttacker _attacker;
    [SerializeField] private List<CriticalHit> _criticalHits = new List<CriticalHit>();
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClipsAttack = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _audioClipsCrit = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _audioClipsDeath = new List<AudioClip>();

    private void Awake()
    {
        _attacker = GetComponent<IAttacker>();
    }

    private void OnEnable()
    {
        _attacker.Attacked += OnAttacked;
        _zombie.Killed += OnZombieKilled;

        foreach (var crit in _criticalHits)
            crit.Applied += OnCritApplied;
    }

    private void OnDisable()
    {
        _attacker.Attacked -= OnAttacked;
        _zombie.Killed -= OnZombieKilled;

        foreach (var crit in _criticalHits)
            crit.Applied -= OnCritApplied;
    }

    private void OnZombieKilled(Enemy enemy, float dyingDuration) => PlaySound(_audioClipsDeath[Random.Range(0, _audioClipsDeath.Count)]);

    private void OnCritApplied(CriticalHit criticalHit) => PlaySound(_audioClipsCrit[Random.Range(0, _audioClipsCrit.Count)]);

    private void OnAttacked() => PlaySound(_audioClipsAttack[Random.Range(0, _audioClipsAttack.Count)]);

    private void PlaySound(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
