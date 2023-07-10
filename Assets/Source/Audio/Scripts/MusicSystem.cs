using UnityEngine;
//using FMODUnity;
//using FMOD.Studio;
using System.Collections;

public class MusicSystem : MonoBehaviour
{
    private const string FmodNameParameter = "Music_system";

    //[SerializeField] private EventReference _musicEvent;
    [SerializeField] private DayCycleManager _dayCycleManager;

    //private EventInstance _musicInstance;
    private bool _isLoaded;

    private void OnDisable()
    {
        //_musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //_musicInstance.release();
    }

    void Start()
    {
        //StartCoroutine(CheckBanksLoaded());
    }

    void Update()
    {
        //if (_isLoaded)
            //_musicInstance.setParameterByName(FmodNameParameter, _dayCycleManager.CurrentNormalizedTime);
    }

    //private IEnumerator CheckBanksLoaded()
    //{
        //while (!RuntimeManager.HaveAllBanksLoaded)
        //{
        //    yield return null;
        //}

        //_musicInstance = RuntimeManager.CreateInstance(_musicEvent);
        //_musicInstance.start();
        //_isLoaded = true;
    //}
}
