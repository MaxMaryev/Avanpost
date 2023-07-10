using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardRevive : RewardAdvertising
{
    private const string FirstRevive = "FirstRevive";

    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private SavesCleaner _savesCleaner;

    protected override void OnEnable()
    {
        if(PlayerPrefs.HasKey(FirstRevive))
            return;

        base.OnEnable();
        _playerDeath.DeathAnimationEnd += OnDeathAnimationEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerDeath.DeathAnimationEnd -= OnDeathAnimationEnd;
    }

    protected override void Start() { }

    protected override void OnReward()
    {
        PlayerPrefs.SetInt(FirstRevive, True);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _savesCleaner.ClearTimeDay();
    }

    private void OnDeathAnimationEnd() => ShowButton();
}
