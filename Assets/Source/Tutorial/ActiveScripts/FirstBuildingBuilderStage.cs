using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstBuildingBuilderStage : TutorialStage
{
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private Building _emptyTemplate;
    [SerializeField] private UIOutline _uIOutlines;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Vector3 _offsetHandClick;
    [SerializeField] private Canvas _background;
    [SerializeField] private BlockBuilder _blockBuilder;

    private int _currentCellIndex;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        if(_blockBuilder.CellsNotFilled.Count == 0)
        {
            Completed?.Invoke(this);
            return;
        }

        UpdateHandClickPosition();

        _background.gameObject.SetActive(true);
        _buildingInstallation.Done += Doned;
    }

    private void UpdateHandClickPosition()
    {
        Destroy(_blockBuilder.CellsNotFilled[_currentCellIndex].Building.gameObject);
        _blockBuilder.CellsNotFilled[_currentCellIndex].ClearCell();
        _handClickView.transform.SetParent(_blockBuilder.CellsNotFilled[_currentCellIndex].transform, false);
        _handClickView.transform.localPosition = _offsetHandClick;
        _handClickView.gameObject.SetActive(true);
        _uIOutlines.transform.SetParent(_blockBuilder.CellsNotFilled[_currentCellIndex].transform, false);
        _uIOutlines.gameObject.SetActive(true);
    }

    private void Doned(Building building)
    {
        _currentCellIndex++;

        if (_currentCellIndex >= _blockBuilder.CellsNotFilled.Count)
        {
            _buildingInstallation.Done -= Doned;
            Completed?.Invoke(this);
            return;
        }

        UpdateHandClickPosition();
    }

    protected override void OnExit()
    {
        _background.gameObject.SetActive(false);
        _uIOutlines.transform.SetParent(transform, false);
        _uIOutlines.gameObject.SetActive(false);
        _handClickView.transform.SetParent(transform, false);
        _handClickView.gameObject.SetActive(false);
    }
}
