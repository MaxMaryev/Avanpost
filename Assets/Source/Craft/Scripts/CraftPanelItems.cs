using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CraftPanelItems : MonoBehaviour
{
    [SerializeField] private CraftItemButton _template;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private WalletPresenter _walletPresenter;

    private List<CraftItemButton> _craftItemButtons = new List<CraftItemButton>();

    public event Action<CraftItemButton> Selected;

    public IReadOnlyList<CraftItemButton> CraftItemButtons => _craftItemButtons;

    private void OnEnable()
    {
        foreach (var button in _craftItemButtons)
            button.Selected += OnButtonSelected;

        SelectFirstButton();
    }

    private void OnDisable()
    {
        foreach (var button in _craftItemButtons)
        {
            button.SetToggle(false);
            button.Selected -= OnButtonSelected;
        }
    }

    public abstract void Fill();

    protected CraftItemButton CreateButton(ICraftableItem craftableItem)
    {
        CraftItemButton craftItemButton = Instantiate(_template, transform);
        craftItemButton.Init(craftableItem, _toggleGroup, _walletPresenter);
        _craftItemButtons.Add(craftItemButton);
        return craftItemButton;
    }

    public void RemoveButton(ICraftableItem craftableItem)
    {
        CraftItemButton craftItemButton = null;

        foreach (var button in _craftItemButtons)
            if (button.CraftableItem == craftableItem)
                craftItemButton = button;

        if (craftItemButton != null)
        {
            _craftItemButtons.Remove(craftItemButton);
            Destroy(craftItemButton.gameObject);
        }

        SelectFirstButton();
    }

    private void SelectFirstButton()
    {
        if (_craftItemButtons.Count > 0)
            _craftItemButtons[0].SetToggle(true);

        Selected?.Invoke(_craftItemButtons.Count > 0 ? _craftItemButtons[0] : null);
    }

    private void OnButtonSelected(CraftItemButton button) => Selected?.Invoke(button);
}
