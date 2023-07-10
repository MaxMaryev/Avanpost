using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThemeSound : MonoBehaviour
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private ZombiesPool _zombiesPool;

    [SerializeField] private AudioSource _windAudioSource;
    [SerializeField] private AudioSource _dayAudioSource;
    [SerializeField] private AudioSource _nightAudioSource;
    [SerializeField] private AudioSource _nightAudioSource2;
    [SerializeField] private AudioSource _minorAudioSource;

    [SerializeField] private AudioClip _dayMusic;
    [SerializeField] private AudioClip _nightStartMusic;
    [SerializeField] private AudioClip _nightAttackMusic;
    [SerializeField] private AudioClip _nightFinishMusic;
    [SerializeField] private AudioClip _nightFinishAccordMusic;
    [SerializeField] private AudioClip _windSound;
    [SerializeField] private AudioClip _minorDaySounds;
    [SerializeField] private AudioClip _minorNightSounds;

    [SerializeField] private float _musicDayMaxVolume;
    [SerializeField] private float _musicNightMaxVolume;

    private Coroutine _nightThemeCoroutine;
    private Coroutine _fadeOutCoroutine;
    private Coroutine _fadeInCoroutine;

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    private void Start()
    {
        _windAudioSource.clip = _windSound;
        _windAudioSource.Play();

        _dayAudioSource.clip = _dayMusic;
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay is Night)
        {
            _nightThemeCoroutine = StartCoroutine(PlayNightTheme());
        }
        else if (timeDay is LateNight == false)
        {
            if (_dayAudioSource.isPlaying)
                return;

            if (_minorAudioSource.clip != _minorDaySounds)
            {
                _minorAudioSource.clip = _minorDaySounds;
                _minorAudioSource.Play();
            }

            if (_nightAudioSource.clip != null)
                FadeTransition(_nightAudioSource, _dayAudioSource, 2f, _musicNightMaxVolume);
            else if (_nightAudioSource2.clip != null)
                FadeTransition(_nightAudioSource2, _dayAudioSource, 2f, _musicNightMaxVolume);
            else
                StartCoroutine(AudioFadeScript.FadeIn(_dayAudioSource, 2f, _musicDayMaxVolume));
        }
    }

    private IEnumerator PlayNightTheme()
    {
        _nightAudioSource.loop = true;
        _minorAudioSource.clip = _minorNightSounds;
        _minorAudioSource.Play();

        yield return new WaitUntil(() => _zombiesPool.Enabled.Count > 0);

        _nightAudioSource.clip = _nightStartMusic;
        FadeTransition(_dayAudioSource, _nightAudioSource, 2, _musicNightMaxVolume);

        yield return new WaitForSeconds(5);

        _nightAudioSource2.loop = true;
        _nightAudioSource2.clip = _nightAttackMusic;
        FadeTransition(_nightAudioSource, _nightAudioSource2, 0.5f, _musicNightMaxVolume);

        yield return new WaitUntil(() => _dayCycleManager.CurrentTimeDay is LateNight);

        _nightAudioSource2.loop = false;
        _nightAudioSource.loop = true;
        _nightAudioSource.clip = _nightFinishMusic;
        FadeTransition(_nightAudioSource2, _nightAudioSource, 0.5f, _musicNightMaxVolume);

        yield return new WaitUntil(() => _zombiesPool.Enabled.Count == 0);
        _nightAudioSource2.clip = _nightFinishAccordMusic;
        FadeTransition(_nightAudioSource, _nightAudioSource2, 1, _musicNightMaxVolume);

        yield return new WaitUntil(() => _nightAudioSource2.isPlaying == false);

        _nightAudioSource2.clip = null;
        _dayAudioSource.clip = _dayMusic;
        StartCoroutine(AudioFadeScript.FadeIn(_dayAudioSource, 2f, _musicDayMaxVolume));

        _minorAudioSource.clip = _minorDaySounds;
        _minorAudioSource.Play();
    }

    private void FadeTransition(AudioSource audioSourceOld, AudioSource audioSourceNew, float transitDuration, float audioSourceNewMaxValue)
    {
        _fadeOutCoroutine = StartCoroutine(AudioFadeScript.FadeOut(audioSourceOld, transitDuration));
        _fadeInCoroutine = StartCoroutine(AudioFadeScript.FadeIn(audioSourceNew, transitDuration, audioSourceNewMaxValue));
    }
}
