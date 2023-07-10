using Lean.Localization;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Confirmer : MonoBehaviour
{
    [SerializeField] private ButtonUI _yesButton;
    [SerializeField] private ButtonUI _noButton;
    [SerializeField] private Canvas _canvasConfirmer;
    [SerializeField] private TMP_Text _message;
    [SerializeField] private TimeScaleHandler _timeScaleHandler;
    [SerializeField] private Canvas _blockingCanvas;

    private Action _acceptAction;
    private Action _cancelAction;

    private void OnEnable()
    {
        _yesButton.Clicked += OnYesButtonClicked;
        _noButton.Clicked += OnNoButtonClicked;
    }

    private void OnDisable()
    {
        _yesButton.Clicked -= OnYesButtonClicked;
        _noButton.Clicked -= OnNoButtonClicked;
    }

    public void AskConfirmation(Action acceptAction, Action cancelAction = null, string message = null)
    {
        _message.text = message == null ? LeanLocalization.CurrentTranslations["Confirm"].Data.ToString() : LeanLocalization.CurrentTranslations[message].Data.ToString();
        _blockingCanvas.gameObject.SetActive(true);
        _canvasConfirmer.enabled = true;
        _acceptAction = acceptAction;
        _cancelAction = cancelAction;
        _timeScaleHandler.FreezeTime(isMute: false);
    }

    private void OnYesButtonClicked()
    {
        _timeScaleHandler.UnFreezeTime();
        _canvasConfirmer.enabled = false;
        _blockingCanvas.gameObject.SetActive(false);
        _acceptAction?.Invoke();
    }

    private void OnNoButtonClicked()
    {
        _timeScaleHandler.UnFreezeTime();
        _blockingCanvas.gameObject.SetActive(false);
        _canvasConfirmer.enabled = false;
        _cancelAction?.Invoke();
    }
}
