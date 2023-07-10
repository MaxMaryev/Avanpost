using System;
using UnityEngine;
using UnityEngine.UI;

public class SunMovement : MonoBehaviour
{
    [SerializeField] private Image _sun;
    [SerializeField] private RawImage _dayBar;
    [SerializeField] private float _speedRotate;
    [SerializeField] private DayCycleManager _cycleManager;
    [SerializeField] private RectTransform _rectTimeDayBar;
    [SerializeField] private RectTransform _sunRectTransform;
    [SerializeField] private float _normalizedOffsetSun;

    private float _speed;
    private float _halfWidthSun;

    private void OnEnable()
    {
        _cycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    private void OnDisable()
    {
        _cycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    private void Start()
    {
        _halfWidthSun = _rectTimeDayBar.rect.width / 2;
        UpdatePosition();
    }

    private void Update()
    {
        _sun.transform.rotation *= Quaternion.Euler(0, 0, -_speedRotate * Time.deltaTime);
        _sunRectTransform.anchoredPosition = new Vector2(Mathf.MoveTowards(_sunRectTransform.anchoredPosition.x, _halfWidthSun, _speed * _cycleManager.TimeSpeedMultiplier * Time.deltaTime), 0);

        if (_sunRectTransform.anchoredPosition.x >= _halfWidthSun)
            _sunRectTransform.anchoredPosition = new Vector2(-_halfWidthSun, 0);
    }

    private void OnTimeDayChanged(TimeDay timeDay) => UpdatePosition();

    private void UpdatePosition()
    {
        _speed = _rectTimeDayBar.rect.width / _cycleManager.TimeSecondsInFullDay;
        _sun.rectTransform.anchoredPosition = new Vector2(-_halfWidthSun + _speed * (_cycleManager.CurrentNormalizedTime + _normalizedOffsetSun) * _cycleManager.TimeSecondsInFullDay, 0);
    }
}