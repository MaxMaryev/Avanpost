using Lean.Localization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BuildingsPanelView : MonoBehaviour
{
    [SerializeField] private Image _buildingsPanelImage;
    [SerializeField] private Image _breakModeLine;
    [SerializeField] private List<SubmenuButton> _submenuButtons;
    [SerializeField] private List<Submenu> _submenus;
    [SerializeField] private RepairButton _repairButton;
    [SerializeField] private InstallationButton _installationButton;
    [SerializeField] private BreakButton _breakButton;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private BuildingsPanel _buildingsPanel;
    [SerializeField] private TMP_Text _junklessMessage;
    [SerializeField] private TMP_Text _cantBuildGateMessage;
    [SerializeField] private TMP_Text _removeHint;
    [SerializeField] private TMP_Text _breakButtonText;
    [SerializeField] private TMP_Text _installationButtonText;
    [SerializeField] private Color _removeModeColor;
    [SerializeField] private Color _junklessMessageDarkColor;
    [SerializeField] private Color _junklessMessageLightColor;

    private bool _isMessageShowing;

    private void OnEnable()
    {
        _buildingInstallation.Done += OnBuildingInstallationDone;
        _buildingRemoving.Done += OnBuildingRemovingDone;
        UpdateSlotsAvailability();
    }

    private void OnDisable()
    {
        _buildingInstallation.Done -= OnBuildingInstallationDone;
        _buildingRemoving.Done -= OnBuildingRemovingDone;
    }

    private void Start()
    {
        _junklessMessage.text = LeanLocalization.CurrentTranslations["Junkless"].Data.ToString();
        _removeHint.text = LeanLocalization.CurrentTranslations["RemoveHint"].Data.ToString();
        _breakButtonText.text = LeanLocalization.CurrentTranslations["Break"].Data.ToString();
        _installationButtonText.text = LeanLocalization.CurrentTranslations["ToBuilderMode"].Data.ToString();
        _cantBuildGateMessage.text = LeanLocalization.CurrentTranslations["GateBuildWarning"].Data.ToString();
    }

    public void ShowInstallationMode()
    {
        _buildingsPanelImage.color = Color.white;
        _removeHint.gameObject.SetActive(false);
        _breakModeLine.gameObject.SetActive(false);
        EnableButtons(true);
    }

    public void ShowBreakMode()
    {
        _buildingsPanelImage.color = _removeModeColor;
        _removeHint.gameObject.SetActive(true);
        _breakModeLine.gameObject.SetActive(true);
        EnableButtons(false);
    }

    public void ShowJunklessMessage() => ShowMessage(_junklessMessage);

    public void ShowGateBuildWarning() => ShowMessage(_cantBuildGateMessage);

    private void ShowMessage(TMP_Text message)
    {
        if (_isMessageShowing)
            return;

        _isMessageShowing = true;
        message.gameObject.SetActive(true);

        DOTween.Sequence()
            .Append(message.transform.DOScale(0.95f, 0.4f))
            .Append(message.transform.DOScale(1f, 0.4f))
            .SetLoops(5);

        DOTween.Sequence()
            .Append(message.DOColor(_junklessMessageDarkColor, 0.4f))
            .Append(message.DOColor(_junklessMessageLightColor, 0.4f))
            .SetLoops(5).OnComplete(Hide);

        void Hide()
        {
            message.gameObject.SetActive(false);
            _isMessageShowing = false;
        }
    }

    private void UpdateSlotsAvailability()
    {
        foreach (var slotButton in _buildingsPanel.SlotButtons)
                slotButton.UpdateAvailabilityView();
    }

    private void EnableButtons(bool state)
    {
        foreach (var submenuButton in _submenuButtons)
        {
            submenuButton.gameObject.SetActive(state);

            if (submenuButton.isActiveAndEnabled == state)
                if (submenuButton.Toggle.isOn)
                    submenuButton.Submenu.gameObject.SetActive(state);
        }

        _repairButton.gameObject.SetActive(state);
        _breakButton.gameObject.SetActive(state);

        _installationButton.gameObject.SetActive(!state);
    }

    private void OnBuildingInstallationDone(Building building)
    {
        UpdateSlotsAvailability();
    }

    private void OnBuildingRemovingDone()
    {
        UpdateSlotsAvailability();
    }
}
