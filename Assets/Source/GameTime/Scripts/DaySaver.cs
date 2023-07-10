using UnityEngine;

public class DaySaver 
{
    private const string KeyDay = "day";

    private int _defaultValue = 1;

    public int Load()
    {
        return PlayerPrefs.GetInt(KeyDay, _defaultValue);
    }

    public void Save(int value)
    {
        PlayerPrefs.SetInt(KeyDay, value);
    }
}
