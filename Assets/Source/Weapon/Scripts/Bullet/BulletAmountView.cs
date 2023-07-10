using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class BulletAmountView : MonoBehaviour
{
    [SerializeField] private FiringWeapon _firingWeapon;
    [SerializeField] private bool _fillIn;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _alarm;

    private float _defaultScale = 1;
    private float _targetScale = 1.3f;

    private void OnEnable()
    {
        _firingWeapon.BulletChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _firingWeapon.BulletChanged -= OnValueChanged;
    }

    private void Start()
    {
        _fill.fillAmount = _fillIn ? 0 : 1;
        OnValueChanged(_firingWeapon.ClipCapacity, _firingWeapon.CurrentBulletsCount);
    }

    private void OnValueChanged(float max, float current)
    {
        _fill.fillAmount = Mathf.Lerp(0, 1, current / max);

        if (_fill.fillAmount <= 0)
            StartCoroutine(Alarm());
    }

    private IEnumerator Alarm()
    {
        _alarm.gameObject.SetActive(true);

        while (_fill.fillAmount <= 0)
        {
            if (_alarm.transform.localScale.x <= _defaultScale)
                _alarm.transform.DOScale(_targetScale, 0.5f);
            else if (_alarm.transform.localScale.x >= _targetScale)
                _alarm.transform.DOScale(_defaultScale, 0.5f);

            yield return null;
        }

        _alarm.gameObject.SetActive(false);
    }
}
