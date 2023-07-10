using Lean.Localization;
using TMPro;
using UnityEngine;

public class LeanLocalizationTMPPro : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _message;

    void Start()
    {
        _text.text = LeanLocalization.CurrentTranslations[_message].Data.ToString();
    }
}
