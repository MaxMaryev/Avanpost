using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmenuButton : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public Submenu Submenu { get; private set; }


    public event Action<SubmenuButton> Clicked;

    public Toggle Toggle { get; private set; }

    private void Awake() => Toggle = GetComponent<Toggle>();

    public virtual void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(this);
}
