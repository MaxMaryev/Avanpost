using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Bar : MonoBehaviour
{
    [SerializeField] private bool _fillIn;
    [SerializeField] private Image _fill;
    [SerializeField] private float _speed;

    private void Start()
    {
        _fill.fillAmount = _fillIn ? 0 : 1;
    }
    
    protected virtual void OnValueChanged(float max, float current)
    {
        if (_fillIn)
            _fill.DOFillAmount(Mathf.Lerp(1, 0, Mathf.InverseLerp(0, max, current)), _speed);
        else
            _fill.DOFillAmount(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, max, current)), _speed);
    }
}