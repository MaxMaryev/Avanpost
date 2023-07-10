using Lean.Localization;
using TMPro;
using UnityEngine;

public class ResumeButton : ButtonUI
{
    [SerializeField] private TMP_Text _name;

    private void Start()
    {
        _name.text = LeanLocalization.CurrentTranslations["RESUME"].Data.ToString();
    }
}
