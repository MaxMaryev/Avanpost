using System;
using UnityEngine;

public class FirstBuilderGateStage : TutorialStage
{
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private UIOutline _uIOutlines;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Vector3 _offsetHandClick;
    [SerializeField] private Canvas _background;
    [SerializeField] private BlockBuilder _blockBuilder;
    [SerializeField] private Vector2Int _targetIndexForGate;
    [SerializeField] private BuildingsPanel _buildingsPanel;
    [SerializeField] private ExitButton _exitBuilderMenuButton;

    private ConstructorCell _constructorCell;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _constructorCell = _constructorGrid.GetCellBy(_targetIndexForGate);

        if (_constructorCell.Building != null)
        {
            if (_constructorCell.Building as Gate == false)
            {
                Destroy(_constructorCell.Building.gameObject);
                _constructorCell.ClearCell();
            }
            else
            {
                Completed?.Invoke(this);
                return;
            }
        }

        _handClickView.transform.SetParent(_constructorCell.transform, false);
        _handClickView.transform.localPosition = _offsetHandClick;
        _handClickView.gameObject.SetActive(true);
        _uIOutlines.transform.SetParent(_constructorCell.transform, false);
        _uIOutlines.gameObject.SetActive(true);

        _background.gameObject.SetActive(true);
        _buildingInstallation.Done += Doned;
    }

    private void Doned(Building building)
    {
        _buildingInstallation.Done -= Doned;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        for (int i = 0; i < _constructorGrid.Cells.Count; i++)
        {
            if (_constructorGrid.Cells[i].Building is Empty)
            {
                Destroy(_constructorGrid.Cells[i].Building.gameObject);
                _constructorGrid.Cells[i].ClearCell();

            }
        }

        _exitBuilderMenuButton.Click();

        _background.gameObject.SetActive(false);
        _uIOutlines.transform.SetParent(transform, false);
        _uIOutlines.gameObject.SetActive(false);
        _handClickView.transform.SetParent(transform, false);
        _handClickView.gameObject.SetActive(false);
    }
}
