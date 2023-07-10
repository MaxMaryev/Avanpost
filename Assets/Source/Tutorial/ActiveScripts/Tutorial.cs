using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private List<TutorialStage> _tutorialStages = new List<TutorialStage>();
    [SerializeField] private Confirmer _confirmer;
    [SerializeField] private TimeDay _timeDayToSkip;
    [SerializeField] private TimeDay _timeDayToRewind;
    [SerializeField] private SpawnBuildingToFirstGame _spawnBuildingToFirstGame;
    [SerializeField] private string _messageSkipTutorial;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private DailyTaskHandler _dailyTaskHandler;

    private TutorialStage _currentTutorialStage;
    private TutorialSaver _tutorialSaver;
    private int _currentIndexStage;

    //public event Action Started;
    public event Action Ended;
    public event Action<TutorialStage> StageChanged;
    public event Action SkipTutorialPanelShowed;
    public event Action SkipTutorialPanelClosed;

    public int CurrentIndexStage => _currentIndexStage;

    private void OnEnable()
    {
        foreach (var stage in _tutorialStages)
            stage.Completed += OnStageCompleted;
    }

    private void OnDisable()
    {
        foreach (var stage in _tutorialStages)
            stage.Completed -= OnStageCompleted;
    }

    private void Awake()
    {
        _tutorialSaver = new TutorialSaver();
        _currentIndexStage = _tutorialSaver.LoadStage();
    }

    private void Start()
    {
        if (_currentIndexStage < 0)
        {
            StartCoroutine(WaitFrame());
            return;
        }

        if (_currentIndexStage >= _tutorialStages.Count)
        {
            Disable();
            return;
        }

        ActivateStages();
    }

    private void ActivateStages()
    {
        for (int i = 0; i < _tutorialStages.Count; i++)
            _tutorialStages[i].gameObject.SetActive(i >= _currentIndexStage);

        SetState(_currentIndexStage);
    }

    private void RestartTutorial()
    {
        SkipTutorialPanelClosed?.Invoke();
        _dayCycleManager.UnFreezeTime();
        _playerMover.enabled = true;
        _currentIndexStage = 0;
        _tutorialSaver.SaveStage(_currentIndexStage);
        ActivateStages();
    }

    private void SkipTutorial()
    {
        SkipTutorialPanelClosed?.Invoke();
        _playerMover.enabled = true;
        _dayCycleManager.UnFreezeTime();
        _spawnBuildingToFirstGame.SpawnStarterBuildings(true);
        _dayCycleManager.SkipDayBeforeTo(_timeDayToSkip);
        _dayCycleManager.RewindTime(0.1f);
        _currentIndexStage = _tutorialStages.Count;
        _tutorialSaver.SaveStage(_currentIndexStage);
        Disable();
    }

    private void SetState(int index)
    {
        ChangeState(_tutorialStages[index]);
    }

    private void ChangeState(TutorialStage tutorialStage)
    {
        if (_currentTutorialStage != null)
        {
            _currentTutorialStage.Exit();
            _currentTutorialStage.gameObject.SetActive(false);
        }

        _currentTutorialStage = tutorialStage;
        StageChanged?.Invoke(_currentTutorialStage);

        if (_currentTutorialStage == null)
        {
            Disable();
            return;
        }

        _currentTutorialStage.Enter();
    }

    private void OnStageCompleted(TutorialStage tutorialStage)
    {
        tutorialStage.Completed -= OnStageCompleted;

        ChangeState(GetNextStage());
    }

    private TutorialStage GetNextStage()
    {
        _currentIndexStage++;
        _tutorialSaver.SaveStage(_currentIndexStage);

        if (_currentIndexStage >= _tutorialStages.Count)
            return null;

        return _tutorialStages[_currentIndexStage];
    }

    private void Disable()
    {
        _dailyTaskHandler.enabled = true;
        Ended?.Invoke();
        gameObject.SetActive(false);
    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();

        _confirmer.AskConfirmation(RestartTutorial, SkipTutorial, _messageSkipTutorial);
        SkipTutorialPanelShowed?.Invoke();
        _tutorialSaver.SaveSkipTutorial(true);
        _playerMover.enabled = false;
        _dayCycleManager.FreezeTime();
    }
}
