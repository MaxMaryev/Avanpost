using System;
using System.Collections;
using UnityEngine;

public class DayAndNightControl : MonoBehaviour
{
    private const float DayBeginTime = 0.1f;
    private const float NightBeginTime = 0.6f;

    [SerializeField] private DayColors _dayColors;
    [SerializeField] private Light _directionalLight;
    [SerializeField] private float _secondsInAFullDay = 120f;

    private float _timeSpeedMultiplier;

    public event Action DayTimeChanged;

    [field: SerializeField] public float CurrentNormilizeTime { get; private set; }

    public int CurrentDay { get; private set; } = 1;
    public float SecondsInAFullDay => _secondsInAFullDay;

    public DayTime DayTime =>
        CurrentNormilizeTime < DayBeginTime || CurrentNormilizeTime >= NightBeginTime ?
        DayTime.Night : DayTime.Day;

    private void Start()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        StartCoroutine(TrackChangingDayTime());
        FreezeTime(false);
    }

    private void Update()
    {
        UpdateLight();
        CurrentNormilizeTime += Time.deltaTime * _timeSpeedMultiplier / _secondsInAFullDay;

        if (CurrentNormilizeTime >= 1)
        {
            CurrentNormilizeTime = 0;
            CurrentDay++;
        }
    }

    public void FreezeTime(bool state)
    {
        if (state)
            _timeSpeedMultiplier = 0;
        else
            _timeSpeedMultiplier = 1;
    }

    private void UpdateLight()
    {
        _directionalLight.transform.localRotation = Quaternion.Euler((CurrentNormilizeTime * 360f) - 90, 170, 0);
        float intensityMultiplier = 1;

        if (CurrentNormilizeTime <= 0.23f || CurrentNormilizeTime >= 0.75f)
            intensityMultiplier = 0;
        else if (CurrentNormilizeTime <= 0.25f)
            intensityMultiplier = Mathf.Clamp01((CurrentNormilizeTime - 0.23f) * (1 / 0.02f));
        else if (CurrentNormilizeTime <= 0.73f)
            intensityMultiplier = Mathf.Clamp01(1 - ((CurrentNormilizeTime - 0.73f) * (1 / 0.02f)));

        if (CurrentNormilizeTime <= 0.2f)
        {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, _dayColors.GetColors(DayTime.Night).SkyColor, Time.deltaTime);
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, _dayColors.GetColors(DayTime.Night).EquatorColor, Time.deltaTime);
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, _dayColors.GetColors(DayTime.Night).HorizonColor, Time.deltaTime);
        }
        if (CurrentNormilizeTime > 0.2f && CurrentNormilizeTime < 0.4f)
        {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, _dayColors.GetColors(DayTime.Dawn).SkyColor, Time.deltaTime);
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, _dayColors.GetColors(DayTime.Dawn).EquatorColor, Time.deltaTime);
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, _dayColors.GetColors(DayTime.Dawn).HorizonColor, Time.deltaTime);
        }
        if (CurrentNormilizeTime > 0.4f && CurrentNormilizeTime < 0.75f)
        {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, _dayColors.GetColors(DayTime.Day).SkyColor, Time.deltaTime);
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, _dayColors.GetColors(DayTime.Day).EquatorColor, Time.deltaTime);
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, _dayColors.GetColors(DayTime.Day).HorizonColor, Time.deltaTime);
        }
        if (CurrentNormilizeTime > 0.75f)
        {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, _dayColors.GetColors(DayTime.Day).SkyColor, Time.deltaTime);
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, _dayColors.GetColors(DayTime.Day).EquatorColor, Time.deltaTime);
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, _dayColors.GetColors(DayTime.Day).HorizonColor, Time.deltaTime);
        }

        _directionalLight.intensity *= intensityMultiplier;
    }

    private IEnumerator TrackChangingDayTime()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        DayTime lastDayTime;

        while (CurrentDay < 1000)
        {
            lastDayTime = DayTime;
            yield return waitForSeconds;

            if (DayTime != lastDayTime)
                DayTimeChanged?.Invoke();
        }
    }
}

public enum DayTime
{
    Dawn,
    Day,
    Night
}
