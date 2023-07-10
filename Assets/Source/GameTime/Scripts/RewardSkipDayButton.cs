using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSkipDayButton : ButtonUI
{
    [SerializeField] private Button _skipDayButton;
    [SerializeField] private TMP_Text _textMeshPro;

    private void Start()
    {
        _textMeshPro.text = LeanLocalization.CurrentTranslations["Skip Day"].Data.ToString();
    }
}
