using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MachineGunStage : HiglightingUIStage
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private LateNight _lateNight;
    [SerializeField] private string _message;
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private RewardMinigun _rewardMinigun;
    [SerializeField] private RewardMinigunButton _rewardMinigunButton;
    [SerializeField] private Image _movieIcon;
    [SerializeField] private Player _player;
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private TimeScaleHandler _timeScaleHandler;

    private Coroutine _coroutineSlow;
    private Coroutine _coroutineComplete;
    private float _currentTargetTimeScale;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _timeScaleHandler.Changed += OnTimeScaleChanged;
        _hintHandler.Add(_message);
        _coroutineSlow = StartCoroutine(Slow(0.1f));
        StartCoroutine(WaitHint());
    }

    private IEnumerator WaitHint()
    {
        yield return new WaitUntil(() => _hintHandler.IsAvailable == false);

        _rewardMinigun.gameObject.SetActive(true);
        _rewardMinigun.GotReward += OnGotReward;
        yield return new WaitForEndOfFrame();
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        _movieIcon.enabled = false;
        _rewardMinigunButton.gameObject.SetActive(true);
        _player.Health.UnderZero += OnPlayerDeath;
    }

    private void OnGotReward(WeaponType weaponType, Sprite sprite, float duration)
    {
        _rewardMinigun.GotReward -= OnGotReward;
        _uIOutline.gameObject.SetActive(false);
        _movieIcon.enabled = true;

        if (_coroutineSlow != null)
            StopCoroutine(_coroutineSlow);

        _hintHandler.SkipCurrentHint();
        _coroutineSlow = StartCoroutine(Slow(1));
        _coroutineComplete = StartCoroutine(CompleteDelay());
    }

    protected override void OnExit()
    {
        base.OnExit();

        _timeScaleHandler.Changed -= OnTimeScaleChanged;

        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);

        _player.Health.UnderZero -= OnPlayerDeath;

        _dayCycleManager.SkipDayBeforeTo(_lateNight);

        if (_coroutineComplete != null)
            StopCoroutine(_coroutineComplete);
    }

    private void OnTimeScaleChanged(float timeScale)
    {
        if (timeScale > 0)
            Time.timeScale = _currentTargetTimeScale;
    }

    private IEnumerator Slow(float targetValue)
    {
        _currentTargetTimeScale = targetValue;
        float currentValue = Time.timeScale;
        float time = 0;
        float timeToTargetSlow = 1;

        while (time < timeToTargetSlow && _player.IsAlive)
        {
            time += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(currentValue, _currentTargetTimeScale, time / timeToTargetSlow);
            yield return null;
        }

        if (_player.IsAlive == false)
            Time.timeScale = 1;

        _coroutineSlow = null;
    }

    private void OnPlayerDeath()
    {
        _player.Health.UnderZero -= OnPlayerDeath;

        if (_coroutineComplete != null)
            StopCoroutine(_coroutineComplete);

        if (_coroutineSlow != null)
            StopCoroutine(_coroutineSlow);

        base.OnExit();

        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    private IEnumerator CompleteDelay()
    {
        base.OnExit();
        yield return new WaitUntil(() => _coroutineSlow == null);
        Completed?.Invoke(this);
    }
}
