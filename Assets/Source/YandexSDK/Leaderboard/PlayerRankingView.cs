using UnityEngine;
using TMPro;

public class PlayerRankingView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _dayDigitLeft;
    [SerializeField] private TextMeshProUGUI _dayDigitRight;
    [SerializeField] private TextMeshProUGUI _hourDigitLeft;
    [SerializeField] private TextMeshProUGUI _hourDigitRight;

    public void Initialize(string rank, string playerName, int lifeTimeInHours)
    {
        _playerName.text = rank + ". " + playerName;
        SetScore(lifeTimeInHours);
    }

    private void SetScore(int lifeTimeInHours)
    {
        int hours = (lifeTimeInHours % 24);
        int days = (lifeTimeInHours - hours) / 24;

        string hoursDigits = hours.ToString();
        string daysDigits = days.ToString();

        if (hoursDigits.Length == 1)
            _hourDigitLeft.text = "0";
        else
            _hourDigitLeft.text = hoursDigits[0].ToString();

        if (daysDigits.Length == 1)
            _dayDigitLeft.text = "0";
        else
            _dayDigitLeft.text = daysDigits[0].ToString();

        _hourDigitRight.text = hoursDigits[hoursDigits.Length - 1].ToString();
        _dayDigitRight.text = daysDigits[daysDigits.Length - 1].ToString();
    }
}
