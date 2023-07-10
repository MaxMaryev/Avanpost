using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private List<Light> _lights = new List<Light>();

    private Coroutine _coroutineSmoothEnabling;
    private Coroutine _coroutineSmoothDisabling;
    private Dictionary<Light, float> _lightsMaxIntencities = new Dictionary<Light, float>();
    private Dictionary<Light, Coroutine> _smoothLights = new Dictionary<Light, Coroutine>();

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;

        foreach (var light in _lights)
            if (_lightsMaxIntencities.ContainsKey(light) == false)
                _lightsMaxIntencities.Add(light, light.intensity);

        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;

        if (_coroutineSmoothDisabling != null)
            StopCoroutine(_coroutineSmoothDisabling);

        if (_coroutineSmoothEnabling != null)
            StopCoroutine(_coroutineSmoothEnabling);
    }

    public void AddLight(Light light)
    {
        _lights.Add(light);

        if (_lightsMaxIntencities.ContainsKey(light) == false)
            _lightsMaxIntencities.Add(light, light.intensity);

        if (_dayCycleManager.CurrentTimeDay is Evening || _dayCycleManager.CurrentTimeDay is LateNight || _dayCycleManager.CurrentTimeDay is Night)
            EnableLight(light, true);
        else
            EnableLight(light, false);

        //if (DayCycleManager.CurrentTimeDay is Evening || DayCycleManager.CurrentTimeDay is LateNight || DayCycleManager.CurrentTimeDay is Night)
        //{
        //    EnableLight(light, true);

        //    if (_smoothLights.ContainsKey(light) == false)
        //        _smoothLights.Add(light, StartCoroutine(SmoothLightEnableDelay(light, true)));
        //    else
        //    {
        //        StopCoroutine(_smoothLights[light]);
        //        _smoothLights[light] = StartCoroutine(SmoothLightEnableDelay(light, true));
        //    }
        //}
        //else
        //    EnableLight(light, false);
    }

    public void DeleteLight(Light light)
    {
        if (light.enabled)
        {
            if (_smoothLights.ContainsKey(light) == false)
                _smoothLights.Add(light, StartCoroutine(SmoothLightEnableDelay(light, false)));
            else
            {
                StopCoroutine(_smoothLights[light]);
                _smoothLights[light] = StartCoroutine(SmoothLightEnableDelay(light, false));
            }
        }

        _lights.Remove(light);
    }

    private void OnTimeDayChanged(TimeDay timeDay) => AdjustLights(timeDay);

    private void AdjustLights(TimeDay timeDay)
    {
        if (_coroutineSmoothDisabling != null)
        {
            StopCoroutine(_coroutineSmoothDisabling);
            _coroutineSmoothDisabling = null;
        }

        if (_coroutineSmoothEnabling != null)
        {
            StopCoroutine(_coroutineSmoothEnabling);
            _coroutineSmoothEnabling = null;
        }

        if (timeDay is Evening)
            _coroutineSmoothEnabling = StartCoroutine(SmoothLightsEnabling(timeDay.StartNormalizedTime, timeDay.EndNormalizedTime));
        else if (timeDay is Night)
            EnableLights(true);
        else if (timeDay is LateNight)
            _coroutineSmoothDisabling = StartCoroutine(SmoothLightsDisabling(timeDay.EndNormalizedTime));
        else if (timeDay is Dawn || timeDay is Day)
            EnableLights(false);
    }

    private void EnableLights(bool state)
    {
        foreach (var light in _lights)
            if (light != null)
                EnableLight(light, state);
    }

    private void EnableLight(Light light, bool state)
    {
        light.enabled = state;

        if (state == false)
            light.intensity = 0;

        if (_dayCycleManager.CurrentTimeDay is Evening || _dayCycleManager.CurrentTimeDay is LateNight || _dayCycleManager.CurrentTimeDay is Night)
        {
            if (_smoothLights.ContainsKey(light) == false)
                _smoothLights.Add(light, StartCoroutine(SmoothLightEnableDelay(light, true)));
            else
            {
                StopCoroutine(_smoothLights[light]);
                _smoothLights[light] = StartCoroutine(SmoothLightEnableDelay(light, true));
            }
        }
    }

    private IEnumerator SmoothLightEnableDelay(Light light, bool state)
    {
        float time = 0;
        float targetTime = 1.5f;

        float targetIntencities = 0;

        while (time < targetTime)
        {
            if (_coroutineSmoothDisabling != null && state)
            {
                float normalizedTime = _dayCycleManager.CurrentNormalizedTime / _dayCycleManager.CurrentTimeDay.EndNormalizedTime;
                targetIntencities = Mathf.Lerp(_lightsMaxIntencities[light], 0, normalizedTime);
            }
            else if (_coroutineSmoothEnabling != null && state)
            {
                float duration = _dayCycleManager.CurrentTimeDay.EndNormalizedTime - _dayCycleManager.CurrentTimeDay.StartNormalizedTime;
                float normalizedTime = (_dayCycleManager.CurrentNormalizedTime - _dayCycleManager.CurrentTimeDay.StartNormalizedTime) / duration;
                targetIntencities = Mathf.Lerp(0, _lightsMaxIntencities[light], normalizedTime);
            }
            else
                targetIntencities = state ? _lightsMaxIntencities[light] : 0;

            time += Time.deltaTime;
            light.intensity = Mathf.MoveTowards(light.intensity, targetIntencities, time / targetTime);
            yield return null;
        }

        if (light.intensity <= 0.01f)
            light.enabled = false;

        _smoothLights.Remove(light);
    }

    private IEnumerator SmoothLightsEnabling(float startTime, float finishTime)
    {
        float duration = finishTime - startTime;

        foreach (var light in _lights)
        {
            if (light != null)
            {
                light.intensity = 0;
                light.enabled = true;
            }
        }

        float normalizedTime = 0;

        while (normalizedTime < 1)
        {
            normalizedTime = (_dayCycleManager.CurrentNormalizedTime - startTime) / duration;

            foreach (var light in _lights)
            {
                if (light != null && _smoothLights.ContainsKey(light) == false)
                    light.intensity = Mathf.Lerp(0, _lightsMaxIntencities[light], normalizedTime);
            }

            yield return null;
        }

        _coroutineSmoothEnabling = null;
    }

    private IEnumerator SmoothLightsDisabling(float finishTime)
    {
        foreach (var light in _lights)
        {
            if (light != null)
            {
                if (light.enabled == false)
                    light.enabled = true;

                light.intensity = _lightsMaxIntencities[light];
            }
        }

        float normalizedTime = 0;

        while (normalizedTime < 1)
        {
            normalizedTime = _dayCycleManager.CurrentNormalizedTime / finishTime;

            foreach (var light in _lights)
            {
                if (light != null && _smoothLights.ContainsKey(light) == false)
                {
                    light.intensity = Mathf.Lerp(_lightsMaxIntencities[light], 0, normalizedTime);

                    if (light.intensity <= 0.01f)
                        light.enabled = false;
                }
            }

            yield return null;
        }

        _coroutineSmoothDisabling = null;
    }
}
