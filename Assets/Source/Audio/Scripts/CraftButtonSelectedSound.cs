using System;
using UnityEngine;

public class CraftButtonSelectedSound : PlayClickSound
{
    [SerializeField] private CraftPanelItems _craftPanelItems;

    private void OnEnable()
    {
        foreach (var btn in _craftPanelItems.CraftItemButtons)
            btn.Clicked += OnClicked;
    }

    private void OnDisable()
    {
        foreach (var btn in _craftPanelItems.CraftItemButtons)
            btn.Clicked -= OnClicked;
    }

    private void OnClicked() => PlaySound();
}
