using Agava.YandexMetrica;
using GameAnalyticsSDK;
using UnityEngine;

public class PlayerDeathEvent : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private DayCycleManager _dayCycleManager;

    private void Start()
    {
        _player.Health.UnderZero += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        _player.Health.UnderZero -= OnPlayerDeath;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, $"day-{_dayCycleManager.CurrentDay}");
        YandexMetrica.Send($"fail-day-{_dayCycleManager.CurrentDay}");
    }
}
