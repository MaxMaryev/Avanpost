using UnityEngine;

public class CleaningPlayerSaveAtDeath : MonoBehaviour
{
    public const string KeyLeftGameDead = "left_game_dead";

    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private RewardRevive _rewardRevive;

    private bool _isCleaning = true;

    private void OnEnable()
    {
        _restartButton.Clicked += OnRestartButtonClicked;
        _rewardRevive.RewardClicked += OnReviveClicked;

        PlayerPrefs.SetInt(KeyLeftGameDead, 1);
    }

    private void OnDisable()
    {
        _restartButton.Clicked -= OnRestartButtonClicked;
        _rewardRevive.RewardClicked -= OnReviveClicked;

        if (_isCleaning)
            CleareAllSave();
        else
            CleareTimeDay();

        PlayerPrefs.DeleteKey(KeyLeftGameDead);
    }

    private void OnRestartButtonClicked() => _isCleaning = true;

    private void OnReviveClicked() => _isCleaning = false;

    private void CleareAllSave() => PlayerPrefs.DeleteAll();

    private void CleareTimeDay() => PlayerPrefs.DeleteKey(DayTimeSaver.KeyDay);
}
