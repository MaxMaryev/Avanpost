using Lean.Localization;
using TMPro;
using UnityEngine;

public class RewardReloadButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _info;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["Reload"].Data.ToString();
        _info.text = LeanLocalization.CurrentTranslations["Instantly"].Data.ToString();
    }
}
