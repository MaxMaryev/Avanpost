using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsInitializator : MonoBehaviour
{
    private void Awake() => GameAnalytics.Initialize();
}
