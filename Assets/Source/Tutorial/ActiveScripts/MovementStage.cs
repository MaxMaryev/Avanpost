using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class MovementStage : TutorialStage
{
    [SerializeField] private InputSystemHandler _inputSystemHandler;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private Canvas _mobile;
    [SerializeField] private Canvas _pc;

    private Coroutine _coroutine;
    private bool _isMobile = false;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _dayCycleManager.ResetDay();
        StartCoroutine(IdentifyDevice());
    }

    private IEnumerator IdentifyDevice()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        while (YandexGamesSdk.IsInitialized == false)
            yield return null;

        _isMobile = Agava.YandexGames.Device.Type != Agava.YandexGames.DeviceType.Desktop;
#endif

        yield return new WaitForEndOfFrame();

        if (_isMobile)
            _mobile.gameObject.SetActive(true);
        else
            _pc.gameObject.SetActive(true);

        _coroutine = StartCoroutine(WaitStartMove());
    }

    protected override void OnExit()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _mobile.gameObject.SetActive(false);
        _pc.gameObject.SetActive(false);
    }

    private IEnumerator WaitStartMove()
    {
        yield return new WaitUntil(() => _inputSystemHandler.GetDirection().sqrMagnitude > Constants.Epsilon);

        Completed?.Invoke(this);
    }
}
