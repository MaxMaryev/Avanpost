using System;
using System.Collections.Generic;
using UnityEngine;

public class SubTutorial : TutorialStage
{
    [SerializeField] private List<TutorialStage> _subTutorialStages = new List<TutorialStage>();

    private int _currentSubTutorialStageIndex;
    private TutorialStage _currentSubTutorialStage;
    private List<TaskView> _taskViews = new List<TaskView>();

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        for (int i = 0; i < _subTutorialStages.Count; i++)
            _subTutorialStages[i].Completed += OnSubTutorialStageCompleted;

        _currentSubTutorialStage = _subTutorialStages[_currentSubTutorialStageIndex];
        _currentSubTutorialStage.Enter();
    }

    protected override void OnExit()
    {
        for (int i = 0; i < _subTutorialStages.Count; i++)
            _subTutorialStages[i].Completed -= OnSubTutorialStageCompleted;
    }

    private void OnSubTutorialStageCompleted(TutorialStage tutorialStage)
    {
        tutorialStage.Completed -= OnSubTutorialStageCompleted;
        _currentSubTutorialStageIndex++;

        if (_currentSubTutorialStage != null)
            _currentSubTutorialStage.Exit();

        _currentSubTutorialStage.gameObject.SetActive(false);

        if (_currentSubTutorialStageIndex >= _subTutorialStages.Count)
        {
            Completed?.Invoke(this);
            return;
        }

        _currentSubTutorialStage = _subTutorialStages[_currentSubTutorialStageIndex];

        if (_currentSubTutorialStage != null)
            _currentSubTutorialStage.Enter();
    }

    public void SkipStageTo(TutorialStage tutorialStage)
    {
        if (_subTutorialStages.Contains(tutorialStage) == false)
        {
            Debug.LogError("It is impossible to proceed to this stage!");
            return;
        }

        _currentSubTutorialStage.Completed -= OnSubTutorialStageCompleted;

        if (_currentSubTutorialStage != null)
            _currentSubTutorialStage.Exit();

        _currentSubTutorialStage.gameObject.SetActive(false);
        _currentSubTutorialStage = tutorialStage;
        _currentSubTutorialStageIndex = _subTutorialStages.IndexOf(_currentSubTutorialStage);

        if (_currentSubTutorialStage != null)
            _currentSubTutorialStage.Enter();
    }
}
