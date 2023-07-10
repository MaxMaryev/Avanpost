using Lean.Localization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private AllCanvases _allCanvases;
    [SerializeField] private List<Canvas> _canvases;
    [SerializeField] private GameMenuButton _gameMenuButton;
    [SerializeField] private ResumeButton _resumeButton;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private TimeScaleHandler _timeScaleHandler;
    [SerializeField] private SavesCleaner _savesCleaner;
    [SerializeField] private TMP_Text _resumeButtonText;
    [SerializeField] private TMP_Text _restartButtonText;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Sprite _spriteMenuOn;
    [SerializeField] private Sprite _spriteMenuOff;
    [SerializeField] private Canvas _blockingCanvas;
    [SerializeField] private VolumeMainSettings _volumeMainSettings;

    private void Start()
    {
        _resumeButtonText.text = LeanLocalization.CurrentTranslations["RESUME"].Data.ToString();
        _restartButtonText.text = LeanLocalization.CurrentTranslations["RESTART"].Data.ToString();
        _label.text = LeanLocalization.CurrentTranslations["Menu"].Data.ToString();
    }

    private void OnEnable()
    {
        _gameMenuButton.Clicked += OnGameMenuButtonClicked;
        _resumeButton.Clicked += OnResumeButtonClicked;
        _restartButton.Clicked += OnRestartButtonClicked;
    }

    private void OnDisable()
    {
        _gameMenuButton.Clicked -= OnGameMenuButtonClicked;
        _resumeButton.Clicked -= OnResumeButtonClicked;
        _restartButton.Clicked -= OnRestartButtonClicked;
    }

    public void ShowButtons(bool state)
    {
        _volumeMainSettings.gameObject.SetActive(state);
        _restartButton.gameObject.SetActive(state);
        _resumeButton.gameObject.SetActive(state);
        _gameMenuButton.Button.image.sprite = state ? _spriteMenuOn : _spriteMenuOff;
    }

    private void OnGameMenuButtonClicked()
    {
        ShowButtons(true);
        _timeScaleHandler.FreezeTime(isMute: false);
        _allCanvases.gameObject.SetActive(false);
        _blockingCanvas.gameObject.SetActive(true);
        _label.gameObject.SetActive(true);

        foreach (var canvas in _canvases)
            canvas.enabled = false;
    }

    private void OnResumeButtonClicked()
    {
        ShowButtons(false);
        _timeScaleHandler.UnFreezeTime();
        _allCanvases.gameObject.SetActive(true);
        _label.gameObject.SetActive(false);
        _blockingCanvas.gameObject.SetActive(false);

        foreach (var canvas in _canvases)
            canvas.enabled = true;
    }

    private void OnRestartButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Agava.YandexGames.InterstitialAd.Show(() => _timeScaleHandler.FreezeTime(), onCloseCallback: (isClosed) =>
        {
            _savesCleaner.ClearAllSave();
            _timeScaleHandler.UnFreezeTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
#else
        _savesCleaner.ClearAllSave();
        _timeScaleHandler.UnFreezeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#endif
    }
}
