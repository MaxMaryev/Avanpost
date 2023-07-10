using UnityEngine;

public abstract class CraftItem : MonoBehaviour
{
    [SerializeField] private CraftMenuPresenter _craftMenuPresenter;

    private void OnEnable()
    {
        _craftMenuPresenter.Crafted += OnCraftedItem;
    }

    private void OnDisable()
    {
        _craftMenuPresenter.Crafted -= OnCraftedItem;
    }

    protected abstract void OnCraftedItem(ICraftableItem craftableItem);
}
