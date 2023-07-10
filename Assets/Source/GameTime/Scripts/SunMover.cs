using System;
using UnityEngine;
using UnityEngine.UI;

public class SunMover : MonoBehaviour
{
    [SerializeField] private Image _sun;
    [SerializeField] private DayCycleManager _cycleManager;
    [SerializeField] private AnimationCurve _animationCurveY;
    [SerializeField] private AnimationCurve _animationCurveX;
    [SerializeField] private TimeDay _startTimeDaySun;
    [SerializeField] private TimeDay _endTimeDaySun;
    [SerializeField] private float _speedRotate;

    private bool _isSunActive => _sun.gameObject.activeSelf;
    private float _maxNormalizedTime;
    private float _currentNormalizedTime;

    private void Start()
    {
        _maxNormalizedTime = _endTimeDaySun.StartNormalizedTime - _startTimeDaySun.StartNormalizedTime;
    }

    private void OnEnable()
    {
        _cycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    private void OnDisable()
    {
        _cycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (_cycleManager.CurrentNormalizedTime >= _startTimeDaySun.StartNormalizedTime && _cycleManager.CurrentNormalizedTime <= _endTimeDaySun.StartNormalizedTime)
            _sun.gameObject.SetActive(true);
        else
            _sun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isSunActive)
        {
            _sun.transform.rotation *= Quaternion.Euler(0, 0, -_speedRotate / _cycleManager.TimeSecondsInFullDay * Time.deltaTime);
            _currentNormalizedTime = _maxNormalizedTime - (_endTimeDaySun.StartNormalizedTime - _cycleManager.CurrentNormalizedTime);
            _sun.rectTransform.anchoredPosition = new Vector2(_animationCurveX.Evaluate(_currentNormalizedTime / _maxNormalizedTime), _animationCurveY.Evaluate(_currentNormalizedTime / _maxNormalizedTime));
        }
    }
}