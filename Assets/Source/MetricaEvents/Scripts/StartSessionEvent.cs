using GameAnalyticsSDK;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StartSessionEvent : MonoBehaviour
{
    public const string KeySessionCount = "session_count";
    public const string KeyRegDate = "reg_date";
    public const string KeyDaysInGame = "days_in_game";

    private int _sessionCountDefaultValue = 0;
    private int _daysInGameDefaultValue = 0;
    private string _regDateDefaultValue = DateTime.Today.ToShortDateString();
    private int _sessionCount;
    private int _daysInGame;
    private string _regDate;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _sessionCount = PlayerPrefs.GetInt(KeySessionCount, _sessionCountDefaultValue);
        _daysInGame = PlayerPrefs.GetInt(KeyDaysInGame, _daysInGameDefaultValue);
        _regDate = PlayerPrefs.GetString(KeyRegDate, _regDateDefaultValue);

        RewardRegDate();
        RewardSession();
        RewardDaysInGame();
        Debug.Log(_sessionCount);
    }

    private void RewardSession()
    {
        _sessionCount++;
        PlayerPrefs.SetInt(KeySessionCount, _sessionCount);
        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("count", _sessionCount);
        AddEvent("session_count", eventParameters);
    }

    private void RewardRegDate()
    {
        if (_sessionCount > 0)
            return;

        PlayerPrefs.SetString(KeyRegDate, _regDate);
        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("date", _regDate);
        AddEvent("reg_day", eventParameters);
    }

    private void RewardDaysInGame()
    {
        int days = CalculateDaysInGame();

        if (days > _daysInGame)
        {
            _daysInGame = days;
            PlayerPrefs.SetInt(KeyDaysInGame, _daysInGame);
            Dictionary<string, object> eventParameters = new Dictionary<string, object>();
            eventParameters.Add("days", days);
            AddEvent("days_in_game", eventParameters);
        }
    }

    private int CalculateDaysInGame()
    {
        return (DateTime.Today - DateTime.Parse(_regDate)).Days + 1;
    }

    private void AddEvent(string nameEvent, Dictionary<string, object> eventParameters) => GameAnalytics.NewDesignEvent(nameEvent, eventParameters);
}
