using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayColors", menuName = "DayColors", order = 64)]
public class DayColors : ScriptableObject
{
    [SerializeField] private List<DayTimeColors> _dayTimeColors;

    public DayTimeColors GetColors(DayTime dayTime)
    {
        return _dayTimeColors.Find(colors => colors.DayTime == dayTime);
    }
}

[Serializable]
public class DayTimeColors
{
    [field: SerializeField] public DayTime DayTime { get; private set; }
    [field: SerializeField] public Color SkyColor { get; private set; }
    [field: SerializeField] public Color EquatorColor { get; private set; }
    [field: SerializeField] public Color HorizonColor { get; private set; }
}