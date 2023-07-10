using System;
using UnityEngine;

public class FirstBreakBuildingStage : TutorialStage
{
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private UIOutline _uIOutlines;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Vector3 _offsetHandClick;
    [SerializeField] private Canvas _background;
    [SerializeField] private Vector2Int _targetIndexForGate;
    [SerializeField] private Empty _emptyTemplate;

    public override event Action<TutorialStage> Completed;
    private ConstructorCell _constructorCell;

    protected override void OnEnter()
    {
        _constructorCell = _constructorGrid.GetCellBy(_targetIndexForGate);

        if (_constructorCell.Building != null)
        {
            if (_constructorCell.Building.Name == BuildingName.Gate_Lvl_1)
            {
                Completed?.Invoke(this);
                return;
            }
        }

        //_buildingRemoving.AddIndexAllowedBreak(_targetIndexForGate);
        _handClickView.transform.SetParent(_constructorCell.transform, false);
        _handClickView.transform.localPosition = _offsetHandClick;
        _handClickView.gameObject.SetActive(true);
        _uIOutlines.transform.SetParent(_constructorCell.transform, false);
        _uIOutlines.gameObject.SetActive(true);

        _background.gameObject.SetActive(true);
        _buildingRemoving.Done += OnBuildingRemovingDone;
    }

    private void OnBuildingRemovingDone()
    {
        _buildingRemoving.Done -= OnBuildingRemovingDone;
        _constructorCell.Fill(Instantiate(_emptyTemplate));
        //_buildingRemoving.RemoveIndexAllowedBreak(_targetIndexForGate);
        Completed?.Invoke(this);
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
