using System.Collections;
using UnityEngine;
using Agava.YandexGames;
using Agava.YandexMetrica;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject _sceneObjects;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        StartCoroutine(Localize());
        StartCoroutine(SDKInitialized());
    }

    private IEnumerator Localize()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        while (YandexGamesSdk.IsInitialized == false)
            yield return null;

        string domen = YandexGamesSdk.Environment.i18n.lang;
        SetLanguage(domen);
    }

    private IEnumerator SDKInitialized()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        Debug.Log("PersonalDataPermission");
        yield return YandexGamesSdk.Initialize(() => PlayerAccount.RequestPersonalProfileDataPermission());
    }

    private void SetLanguage(string domen)
    {
        string language = domen switch
        {
            "en" => "English",
            "ru" => "Russian",
            "tr" => "Turkish",
            _ => ""
        };

        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(language);
        _sceneObjects.SetActive(true);
    }
}
