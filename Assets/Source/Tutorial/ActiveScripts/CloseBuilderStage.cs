using System;
using UnityEngine;

public class CloseBuilderStage : TutorialStage
{
    [SerializeField] private ExitButton _exitButton;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _exitButton.Clicked += OnCloseCraftMenuButtonClicked;
    }

    private void OnCloseCraftMenuButtonClicked()
    {
        _exitButton.Clicked -= OnCloseCraftMenuButtonClicked;

        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {

    }
}
