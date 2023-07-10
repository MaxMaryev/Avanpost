using Agava.YandexMetrica;
using GameAnalyticsSDK;
using UnityEngine;

public abstract class RewardEvent : MonoBehaviour
{
    [SerializeField] private RewardAdvertising _rewardAdvertising;

    private void OnEnable()
    {
        _rewardAdvertising.ButtonShowed += OnRewardShowed;
        _rewardAdvertising.RewardClicked += OnRewardClicked;
    }

    private void OnDisable()
    {
        _rewardAdvertising.ButtonShowed -= OnRewardShowed;
        _rewardAdvertising.RewardClicked -= OnRewardClicked;
    }

    private void OnRewardClicked()
    {
        Debug.Log($"{GetTypeReward()}-ad-click");
        GameAnalytics.NewDesignEvent($"{GetTypeReward()}-ad-click");
        YandexMetrica.Send($"{GetTypeReward()}-ad-click");
    }

    private void OnRewardShowed(RewardAdvertising rewardAdvertising)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        Debug.Log($"{GetTypeReward()}-ad-offer");
        GameAnalytics.NewDesignEvent($"{GetTypeReward()}-ad-offer");
        YandexMetrica.Send($"{GetTypeReward()}-ad-offer");
    }

    protected abstract string GetTypeReward();
}
