using TMPro;
using UnityEngine;

public class CritChanceUpgradeButton : ButtonUI, ITextMeshProHandler
{
    [field: SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; private set; }
}
