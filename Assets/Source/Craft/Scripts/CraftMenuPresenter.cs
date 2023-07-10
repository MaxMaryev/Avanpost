using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftMenuPresenter : MonoBehaviour
{
    [SerializeField] private CraftMenu _craftMenu;
    [SerializeField] private Workbench _workbench;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private ButtonCloseCraftMenu _buttonCloseCraft;
    [SerializeField] private WalletPresenter _walletPresenter;
    [SerializeField] private List<CraftPanelItems> _craftPanelItems = new List<CraftPanelItems>();
    [SerializeField] private CraftItemSelectedView _craftItemSelectedView;
    [SerializeField] private ButtonCraftItem _buttonCraftItem;

    private ICraftableItem _currentSelectedCraftItem;

    public event Action<ICraftableItem> Crafted;
    public event Action Exited;

    private void OnEnable()
    {
        _workbench.OpenedCraftMenu += OnOpenedCraftMenu;
        _buttonCloseCraft.Clicked += OnCloseCraftMenu;
        _buttonCraftItem.Clicked += OnButtonCraftClicked;
    }

    private void OnDisable()
    {
        _workbench.OpenedCraftMenu -= OnOpenedCraftMenu;
        _buttonCloseCraft.Clicked -= OnCloseCraftMenu;
        _buttonCraftItem.Clicked -= OnButtonCraftClicked;

        foreach (var panel in _craftPanelItems)
            panel.Selected -= OnButtonSelected;
    }

    private void Start()
    {
        foreach (var panel in _craftPanelItems)
        {
            panel.Fill();
            panel.Selected += OnButtonSelected;
        }
    }

    private void OnButtonSelected(CraftItemButton craftItemButton)
    {
        if (craftItemButton == null)
        {
            _craftItemSelectedView.ShowCraftItem(null, false);
            return;
        }

        _currentSelectedCraftItem = craftItemButton.CraftableItem;
        _craftItemSelectedView.ShowCraftItem(_currentSelectedCraftItem, _walletPresenter.Value >= craftItemButton.CraftableItem.JunkNeeded);
    }

    private void OnCloseCraftMenu()
    {
        _playerMover.enabled = true;
        _craftMenu.gameObject.SetActive(false);
        Exited?.Invoke();
    }

    private void OnOpenedCraftMenu()
    {
        _playerMover.enabled = false;
        _craftMenu.gameObject.SetActive(true);
        UpdateAvailabilityView();
    }

    private void OnButtonCraftClicked()
    {
        if (_currentSelectedCraftItem == null)
            return;

        if (_walletPresenter.Value >= _currentSelectedCraftItem.JunkNeeded)
        {
            _walletPresenter.SpendResource(_currentSelectedCraftItem.JunkNeeded);
            Crafted?.Invoke(_currentSelectedCraftItem);
        }

        UpdateAvailabilityView();
    }

    private void UpdateAvailabilityView()
    {
        foreach (var panelItems in _craftPanelItems)
            foreach (var itemButtons in panelItems.CraftItemButtons)
                itemButtons.UpdateAvailabilityView();
    }
}
