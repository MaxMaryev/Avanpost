using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RewardTimeIcon : MonoBehaviour, IActivable, IDeactivable
{
    [SerializeField] private Color _deactivateColor;

    private Image _icon;
    private Color _activateColor;

    private void Awake()
    {
        _icon = GetComponent<Image>();
        _activateColor = _icon.color;
    }

    public void Activate()
    {
        _icon.color = _activateColor;
    }

    public void Deactivate()
    {
        _icon.color = _deactivateColor;
    }
}
