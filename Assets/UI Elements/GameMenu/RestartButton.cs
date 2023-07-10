using Lean.Localization;
using TMPro;
using UnityEngine;

public class RestartButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _info;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["RESTART"].Data.ToString();

        if (_info != null)
            _info.text = LeanLocalization.CurrentTranslations["RestartInfo"].Data.ToString();
    }
}
