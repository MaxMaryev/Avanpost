using Lean.Localization;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private TMP_Text _message;
    [SerializeField] private float _timeScaleChange;
    [SerializeField] private float _timeShow;

    private float _timeSpeedMupltiplier = 1;
    private Coroutine _coroutine;
    private Vector3 _startScale = Vector3.zero;

    public event Action<Hint> Showed;

    private void Awake()
    {
        _startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void Init(string message)
    {
        _message.text = LeanLocalization.CurrentTranslations[message].Data.ToString();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        float time = 0;
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = _startScale;

        while (time < _timeScaleChange)
        {
            time += Time.unscaledDeltaTime * _timeSpeedMupltiplier;
            transform.localScale = Vector3.Lerp(currentScale, targetScale, time / _timeScaleChange);
            yield return null;
        }

        _coroutine = StartCoroutine(HideDelay());
    }

    private IEnumerator HideDelay()
    {
        yield return new WaitForSecondsRealtime(_timeShow);

        _coroutine = StartCoroutine(Hide());
    }

    public void SkipShow()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        float time = 0;
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (time < _timeScaleChange)
        {
            time += Time.unscaledDeltaTime * _timeSpeedMupltiplier;
            transform.localScale = Vector3.Lerp(currentScale, targetScale, time / _timeScaleChange);
            yield return null;
        }

        Showed?.Invoke(this);
    }

    public void SetTimeSpeedMultiplier(float speed) => _timeSpeedMupltiplier = speed;
}
