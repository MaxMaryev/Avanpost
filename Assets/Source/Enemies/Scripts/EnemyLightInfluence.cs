using UnityEngine;

public class EnemyLightInfluence : MonoBehaviour
{
    private DayCycleManager _dayCycleManager;

    public bool IsHide => LightsCount == 0 && _dayCycleManager.CurrentTimeDay is Night;
    public int LightsCount { get; private set; }

    private void OnDisable()
    {
        LightsCount = 0;
    }

    public void Init(DayCycleManager dayCycleManager) => _dayCycleManager = dayCycleManager;

    public void IncreaseLigthsCount() => LightsCount++;

    public void DecreaseLigthsCount() => LightsCount--;
}
