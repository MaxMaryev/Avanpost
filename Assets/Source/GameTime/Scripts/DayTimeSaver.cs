using UnityEngine;

public class DayTimeSaver 
{
    public const string KeyDay = "dayTime";

    public float Load(float defaultValue)
    {
        return PlayerPrefs.GetFloat(KeyDay, defaultValue);
    }

    public void Save(float value)
    {
        PlayerPrefs.SetFloat(KeyDay, value);
    }
}
