using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void Render(int value)
    {
        _text.text = $"{Mathf.RoundToInt(value)}";
    }
}
