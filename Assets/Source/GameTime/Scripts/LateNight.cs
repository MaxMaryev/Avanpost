using System.Collections;
using System.Linq;
using UnityEngine;

public class LateNight : TimeDay
{
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private ObjectsPool[] _objectsPools;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private TimeDay _nextTimdeDay;

    private Coroutine _rewindCoroutine;

    public override void Enter()
    {
        base.Enter();
        _rewindCoroutine = StartCoroutine(RewindTimeAfterAllEnemiesDied());
    }

    public override void Exit()
    {
        if (_rewindCoroutine != null)
            StopCoroutine(_rewindCoroutine);

        base.Exit();
    }

    private IEnumerator RewindTimeAfterAllEnemiesDied()
    {
        yield return new WaitUntil(() => GetLenghtPools() == 0);
        _dayCycleManager.RewindTime(_nextTimdeDay.StartNormalizedTime);
    }

    private int GetLenghtPools() => _objectsPools.Where(x => x.Enabled.Count > 0).Count();
}
