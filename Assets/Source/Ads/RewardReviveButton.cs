using Lean.Localization;
using TMPro;
using UnityEngine;

public class RewardReviveButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _info;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["Revive"].Data.ToString();
        _info.text = LeanLocalization.CurrentTranslations["ReviveInfo"].Data.ToString();
    }
}
