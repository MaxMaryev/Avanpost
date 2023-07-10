using UnityEngine;

public class TutorialSaver
{
    public const string KeyTutorial = "tutorial";
    public const string KeySkipTutorialPanel = "tutorial_skip";

    private int _defaultValue = -1;
    private int _defaultValueSkipTutorial = 0;

    public int LoadStage()
    {
        return PlayerPrefs.GetInt(KeyTutorial, _defaultValue);
    }

    public bool LoadSkipTutorial()
    {
        return PlayerPrefs.GetInt(KeySkipTutorialPanel, _defaultValueSkipTutorial) == 0 ? false : true;
    }

    public void SaveStage(int index)
    {
        PlayerPrefs.SetInt(KeyTutorial, index);
    }

    public void SaveSkipTutorial(bool isPassed)
    {
        PlayerPrefs.SetInt(KeySkipTutorialPanel, isPassed ? 1 : 0);
    }
}
