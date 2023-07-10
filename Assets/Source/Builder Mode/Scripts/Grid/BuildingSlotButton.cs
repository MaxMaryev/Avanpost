using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lean.Localization;
using TMPro;
using System;
using System.Collections;
using System.Linq;

public class BuildingSlotButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _itemNameTMPro;
    [SerializeField] private TMP_Text _safetyMarginTMPro;
    [SerializeField] private TMP_Text _priceTMPro;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Sprite _unlockSprite;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Image _lockImage;
    [SerializeField] private Color _lockColor;
    [SerializeField] private Color _unlockColor;

    private WalletPresenter _walletPresenter;

    public event Action<BuildingSlotData> Selected;
    public event Action Clicked;

    public BuildingSlotData SlotData { get; private set; }

    private void OnEnable() => StartCoroutine(SelectToggledOnStart());

    private void Start() => Translate();

    public void OnPointerDown(PointerEventData eventData)
    {
        Selected?.Invoke(SlotData);
        Clicked?.Invoke();
    }

    public void Init(BuildingSlotData slotData, ToggleGroup toggleGroup, WalletPresenter walletPresenter)
    {
        SlotData = slotData;
        _iconImage.sprite = slotData.Icon;

        _safetyMarginTMPro.text = slotData.MaxSafetyMargin.ToString();
        _priceTMPro.text = slotData.JunkNeeded.ToString();
        _toggle.group = toggleGroup;
        _walletPresenter = walletPresenter;
        UpdateAvailabilityView();
    }

    public void UpdateAvailabilityView()
    {
        if (_walletPresenter.Value < SlotData.JunkNeeded)
        {
            _priceTMPro.color = _lockColor;

            if (_lockImage.sprite != _lockSprite)
                _lockImage.sprite = _lockSprite;
        }
        else
        {
            _priceTMPro.color = _unlockColor;

            if (_lockImage.sprite != _unlockSprite)
                _lockImage.sprite = _unlockSprite;
        }
    }

    public void SetToggle(bool state) => _toggle.isOn = state;

    private IEnumerator SelectToggledOnStart()
    {
        yield return new WaitUntil(() => SlotData != null);

        if (_toggle.isOn)
            Selected?.Invoke(SlotData);
    }

    private void Translate()
    {
        string name = SlotData.Name.ToString();
        string[] nameWords;

        if (SlotData.Class == BuildingClass.Obstacle)
        {
            nameWords = name.Split('_');
            name = $"{LeanLocalization.CurrentTranslations[nameWords[0]].Data} {LeanLocalization.CurrentTranslations[nameWords[1]].Data} {nameWords[2]}";
        }
        else
        {
            name = LeanLocalization.CurrentTranslations[name].Data.ToString();
        }

        _itemNameTMPro.text = name;
    }
}