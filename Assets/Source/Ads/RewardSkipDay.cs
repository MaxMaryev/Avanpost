using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardSkipDay : RewardAdvertising
{
    [SerializeField] private Night _tOTimeDay;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private Image _background;
    [SerializeField] private Confirmer _confirmer;
    [SerializeField] private PlayerMover _playerMover;

    private Coroutine _delayedClickCoroutine;
    private Color _startColor;
    private float _timeToSkipDay = 3f;

    protected override void Awake()
    {
        base.Awake();
        _startColor = _background.color;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    protected override void Start() => OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);

    protected override void OnReward() => _delayedClickCoroutine = StartCoroutine(RewardDelay());

    protected override void OnRewardButtonClicked() => _confirmer.AskConfirmation(() => base.OnRewardButtonClicked(), null);

    private IEnumerator RewardDelay()
    {
        _playerMover.enabled = false;
        _background.gameObject.SetActive(true);
        float time = 0;
        Color targetColor = new Color(_startColor.r, _startColor.g, _startColor.b, 1);

        while (time < _timeToSkipDay / 2)
        {
            time += Time.deltaTime;
            _background.color = Color.Lerp(_startColor, targetColor, time / (_timeToSkipDay / 2));
            yield return null;
        }

        _dayCycleManager.SkipDayBeforeTo(_tOTimeDay);
        time = 0;

        while (time < _timeToSkipDay / 2)
        {
            time += Time.deltaTime;
            _background.color = Color.Lerp(targetColor, _startColor, time / (_timeToSkipDay / 2));
            yield return null;
        }

        _playerMover.enabled = true;
        _background.gameObject.SetActive(false);
        _playerMover.enabled = true;
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay is Day || timeDay is Dawn || timeDay is Evening)
        {
            if (IsShowed == false)
            {
                ActivateButton();
                ShowButton();
            }
        }
        else
        {
            HideButton();
        }
    }
}
