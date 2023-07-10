using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System;
using UnityEngine;

public class TutorialStageEvent : MonoBehaviour
{
    [SerializeField] private Tutorial _tutorial;

    private bool _isFirstChange = false;
    private bool _isFirstPanelShow;

    private void Awake()
    {
        _isFirstPanelShow = PlayerPrefs.GetInt(TutorialSaver.KeySkipTutorialPanel, 0) == 0;
    }

    private void OnEnable()
    {
        _tutorial.StageChanged += OnTutorialStageChanged;
        _tutorial.SkipTutorialPanelShowed += OnPanelShowed;
        _tutorial.SkipTutorialPanelClosed += OnPanelClosed;
    }

    private void OnDisable()
    {
        _tutorial.StageChanged -= OnTutorialStageChanged;
        _tutorial.SkipTutorialPanelShowed -= OnPanelShowed;
        _tutorial.SkipTutorialPanelClosed -= OnPanelClosed;
    }

    private void OnPanelClosed()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (_isFirstPanelShow)
            YandexMetrica.Send($"skip-tutorial-ad-click");
    }

    private void OnPanelShowed()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (_isFirstPanelShow)
            YandexMetrica.Send($"skip-tutorial-ad-offer");
    }

    private void OnTutorialStageChanged(TutorialStage stage)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (_isFirstChange)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"tutorialStage-{_tutorial.CurrentIndexStage - 1}");
            YandexMetrica.Send($"complete-tutorialStage-{_tutorial.CurrentIndexStage - 1}");
        }

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, $"tutorialStage-{_tutorial.CurrentIndexStage}");
        YandexMetrica.Send($"start-tutorialStage-{_tutorial.CurrentIndexStage}");
        _isFirstChange = true;
    }
}
