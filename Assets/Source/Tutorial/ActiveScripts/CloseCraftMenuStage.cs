using System;
using UnityEngine;

public class CloseCraftMenuStage : TutorialStage
{
    [SerializeField] private ButtonCloseCraftMenu _closeCraftMenuButton;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _closeCraftMenuButton.Clicked += OnCloseCraftMenuButtonClicked;
    }

    private void OnCloseCraftMenuButtonClicked()
    {
        _closeCraftMenuButton.Clicked -= OnCloseCraftMenuButtonClicked;

        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        
    }
}
