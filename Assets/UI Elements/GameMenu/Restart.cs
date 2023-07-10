using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private TimeScaleHandler _timeScaleHandler;
    [SerializeField] private SavesCleaner _savesCleaner;

    private void OnEnable()
    {
        _restartButton.Clicked += RestartGame;
    }

    private void OnDisable()
    {
        _restartButton.Clicked -= RestartGame;
    }

    private void RestartGame()
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
