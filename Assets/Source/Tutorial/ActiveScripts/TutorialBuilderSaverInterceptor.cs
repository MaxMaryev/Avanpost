using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuilderSaverInterceptor : MonoBehaviour
{
    [SerializeField] private TutorialStage _tutorialStage;
    //[SerializeField] private Builder _builder;
    [SerializeField] private BuildingsSaver _buildingsSaver;
    [SerializeField] private List<Building> _buildings = new List<Building>();

    private void OnEnable()
    {
        _tutorialStage.Entered += OnTutorialEntered;
    }

    private void OnDisable()
    {
        _tutorialStage.Entered -= OnTutorialEntered;
    }

    private void OnTutorialEntered()
    {
        _tutorialStage.Completed += OnTutorialStageCompleted;
       // _builder.Selected += OnBuilderSelected;
    }

    private void OnBuilderSelected(Building building)
    {
        _buildings.Add(building);
        //_builder.InstallationDone += OnBuilderPlaced;
    }

    private void OnBuilderPlaced()
    {
        //_builder.InstallationDone -= OnBuilderPlaced;
        _buildingsSaver.DeleteFromSaveList(_buildings[_buildings.Count - 1]);
    }

    private void OnTutorialStageCompleted(TutorialStage tutorialStage)
    {
        _tutorialStage.Completed -= OnTutorialStageCompleted;
        //_builder.Selected -= OnBuilderSelected;

        foreach (var building in _buildings)
            _buildingsSaver.SaveOnQuitGame(building);

        _buildings.Clear();

        enabled = false;
    }
}
