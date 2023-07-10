using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivableBuilderModeStage : TutorialStage
{
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private List<string> _messages;
    //[SerializeField] private InstallationButton _builderModeButton;
    [SerializeField] private UIOutline _uIOutline;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        foreach (var messgage in _messages)
            _hintHandler.Add(messgage);

        //_builderModeButton.gameObject.SetActive(true);
        _uIOutline.gameObject.SetActive(true);

        //_builderModeButton.Clicked += OnBuilderModeClicked;
    }

    private void OnBuilderModeClicked()
    {
        _uIOutline.gameObject.SetActive(false);
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        _hintHandler.SkipAllHitns();
    }
}
