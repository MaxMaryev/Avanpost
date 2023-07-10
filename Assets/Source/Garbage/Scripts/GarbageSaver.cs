using UnityEngine;

public class GarbageSaver
{
    private const string KeyDay = "garbagepoint";

    private int _defaultValue = 0;

    public int Load(int id)
    {
        return PlayerPrefs.GetInt(KeyDay + id, _defaultValue);
    }

    public void Save(int value, int id)
    {
        PlayerPrefs.SetInt(KeyDay + id, value);
    }
}
