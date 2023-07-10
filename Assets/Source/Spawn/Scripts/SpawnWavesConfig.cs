using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnWave", menuName = "Spawn/Wave", order = 65)]
public class SpawnWavesConfig : ScriptableObject
{
    [SerializeField] private List<WaveConfig> _waveConfigs;

    public WaveConfig GetWaveBy(int dayNumber)
    {
        WaveConfig waveConfig = _waveConfigs.FirstOrDefault(wave => wave.DayNumber == dayNumber);
        return waveConfig == null ? _waveConfigs[_waveConfigs.Count - 1] : waveConfig;
    }
}

[Serializable]
public class WaveConfig
{
    [field: SerializeField] public List<Zombie> ZombiesTemplates { get; private set; }
    [field: SerializeField] public List<Hulk> HulkTemplates { get; private set; }
    [field: SerializeField] public float HulkSpawnChanceInPercent { get; private set; }
    [field: SerializeField] public float SpawnDelay { get; private set; }
    [field: SerializeField] public int MaxCount { get; private set; }
    [field: SerializeField] public bool IsStopWhenMaxCountReached { get; private set; }
    [field: SerializeField] public int DayNumber { get; private set; }
}
