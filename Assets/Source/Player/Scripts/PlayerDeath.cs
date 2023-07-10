using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Lean.Localization;
using UnityEngine.AI;
using Agava.YandexMetrica;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private List<Canvas> _canvases;
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Collider _collider;
    [SerializeField] private TimeScaleHandler _timeScaleHandler;
    [SerializeField] private LayerMask _cameraLayerMask;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private HintHandler _hintHandler;

    private Health _health;

    public event Action DeathAnimationEnd;

    private void OnDisable() => _health.UnderZero -= OnHealthUnderZero;

    public void Init(Health health)
    {
        _health = health;
        _health.UnderZero += OnHealthUnderZero;
    }

    private void OnHealthUnderZero()
    {
        _timeScaleHandler.UnFreezeTime();
        _dayCycleManager.FreezeTime();
        Camera.main.cullingMask = _cameraLayerMask;
        _cinemachineCamera.enabled = true;

        _hintHandler.gameObject.SetActive(false);
        _collider.enabled = false;
        _navMeshAgent.enabled = false;

        foreach (var canvas in _canvases)
            canvas.enabled = false;

        InvokeRepeating(nameof(PlayBloodEffect), 1.5f, 0.5f);
        Invoke(nameof(ShowText), 2f);
        Invoke(nameof(ShowButtons), 5);
    }

    private void PlayBloodEffect() => _particleSystem.Play();

    private void ShowText()
    {
        _canvas.gameObject.SetActive(true);
        Color color = _textMeshPro.color;
        Color noAlphaColor = new Color(color.r, color.g, color.b, 0.75f);
        _textMeshPro.text = LeanLocalization.CurrentTranslations["YOU DIED"].Data.ToString();
        _textMeshPro.DOColor(noAlphaColor, 15);
        _textMeshPro.transform.DOLocalMoveY(0, 7);
    }

    private void ShowButtons()
    {
        DeathAnimationEnd?.Invoke();
        _restartButton.gameObject.SetActive(true);
    }
}
