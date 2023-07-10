using Lean.Localization;
using TMPro;
using UnityEngine;

public class RewardMinigunButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["Get"].Data.ToString() + "\n" + LeanLocalization.CurrentTranslations["Minigun"].Data.ToString().ToLower();
    }
}
