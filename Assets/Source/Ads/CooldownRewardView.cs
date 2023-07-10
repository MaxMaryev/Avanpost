using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CooldownRewardView : MonoBehaviour
{
    [SerializeField] private RewardAdvertising _rewardAdvertising;
    [SerializeField] private Image _view;

    private Coroutine _coroutineHideView;

    private void OnEnable()
    {
        _rewardAdvertising.ButtonDeactevated += OnRewardDeactivated;
    }

    private void OnDisable()
    {
        _rewardAdvertising.ButtonDeactevated -= OnRewardDeactivated;
    }

    private void OnRewardDeactivated()
    {
        if (_coroutineHideView == null)
            _coroutineHideView = StartCoroutine(HideView());
    }

    private IEnumerator HideView()
    {
        float time = 0;

        while (time <= _rewardAdvertising.Cooldown)
        {
            time += Time.deltaTime;
            _view.fillAmount = Mathf.Lerp(1, 0, time / _rewardAdvertising.Cooldown);
            yield return null;
        }

        _coroutineHideView = null;
    }
}
