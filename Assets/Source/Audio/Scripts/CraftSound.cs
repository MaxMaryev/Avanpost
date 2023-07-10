using UnityEngine;

public class CraftSound : PlayClickSound
{
    [SerializeField] private CraftMenuPresenter _craftMenuPresenter;

    private void OnEnable()
    {
        _craftMenuPresenter.Crafted += OnCrafted;
    }

    private void OnDisable()
    {
        _craftMenuPresenter.Crafted -= OnCrafted;
    }

    private void OnCrafted(ICraftableItem craftableItem) => PlaySound();
}
