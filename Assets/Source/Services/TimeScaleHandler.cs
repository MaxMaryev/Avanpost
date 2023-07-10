using Agava.WebUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleHandler : MonoBehaviour
{
    [SerializeField] private List<RewardAdvertising> _rewardAdvertisings;
    [SerializeField] private GameSoundHandler _gameSoundHandler;

    private bool _inBackground;
    private float _savedTimeScale;
    private bool _savedPause;

    public event Action<float> Changed;

    private void Start()
    {
        Time.timeScale = 1;
        _savedTimeScale = Time.timeScale;
    }

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    public void FreezeTime(bool isMute = true)
    {
        _savedTimeScale = Time.timeScale;
        Time.timeScale = 0;

        if (isMute)
            _gameSoundHandler.Pause(true);
    }

    public void UnFreezeTime()
    {
        if (_inBackground == false)
        {
            Time.timeScale = 1;
            Changed?.Invoke(Time.timeScale);
            _gameSoundHandler.Pause(false);
        }
        else
        {
            _savedTimeScale = 1;
            _savedPause = false;
        }
    }

    private bool IsAdsPlaying()
    {
        foreach (var rewardAdvertising in _rewardAdvertisings)
            if (rewardAdvertising.IsAdsPlaying)
                return true;

        return false;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        _inBackground = inBackground;

        if (IsAdsPlaying())
            return;

        if (inBackground == false)
        {
            Time.timeScale = _savedTimeScale;
            Changed?.Invoke(Time.timeScale);
            _gameSoundHandler.Pause(_savedPause);
        }
        else
        {
            _savedPause = _gameSoundHandler.IsPause;
            _gameSoundHandler.Pause(true);
            _savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            Changed?.Invoke(Time.timeScale);
        }
    }
}
