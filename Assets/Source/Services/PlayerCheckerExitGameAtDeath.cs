using System;
using UnityEngine;

public class PlayerCheckerExitGameAtDeath : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerPresenter _playerPresenter;

    private void OnEnable()
    {
        _playerPresenter.Initialized += OnPlayerPresenterInitialized;
    }

    private void OnDisable()
    {
        _playerPresenter.Initialized -= OnPlayerPresenterInitialized;
    }

    private void OnPlayerPresenterInitialized()
    {
        bool isDead = PlayerPrefs.HasKey(CleaningPlayerSaveAtDeath.KeyLeftGameDead);

        if (isDead)
        {
            _player.TakeDamage(1000);
        }
    }
}
