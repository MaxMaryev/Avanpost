using Agava.YandexMetrica;
using GameAnalyticsSDK;
using UnityEngine;

public class DayCompleteEvent : MonoBehaviour
{
    [SerializeField] private DayCycleManager _dayCycleManager;

    private int _lastValueDay = 0;

    private void OnEnable()
    {
        _dayCycleManager.DayChanged += OnDayChanged;
    }

    private void OnDisable()
    {
        _dayCycleManager.DayChanged -= OnDayChanged;
    }

    private void Start()
    {
        OnDayChanged(_dayCycleManager.CurrentDay);
    }

    private void OnDayChanged(int value)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (_lastValueDay > 0)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"day-{value - 1}");
            YandexMetrica.Send($"complete-day-{value - 1}");
        }

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, $"day-{value}");
        YandexMetrica.Send($"start-day-{value}");
        _lastValueDay = value;
    }
}
