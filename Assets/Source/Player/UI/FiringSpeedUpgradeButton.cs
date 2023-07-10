using TMPro;
using UnityEngine;

public class FiringSpeedUpgradeButton : ButtonUI, ITextMeshProHandler
{
    [field: SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; private set; }
}
