using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipDayStage : HiglightingUIStage
{
    [SerializeField] private RewardSkipDay _rewardSkipDay;
    [SerializeField] private RewardSkipDayButton _skipDayButton;
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private List<string> _messages;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Vector3 _offsetHandClick;

    private Coroutine _coroutine;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    protected override void OnExit()
    {
        base.OnExit();

        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);

        _skipDayButton.Button.interactable = true;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay as Night || timeDay as LateNight)
            return;

        foreach (var message in _messages)
            _hintHandler.Add(message);

        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
        _coroutine = StartCoroutine(ShowDelay());
    }

    private IEnumerator ShowDelay()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => _hintHandler.IsAvailable == false);

        _rewardSkipDay.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        _skipDayButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();

        base.OnEnter();

        _handClickView.transform.SetParent(_skipDayButton.transform, false);
        _uIOutline.transform.SetParent(_skipDayButton.transform, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetHandClick;
        _uIOutline.gameObject.SetActive(true);

        _skipDayButton.Button.interactable = false;

        yield return new WaitForSeconds(3f);

        Completed?.Invoke(this);
    }
}
