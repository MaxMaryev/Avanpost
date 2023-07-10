using UnityEngine;
using UnityEngine.UI;

public class RepairBar : MonoBehaviour
{
    [SerializeField] private bool _fillIn;
    [SerializeField] protected Image _fill;
    [SerializeField] private Building _building;

    private void OnEnable()
    { 
        _building.SafetyMarginChanged += OnValueChanged;
        OnValueChanged(_building.MaxSafetyMargin, _building.CurrentSafetyMargin);
    }

    private void Start() => OnValueChanged(_building.MaxSafetyMargin, _building.CurrentSafetyMargin);


    private void OnDisable() => _building.SafetyMarginChanged -= OnValueChanged;

    private void OnValueChanged(float max, float current, Building building = null)
    {
        if (_fillIn)
            _fill.fillAmount = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, max, current));
        else
            _fill.fillAmount = Mathf.Lerp(0, 1, Mathf.InverseLerp(0, max, current));
    }
}
