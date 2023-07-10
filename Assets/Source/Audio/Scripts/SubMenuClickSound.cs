using UnityEngine;

public class SubMenuClickSound : PlayClickSound
{
    [SerializeField] private SubmenuButton _subMenu;

    private void OnEnable()
    {
        _subMenu.Clicked += OnSubMenuClicked;
    }

    private void OnDisable()
    {
        _subMenu.Clicked -= OnSubMenuClicked;
    }

    private void OnSubMenuClicked(SubmenuButton submenu) => PlaySound();
}
