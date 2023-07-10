using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardView : MonoBehaviour
{
    [SerializeField] private Button _authorizeButton;
    [SerializeField] private TMP_Text _authorizeMessage;
    [SerializeField] private TMP_Text _authorizeButtonText;
    [SerializeField] private TMP_Text _scoreLabel;
    [SerializeField] private TMP_Text _daysText;
    [SerializeField] private TMP_Text _hoursText;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private ScrollRect _scroll;

    private void Start()
    {
        _scoreLabel.text = LeanLocalization.CurrentTranslations["ScoreLabel"].Data.ToString();
        _daysText.text = LeanLocalization.CurrentTranslations["Days"].Data.ToString();
        _hoursText.text = LeanLocalization.CurrentTranslations["Hours"].Data.ToString();
        _name.text = LeanLocalization.CurrentTranslations["Name"].Data.ToString();
    }

    public void ShowAuthorizeOffer()
    {
        _authorizeMessage.text = LeanLocalization.CurrentTranslations["AuthorizeMessage"].Data.ToString();
        _authorizeButtonText.text = LeanLocalization.CurrentTranslations["Authorize"].Data.ToString();
        _authorizeMessage.gameObject.SetActive(true);
        _authorizeButton.gameObject.SetActive(true);
    }

    public void  HideAuthorizeOffer()
    {
        _authorizeMessage.gameObject.SetActive(false);
        _authorizeButton.gameObject.SetActive(false);
    }

    public void ShowSlider() => _scroll.gameObject.SetActive(true);
}
