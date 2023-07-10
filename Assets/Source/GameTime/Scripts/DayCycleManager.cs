using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private List<TimeDay> _timeDays = new List<TimeDay>();
    [SerializeField] private float _timeSecondsInFullDay;
    [SerializeField] private float _currentNormalizedTime;

    private float _timeSpeedMultiplier = 1;
    private TimeDay _currentTimeDay;
    private int _currentIndexTimeDay = 0;
    private DaySaver _daySaver;
    private DayTimeSaver _dayTimeSaver;
    private Coroutine _coroutineSave;
    private Coroutine _coroutineRewind;

    public event Action<TimeDay> TimeDayChanged;
    public event Action<int> DayChanged;

    public float TimeSpeedMultiplier => _timeSpeedMultiplier;
    public float CurrentNormalizedTime => _currentNormalizedTime;
    public int CurrentDay { get; private set; } = 1;
    public TimeDay CurrentTimeDay => _currentTimeDay;
    public float TimeSecondsInFullDay => _timeSecondsInFullDay;

    private void Awake()
    {
        _daySaver = new DaySaver();
        _dayTimeSaver = new DayTimeSaver();
        CurrentDay = _daySaver.Load();
        _currentNormalizedTime = _dayTimeSaver.Load(_currentNormalizedTime);
        _coroutineSave = StartCoroutine(SaveDelay());
    }

    private void OnDisable()
    {
        if (_coroutineSave != null)
            StopCoroutine(_coroutineSave);
    }

    private void Start()
    {
        TimeDay timeDay = GetTimeDayByNormalizedTime();
        ChangeTimeDay(timeDay);
    }

    private void Update()
    {
        _currentNormalizedTime += Time.deltaTime * _timeSpeedMultiplier / _timeSecondsInFullDay;
        transform.localRotation = Quaternion.Euler((_currentNormalizedTime * 360f) - 90, 170, 0);

        if (_currentNormalizedTime >= 1)
        {
            _currentNormalizedTime = 0;
            NextDay();
        }

        _currentTimeDay?.Tick(_currentNormalizedTime);
    }

    public void FreezeTime()
    {
        _timeSpeedMultiplier = 0;
    }

    public void UnFreezeTime()
    {
        _timeSpeedMultiplier = 1;
    }

    public void SkipDayBeforeTo(TimeDay timeDay)
    {
        if (_currentTimeDay != null)
            _currentTimeDay.Completed -= OnTimeDayCompleted;

        _currentIndexTimeDay = _timeDays.IndexOf(timeDay);
        _currentNormalizedTime = timeDay.StartNormalizedTime;

        if (CurrentDay == 1 && timeDay is LateNight)
            NextDay();

        ChangeTimeDay(timeDay);
    }

    public void ResetDay()
    {
        CurrentDay = 1;
        _daySaver.Save(CurrentDay);
    }

    public void RewindTime(float targetNormalizedTime)
    {
        if (_coroutineRewind != null)
            StopCoroutine(_coroutineRewind);

        _coroutineRewind = StartCoroutine(RewindDelay(targetNormalizedTime));
    }

    private IEnumerator RewindDelay(float targetTime)
    {
        float _lastSpeedMultiplier = _timeSpeedMultiplier;
        float _speedMultiplier = _timeSecondsInFullDay / 20;
        _timeSpeedMultiplier = _speedMultiplier;

        yield return new WaitUntil(() => Mathf.Abs(_currentNormalizedTime - targetTime) < 0.01f);

        if (_timeSpeedMultiplier == _speedMultiplier)
            _timeSpeedMultiplier = _lastSpeedMultiplier;
        else
            _timeSpeedMultiplier = 1f;

        _coroutineRewind = null;
    }

    public int GetTotalLivingTimeInHours()
    {
        float totalNormilizedTime = (CurrentDay - 1) + CurrentNormalizedTime;
        int totalTimeOnHours = Mathf.RoundToInt(totalNormilizedTime * 24);
        return totalTimeOnHours;
    }

    private IEnumerator SaveDelay()
    {
        while (true)
        {
            _dayTimeSaver.Save(_currentNormalizedTime);
            yield return new WaitForSeconds(1f);
        }
    }

    private void NextDay()
    {
        CurrentDay++;
        DayChanged?.Invoke(CurrentDay);
        _daySaver.Save(CurrentDay);
    }

    private void ChangeTimeDay(TimeDay timeDay)
    {
        UnFreezeTime();

        if (_currentTimeDay != null)
        {
            _currentTimeDay.Completed -= OnTimeDayCompleted;
            _currentTimeDay.Exit();
        }

        _currentTimeDay = timeDay;

        if (_currentTimeDay != null)
        {
            _currentTimeDay.Enter();
            _currentTimeDay.Completed += OnTimeDayCompleted;
            TimeDayChanged?.Invoke(_currentTimeDay);
        }
    }

    private void OnTimeDayCompleted(TimeDay timeDay)
    {
        _currentIndexTimeDay++;
        ChangeTimeDay(GetNextTimeDay());
    }

    private TimeDay GetNextTimeDay()
    {
        if (_currentIndexTimeDay >= _timeDays.Count)
            _currentIndexTimeDay = 0;

        return _timeDays[_currentIndexTimeDay];
    }

    private TimeDay GetTimeDayByNormalizedTime()
    {
        foreach (var day in _timeDays)
        {
            if (day.IsTimeInRange(_currentNormalizedTime))
            {
                _currentIndexTimeDay = _timeDays.IndexOf(day);
                return day;
            }
        }

        return null;
    }
}
