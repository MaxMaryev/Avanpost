using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectsPool : MonoBehaviour
{
    [SerializeField] private List<Enemy> _disabled = new();
    [SerializeField] private DayCycleManager _dayCycleManager;

    private List<Enemy> _enabled = new();
    private WaitForSeconds _waitEndDyingAnimation;

    public event Action<IRewardGiver> EnemyDied;

    public bool HasAvailable => _disabled.Count > 0;
    public IReadOnlyList<IVisible> Enabled => _enabled;
    public List<Enemy> Corpses { get; private set; } = new();

    private void OnEnable() => _dayCycleManager.TimeDayChanged += OnDayTimeChanged;

    private void OnDisable()
    {
        foreach (var enemy in _disabled)
            enemy.Killed -= OnDied;

        foreach (var enemy in _enabled)
            enemy.Killed -= OnDied;

        _dayCycleManager.TimeDayChanged -= OnDayTimeChanged;
    }

    public Enemy GetAvailable()
    {
        Enemy enemy = _disabled.FirstOrDefault(enemy => enemy.gameObject.activeSelf == false);

        if (enemy)
        {
            _enabled.Add(enemy);
            _disabled.Remove(enemy);
            return enemy;
        }

        return null;
    }

    public void AddNew(Enemy enemy)
    {
        _enabled.Add(enemy);
        enemy.Killed += OnDied;
    }

    public void AddToDisable(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _disabled.Add(enemy);
    }

    private void RemoveFromEnabled(Enemy enemy) => _enabled.Remove(enemy);

    private IEnumerator DelayedAddToDisabledList(Enemy enemy, float delay)
    {
        _waitEndDyingAnimation ??= new WaitForSeconds(delay);
        RemoveFromEnabled(enemy);
        yield return _waitEndDyingAnimation;
        AddToDisable(enemy);
        Corpses.Remove(enemy);
    }

    public void OnStartPoolInstantiated()
    {
        _disabled.AddRange(GetComponentsInChildren<Enemy>());

        foreach (var enemy in _disabled)
        {
            enemy.gameObject.SetActive(false);
            enemy.Killed += OnDied;
        }
    }

    private void OnDayTimeChanged(TimeDay timeDay)
    {
        if (timeDay is Night)
        {
            foreach (var enemy in _enabled)
                AddToDisable(enemy);

            _enabled.Clear();
        }
    }

    private void OnDied(Enemy enemy, float dyingDelay)
    {
        Corpses.Add(enemy);
        EnemyDied?.Invoke(enemy);
        StartCoroutine(DelayedAddToDisabledList(enemy, dyingDelay));
    }
}
