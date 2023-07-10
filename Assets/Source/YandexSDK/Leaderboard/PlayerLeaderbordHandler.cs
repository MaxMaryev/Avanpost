using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Player))]
public class PlayerLeaderbordHandler : MonoBehaviour
{
    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private Player _player;

    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private YandexLeaderboard _leaderboard;
    [SerializeField] private Canvas _canvas;


    private void OnEnable()
    {
        _playerDeath.DeathAnimationEnd += OnDeathAnimationEnd;
        //_dayCycleManager.TimeDayChanged += OnTimeDayChanged;
    }

    //private void Start()
    //{
    //    _player.Health.UnderZero += OnHealthUnderZero;
    //}

    private void OnDisable()
    {
        //_player.Health.UnderZero -= OnHealthUnderZero;
        //_dayCycleManager.TimeDayChanged -= OnTimeDayChanged;
        _playerDeath.DeathAnimationEnd -= OnDeathAnimationEnd;
    }

    private void ShowLeaderBoard()
    {
        _canvas.enabled = true;
        _leaderboard.transform.DOShakeScale(1, 0.1f, 5);
        _leaderboard.Show();
    }

    private void OnDeathAnimationEnd() => ShowLeaderBoard();

//    private void OnTimeDayChanged(TimeDay timeDay)
//    {
//#if UNITY_WEBGL && !UNITY_EDITOR
//        _leaderboard.SetScore(_dayCycleManager.GetTotalLivingTimeInHours());
//#endif
//    }

    //private void OnHealthUnderZero() => _leaderboard.SetScore(_dayCycleManager.GetTotalLivingTimeInHours());
}
