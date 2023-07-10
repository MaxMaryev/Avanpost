using Agava.WebUtility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class RewardAdvertising : MonoBehaviour
{
    protected const string FirstRewardPassed = "FirstRewardPassed";
    protected const int True = 1;

    [SerializeField] private ButtonUI _rewardButton;
    [SerializeField] private TimeScaleHandler _timeScaleHandler;
    [SerializeField] private float _cooldown;
    [SerializeField] private Image _movieIcon;
    [SerializeField] private Image _panelGun;
    [SerializeField] private Sprite _activePanel;
    [SerializeField] private Sprite _deactivPanel;
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private string _adblockMessage;

    private List<IActivable> _activables = new List<IActivable>();
    private List<IDeactivable> _deactivables = new List<IDeactivable>();

    [field: SerializeField] public DayCycleManager DayCycleManager { get; private set; }
    public float Cooldown => _cooldown;
    public bool IsAdsPlaying { get; private set; }
    public bool IsShowed => _rewardButton.gameObject.activeSelf;

    public event Action RewardClicked;
    public event Action<RewardAdvertising> ButtonShowed;
    public event Action ButtonDeactevated;

    protected virtual void Awake()
    {
        _activables.AddRange(_rewardButton.GetComponentsInChildren<IActivable>());
        _deactivables.AddRange(_rewardButton.GetComponentsInChildren<IDeactivable>());
    }

    protected virtual void OnEnable() => _rewardButton.Clicked += OnRewardButtonClicked;

    protected virtual void OnDisable() => _rewardButton.Clicked -= OnRewardButtonClicked;

    protected virtual void Start() => ShowButton();

    protected virtual void ShowButton()
    {
        if (_rewardButton.gameObject.activeSelf == false)
        {
            _rewardButton.gameObject.SetActive(true);
            ButtonShowed?.Invoke(this);
        }
    }

    protected abstract void OnReward();

    protected virtual void HideButton()
    {
        _rewardButton.gameObject.SetActive(false);
    }

    protected virtual void ActivateButton()
    {
        _panelGun.sprite = _activePanel;
        _rewardButton.Button.interactable = true;
        _movieIcon.enabled = true;

        if (_rewardButton.gameObject.activeSelf)
            ButtonShowed?.Invoke(this);

        foreach (var activable in _activables)
            activable.Activate();
    }

    protected virtual void DeactivateButton()
    {
        foreach (var deactivable in _deactivables)
            deactivable.Deactivate();

        _panelGun.sprite = _deactivPanel;
        _movieIcon.enabled = false;
        _rewardButton.Button.interactable = false;
        ButtonDeactevated?.Invoke();
    }

    protected virtual void OnRewardButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (AdBlock.Enabled)
        {
            _hintHandler.Add(_adblockMessage);
            return;
        }
            Agava.YandexGames.VideoAd.Show(() => OnOpenCallback(), onCloseCallback: () => OnCloseCallback());
#else
        OnCloseCallback();
#endif
        RewardClicked?.Invoke();


        void OnOpenCallback()
        {
            IsAdsPlaying = true;
            _timeScaleHandler.FreezeTime();
        }

        void OnCloseCallback()
        {
            IsAdsPlaying = false;
            _timeScaleHandler.UnFreezeTime();
            OnReward();
        }

        DeactivateButton();
    }
}
