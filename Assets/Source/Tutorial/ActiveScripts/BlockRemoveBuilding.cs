using System;
using UnityEngine;

public class BlockRemoveBuilding : MonoBehaviour
{
    [SerializeField] private TutorialStage _startStage;
    [SerializeField] private TutorialStage _endStage;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private Vector2Int _targetIndexForGate;

    private void Start()
    {
        _startStage.Entered += OnStageEntered;
    }

    private void OnStageEntered()
    {
        _startStage.Entered -= OnStageEntered;
        _endStage.Exited += OnStageExited;
        _buildingRemoving.AddIndexAllowedBreak(_targetIndexForGate);
    }

    private void OnStageExited()
    {
        _endStage.Exited -= OnStageExited;
        _buildingRemoving.RemoveIndexAllowedBreak(_targetIndexForGate);
    }
}
