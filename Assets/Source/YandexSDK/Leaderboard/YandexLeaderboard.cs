using System.Collections;
using Agava.YandexGames;
using UnityEngine;

public class YandexLeaderboard : MonoBehaviour
{
    private const string Name = "Leaderboard";

    [SerializeField] private PlayerRankingView _rankingViewPrefab;
    [SerializeField] private PlayerRankingView _playerRank;
    [SerializeField] private Transform _container;
    [SerializeField] private LeaderBoardView _leaderboardView;

    private int _rankingMaxCount = 25;

    public void SetScore(int currentValue)
    {
        Leaderboard.GetPlayerEntry(Name, (result) =>
        {
            if (result == null || result.score < currentValue)
            {
                Leaderboard.SetScore(Name, currentValue);
            }
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Clear();

        if (PlayerAccount.IsAuthorized == false)
            OfferAuthorization();
        else if (PlayerAccount.HasPersonalProfileDataPermission == false)
            PlayerAccount.RequestPersonalProfileDataPermission(() => GetLeaderboardData());
        else
            GetLeaderboardData();

    }

    public void GetLeaderboardData()
    {
        Leaderboard.GetEntries(Name, (result) => OnSuccess(result), topPlayersCount: _rankingMaxCount, competingPlayersCount: _rankingMaxCount);

        void OnSuccess(LeaderboardGetEntriesResponse result)
        {
            foreach (var entry in result.entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                PlayerRankingView playerRankingView = Instantiate(_rankingViewPrefab, _container);
                playerRankingView.Initialize(entry.rank.ToString(), name, entry.score);

                if (entry.rank == _rankingMaxCount)
                    break;
            }

            if (result.entries.Length >= 6)
                _leaderboardView.ShowSlider();
        }
    }

    public void Authorize()
    {
        PlayerAccount.Authorize(OnSucces);
        StartCoroutine(Reload());

        void OnSucces()
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            _leaderboardView.HideAuthorizeOffer();
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitUntil(() => PlayerAccount.IsAuthorized && PlayerAccount.HasPersonalProfileDataPermission);
        GetLeaderboardData();
    }

    private void Clear()
    {
        PlayerRankingView[] _ranking = _container.GetComponentsInChildren<PlayerRankingView>();

        foreach (var item in _ranking)
            Destroy(item.gameObject);
    }

    private void OfferAuthorization() => _leaderboardView.ShowAuthorizeOffer();
}
