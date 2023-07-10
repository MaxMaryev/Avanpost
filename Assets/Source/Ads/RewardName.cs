using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class RewardName : MonoBehaviour, IActivable, IDeactivable
{
    [SerializeField] private Color _deactivateColor;

    private TMP_Text _name;
    private Color _activateColor;

    private void Awake()
    {
        _name = GetComponent<TMP_Text>();
        _activateColor = _name.color;
    }

    public void Activate()
    {
        _name.color = _activateColor;
    }

    public void Deactivate()
    {
        _name.color = _deactivateColor;
    }
}

