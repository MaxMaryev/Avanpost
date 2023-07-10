using System;
using System.Collections;
using UnityEngine;

public class GarbageReward : MonoBehaviour
{
    [SerializeField] private WalletPresenter _walletPresenter;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private ResourceRewardView _templateGarbageRewardView;
    [SerializeField] private RewardLoot _rewardLoot;

    private Pool<ResourceRewardView> _pool;
    private Vector3 _offset = new Vector3(0, 1, 0);
    private int _rewardBonus = 1;

    public event Action<int> Rewarded;

    private void Awake()
    {
        _pool = new Pool<ResourceRewardView>(_templateGarbageRewardView, transform, 5);
    }

    private void OnEnable()
    {
        _resourceSpawner.ResourceSpawned += OnResourceSpawned;
        _rewardLoot.RewardActivated += OnRewardActivated;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceSpawned -= OnResourceSpawned;
        _rewardLoot.RewardActivated -= OnRewardActivated;
    }

    private void OnResourceSpawned(Resource resource)
    {
        resource.Picked += OnResourcePicked;
    }

    private void OnResourcePicked(Resource resource)
    {
        resource.Picked -= OnResourcePicked;
        int reward = resource.Amount * _rewardBonus;
        _walletPresenter.AddResource(reward);
        ResourceRewardView garbageRewardView = _pool.GetFreeElement();
        garbageRewardView.transform.position = resource.transform.position + _offset;
        garbageRewardView.Init(reward);
        garbageRewardView.gameObject.SetActive(true);
        Rewarded?.Invoke(reward);
    }

    private void OnRewardActivated(int bonus, float duration)
    {
        StartCoroutine(IncreaseRewardBonus());

        IEnumerator IncreaseRewardBonus()
        {
            _rewardBonus = bonus;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            _rewardBonus = 1;
        }
    }
}
