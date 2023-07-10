using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteBlinking : MonoBehaviour
{
    [SerializeField] private float _minAlpha;
    [SerializeField] private float _maxAlpha;
    [SerializeField] private float _speedChangeAlpha;
    [SerializeField] private Image _image;

    private float _currentAlpha;
    private float _targetAlpha;

    private void OnEnable()
    {
        _targetAlpha = _minAlpha;
    }

    private void Update()
    {
        if (_currentAlpha == _minAlpha)
            _targetAlpha = _maxAlpha;
        else if (_currentAlpha == _maxAlpha)
            _targetAlpha = _minAlpha;

        _currentAlpha = Mathf.MoveTowards(_image.color.a, _targetAlpha, Time.deltaTime * _speedChangeAlpha);
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _currentAlpha);
    }
}
