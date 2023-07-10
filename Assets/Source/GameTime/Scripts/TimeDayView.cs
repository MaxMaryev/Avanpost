using Lean.Localization;
using TMPro;
using UnityEngine;

public class TimeDayView : MonoBehaviour
{
    [SerializeField] private TMP_Text _dayNumber;
    [SerializeField] private TMP_Text _dayNumberLabel;
    [SerializeField] private DayCycleManager _dayCycleManager;

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;

        if (_dayCycleManager.CurrentTimeDay != null)
            OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
    }

    private void Start()
    {
        _dayNumberLabel.text = LeanLocalization.CurrentTranslations["Day"].Data.ToString();
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        _dayNumber.text = _dayCycleManager.CurrentDay.ToString();
    }
}
