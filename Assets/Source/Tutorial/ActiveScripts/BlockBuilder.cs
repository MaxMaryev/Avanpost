using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private Building _emptyTemplate;
    [SerializeField] private TutorialStage _tutorialStage;
    [SerializeField] private SpawnBuildingToFirstGame _spawnBuildingToFirst;

    private List<ConstructorCell> _emptyCells = new List<ConstructorCell>();
    private List<ConstructorCell> _filledCells = new List<ConstructorCell>();
    private List<ConstructorCell> _cellsNotFilled = new List<ConstructorCell>();

    public IReadOnlyList<ConstructorCell> EmptyCells => _emptyCells;
    public IReadOnlyList<ConstructorCell> FilledCells => _filledCells;
    public IReadOnlyList<ConstructorCell> CellsNotFilled => _cellsNotFilled;

    private void OnEnable()
    {
        _tutorialStage.Exited += OnStageExited;
    }

    private void Start()
    {
        for (int i = 0; i < _constructorGrid.Cells.Count; i++)
        {
            if (_constructorGrid.Cells[i].Building == null)
            {
                _constructorGrid.Cells[i].Fill(Instantiate(_emptyTemplate));
                _emptyCells.Add(_constructorGrid.Cells[i]);
            }
            else
            {
                _filledCells.Add(_constructorGrid.Cells[i]);
            }
        }
    }

    private void OnStageExited()
    {
        _tutorialStage.Exited -= OnStageExited;

        for (int i = 0; i < _spawnBuildingToFirst.BuildingIdentificators.Count; i++)
        {
            ConstructorCell cell = _constructorGrid.GetCellBy(_spawnBuildingToFirst.BuildingIdentificators[i].StartPosition);

            if (cell != null)
            {
                if (cell.Building == null)
                {
                    if (_spawnBuildingToFirst.BuildingIdentificators[i].Identifier != BuildingName.Fence_Lvl_1)
                    {
                        cell.Fill(Instantiate(_emptyTemplate));
                        continue;
                    }
                    else 
                    {
                        cell.Fill(Instantiate(_emptyTemplate));
                        _cellsNotFilled.Add(cell);
                    }
                }
                else if (cell.Building as Empty && _spawnBuildingToFirst.BuildingIdentificators[i].Identifier == BuildingName.Fence_Lvl_1)
                {
                    _cellsNotFilled.Add(cell);
                }
            }
        }
    }
}
