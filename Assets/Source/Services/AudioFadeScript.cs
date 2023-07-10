using System.Collections;
using UnityEngine;

public static class AudioFadeScript
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        float time = 0;

        while (audioSource.volume > 0)
        {
            time += Time.deltaTime;
            float normilizedDuration = time / FadeTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, normilizedDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        audioSource.clip = null;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float maxValue)
    {
        audioSource.volume = 0;
        float startVolume = 0;
        float time = 0;
        audioSource.Play();

        while (audioSource.volume < maxValue)
        {
            time += Time.deltaTime;
            float normilizedDuration = time / FadeTime;
            audioSource.volume = Mathf.Lerp(startVolume, 1, normilizedDuration);
            yield return null;
        }
    }
}