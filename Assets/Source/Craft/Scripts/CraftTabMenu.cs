using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftTabMenu : MonoBehaviour
{
    private List<CraftSubMenuButton> _craftSubMenuButtons = new List<CraftSubMenuButton>();

    private void Awake()
    {
        _craftSubMenuButtons.AddRange(GetComponentsInChildren<CraftSubMenuButton>());
    }

    private void OnEnable()
    {
        foreach (var submenu in _craftSubMenuButtons)
            submenu.Clicked += OnSubMenuClicked;
    }

    private void OnDisable()
    {
        foreach (var submenu in _craftSubMenuButtons)
            submenu.Clicked -= OnSubMenuClicked;
    }

    private void OnSubMenuClicked(CraftSubMenuButton craftSubMenuButton)
    {
        foreach (var submenu in _craftSubMenuButtons)
        {
            if (submenu != craftSubMenuButton)
            {
                submenu.Toggle.isOn = false;
                submenu.CraftPanelItems.gameObject.SetActive(false);
            }
        }

        craftSubMenuButton.CraftPanelItems.gameObject.SetActive(true);
        craftSubMenuButton.Toggle.isOn = true;
    }
}
