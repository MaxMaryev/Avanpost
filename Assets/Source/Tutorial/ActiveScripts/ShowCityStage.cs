using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class ShowCityStage : TutorialStage
{
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private string _message;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _speedCameraRotate;
    [SerializeField] private PlayerMover _playerMover;

    private Coroutine _coroutine;
    private CinemachineOrbitalTransposer _transposer;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _playerMover.enabled = false;
        _transposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _hintHandler.Add(_message);
        _coroutine = StartCoroutine(ShowDelay());
    }

    protected override void OnExit()
    {
        _cinemachineVirtualCamera.enabled = false;
        _playerMover.enabled = true;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator ShowDelay()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => _hintHandler.IsAvailable == false);

        _cinemachineVirtualCamera.enabled = true;

        while (_transposer.m_XAxis.Value < 240)
        {
            _transposer.m_XAxis.Value += Time.deltaTime * _speedCameraRotate; 
            yield return null;
        }

        Completed?.Invoke(this);
    }
}
