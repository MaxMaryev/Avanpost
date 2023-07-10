using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CraftSubMenuButton : MonoBehaviour, IPointerClickHandler
{
    public Toggle Toggle { get; private set; }

    public event Action<CraftSubMenuButton> Clicked;

    [field: SerializeField] public CraftPanelItems CraftPanelItems { get; private set; }

    private void Awake() => Toggle = GetComponent<Toggle>();

    public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(this);
}
