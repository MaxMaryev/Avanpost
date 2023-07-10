using UnityEngine;

public class GameSoundHandler : MonoBehaviour
{
    private const string Volume = "Volume";

    public bool IsPause { get; private set; }

    private void Awake()
    {
        LoadVolume();
    }

    public void Pause(bool state)
    {
        IsPause = state;

        if (state)
        {
            PlayerPrefs.SetFloat(Volume, AudioListener.volume);
            AudioListener.volume = 0;
        }
        else
        {
            LoadVolume();
        }
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey(Volume))
            AudioListener.volume = PlayerPrefs.GetFloat(Volume);
        else
            AudioListener.volume = 0.7f;
    }
}
