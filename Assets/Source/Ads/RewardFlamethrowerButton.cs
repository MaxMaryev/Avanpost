using Lean.Localization;
using TMPro;
using UnityEngine;

public class RewardFlamethrowerButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["Get"].Data.ToString() + "\n" + LeanLocalization.CurrentTranslations["Flamethrower"].Data.ToString().ToLower();
    }
}
