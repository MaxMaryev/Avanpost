using UnityEngine;
using System.Collections;

public class AMBDayNight : MonoBehaviour
{
    //private const string FmodNameParameter = "DayNight";

    //[SerializeField] private EventReference _ambEvent;
    //[SerializeField] private DayCycleManager _dayCycleManager;

    //private EventInstance _ambInstance;
    //private bool _isLoaded;

    //private void OnDisable()
    //{
    //    _ambInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    //    _ambInstance.release();
    //}

    //void Start()
    //{
    //    StartCoroutine(CheckBanksLoaded());
    //}

    //void Update()
    //{
    //    if (_isLoaded)
    //        _ambInstance.setParameterByName(FmodNameParameter, _dayCycleManager.CurrentNormalizedTime);
    //}

    //private IEnumerator CheckBanksLoaded()
    //{
    //    while (!RuntimeManager.HaveAllBanksLoaded)
    //    {
    //        yield return null;
    //    }

    //    _ambInstance = RuntimeManager.CreateInstance(_ambEvent);
    //    _ambInstance.start();
    //    _isLoaded = true;
    //}
}
