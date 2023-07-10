using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintHandler : MonoBehaviour
{
    [SerializeField] private List<Workbench> _workbenches;
    [SerializeField] private Hint _hint;

    private Queue<string> _messages = new Queue<string>();
    private Coroutine _coroutine;
    private bool _isFreeze;

    public bool IsAvailable => _hint.gameObject.activeSelf || _messages.Count > 0;

    private void OnCraftMenuClosed() => Freeze(false);

    private void OnCraftMenuOpened() => Freeze(true);

    private void Freeze(bool state)
    {
        _isFreeze = state;
        _hint.SetTimeSpeedMultiplier(state == true ? 0 : 1);
    }

    public void Add(string message)
    {
        if (_messages.Contains(message))
            return;

        _messages.Enqueue(message);

        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(UpdateHint());
        }
    }

    private IEnumerator UpdateHint()
    {
        while (_messages.Count > 0)
        {
            yield return new WaitUntil(() => _hint.gameObject.activeSelf == false);
            _hint.gameObject.SetActive(true);
            _hint.Init(_messages.Dequeue());
            _hint.Showed += OnHintShowed;
            yield return new WaitForSecondsRealtime(2f);
            yield return null;
        }

        _coroutine = null;
    }

    private void OnHintShowed(Hint hint)
    {
        _hint.Showed -= OnHintShowed;
        _hint.gameObject.SetActive(false);
    }

    public void SkipCurrentHint()
    {
        if (_hint.gameObject.activeSelf)
        {
            _hint.SkipShow();
        }
    }

    public void SkipAllHitns()
    {
        SkipCurrentHint();

        _messages.Clear();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = null;
    }
}
