using UnityEngine;

public class SavesCleaner : MonoBehaviour
{
    private const string FirstStart = "FirstStart";

    [SerializeField] private TimeScaleHandler _timeScaleHandler;

    private int _sessionCount;
    private int _daysInGame;
    private string _regDate;
    private int _firstStart;
    private int _firstSkipPanel;

    public void ClearAllSave()
    {
        _firstSkipPanel = PlayerPrefs.GetInt(TutorialSaver.KeySkipTutorialPanel, 0);
        _sessionCount = PlayerPrefs.GetInt(StartSessionEvent.KeySessionCount);
        _daysInGame = PlayerPrefs.GetInt(StartSessionEvent.KeyDaysInGame);
        _regDate = PlayerPrefs.GetString(StartSessionEvent.KeyRegDate);
        _firstStart = PlayerPrefs.GetInt(FirstStart);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(StartSessionEvent.KeySessionCount, _sessionCount);
        PlayerPrefs.SetString(StartSessionEvent.KeyRegDate, _regDate);
        PlayerPrefs.SetInt(StartSessionEvent.KeyDaysInGame, _daysInGame);
        PlayerPrefs.SetInt(FirstStart, 1);
        PlayerPrefs.SetInt(TutorialSaver.KeySkipTutorialPanel, _firstSkipPanel);
    }

    public void ClearTimeDay()
    {
        PlayerPrefs.DeleteKey(DayTimeSaver.KeyDay);
    }
}
