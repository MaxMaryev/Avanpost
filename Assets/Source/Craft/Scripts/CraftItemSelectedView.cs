using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemSelectedView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _craftText;
    [SerializeField] private Color _availableCreateColor;
    [SerializeField] private Color _notAvailableCreateColor;
    [SerializeField] private ButtonCraftItem _buttonCraftItem;
    [SerializeField] private Sprite _emptySprite;

    [SerializeField] private TMP_Text _damageParameter;
    [SerializeField] private TMP_Text _rateFireParameter;
    [SerializeField] private TMP_Text _clipCapacityParameter;
    [SerializeField] private TMP_Text _reloadParameter;
    [SerializeField] private VerticalLayoutGroup _gridParametes;

    public void ShowCraftItem(ICraftableItem craftableItem, bool HasEnoughJunkForCraft)
    {
        if (craftableItem == null)
        {
            _buttonCraftItem.gameObject.SetActive(false);
            _image.sprite = _emptySprite;
            _gridParametes.gameObject.SetActive(false);
            return;
        }

        _buttonCraftItem.gameObject.SetActive(true);
        _gridParametes.gameObject.SetActive(true);
        _image.sprite = craftableItem.Poster;
        _priceText.text = craftableItem.JunkNeeded.ToString();
        _buttonCraftItem.Button.interactable = HasEnoughJunkForCraft;

        if (craftableItem is WeaponSlotData slotData)
        {
            _damageParameter.text = LeanLocalization.CurrentTranslations[slotData.Damage].Data.ToString();
            _rateFireParameter.text = LeanLocalization.CurrentTranslations[slotData.RateFire].Data.ToString();
            _clipCapacityParameter.text = LeanLocalization.CurrentTranslations[slotData.StoreCapacity].Data.ToString();
            _reloadParameter.text = slotData.Reload + LeanLocalization.CurrentTranslations["s"].Data.ToString();
        }

        if (HasEnoughJunkForCraft == false)
            SetTextsColor(_notAvailableCreateColor);
        else
            SetTextsColor(_availableCreateColor);

        _craftText.text = $"{LeanLocalization.CurrentTranslations["Create"].Data}";
    }

    private void SetTextsColor(Color color)
    {
        _priceText.color = color;
    }
}
