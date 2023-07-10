using Lean.Localization;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _itemNameTMPro;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Color _lockColor;
    [SerializeField] private Color _unlockColor;

    private WalletPresenter _walletPresenter;

    public event Action<CraftItemButton> Selected;
    public event Action Clicked;

    public ICraftableItem CraftableItem { get; private set; }

    private void OnEnable() => StartCoroutine(SelectToggledOnStart());

    public void OnPointerDown(PointerEventData eventData)
    {
        Selected?.Invoke(this);
        Clicked?.Invoke();
    }

    public void Init(ICraftableItem craftableItem, ToggleGroup toggleGroup, WalletPresenter walletPresenter)
    {
        CraftableItem = craftableItem;
        _image.sprite = craftableItem.Icon;
        _itemNameTMPro.text = LeanLocalization.CurrentTranslations[craftableItem.Name.ToString()].Data.ToString();
        _toggle.group = toggleGroup;
        _price.text = craftableItem.JunkNeeded.ToString();
        _walletPresenter = walletPresenter;
    }

    public void UpdateAvailabilityView()
    {
        if (_walletPresenter.Value < CraftableItem.JunkNeeded)
        {
            _price.color = _lockColor;
        }
        else
        {
            _price.color = _unlockColor;
        }
    }

    public void SetToggle(bool state) => _toggle.isOn = state;

    private IEnumerator SelectToggledOnStart()
    {
        yield return new WaitUntil(() => CraftableItem != null);

        if (_toggle.isOn)
            Selected?.Invoke(this);
    }
}
