using Lean.Localization;
using TMPro;
using UnityEngine;

public class RewardLootButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["Increase"].Data.ToString() + "\n" + LeanLocalization.CurrentTranslations["loot"].Data.ToString();
    }
}
