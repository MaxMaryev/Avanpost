using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponPanelSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _icon;

    [field: SerializeField] public WeaponType WeaponType { get; private set; }
    [field: SerializeField] public Toggle Toggle { get; private set; }

    public bool IsReward { get; private set; }

    public event Action<WeaponType, bool> Selected;

    public void OnPointerDown(PointerEventData eventData) => Selected?.Invoke(WeaponType, IsReward);

    public void Init(Sprite icon, WeaponType weaponType, bool isReward = false)
    {
        _icon.sprite = icon;
        WeaponType = weaponType;
        IsReward = isReward;
    }

    public void Select()
    {
        Selected?.Invoke(WeaponType, IsReward);
        Toggle.isOn = true;
    }

    public void Delete() => WeaponType = WeaponType.None;
}