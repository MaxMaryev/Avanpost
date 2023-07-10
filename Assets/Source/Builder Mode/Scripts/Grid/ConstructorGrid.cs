using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorGrid : MonoBehaviour
{
    [SerializeField] private ConstructorCell _cellTemplate;
    [SerializeField] private float _cellSize;
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    private List<ConstructorCell> _cells = new List<ConstructorCell>();
    private ConstructorCell[,] _cellsMatrix;

    public IReadOnlyList<ConstructorCell> Cells => _cells;
    public bool IsCreated { get; private set; }

    private void Awake()
    {
        _cellsMatrix = new ConstructorCell[_height, _width];
        Create();
    }

    public ConstructorCell GetCellBy(Vector2Int index) => _cellsMatrix[index.y, index.x];

    public bool IsAreaEmpty(Vector2Int index, BuildingName buildingName)
    {
        foreach (var cell in GetArea(index, buildingName))
            if (cell.Building != null)
                return false;

        return true;
    }

    public void FillArea(Vector2Int index, Building building)
    {
        foreach (var cell in GetArea(index, building.Name))
            cell.Fill(building);

        _cellsMatrix[index.y, index.x].Fill(building);
    }

    public void ShowFillArea(Vector2Int index, BuildingName buildingName)
    {
        foreach (var cell in GetArea(index, buildingName))
            cell.ShowFill();

        _cellsMatrix[index.y, index.x].ShowFill();
    }

    public void UpdateColors()
    {
        foreach (var cell in _cells)
        {
            if (cell.Icon.color != Color.black && cell.Building == null)
                cell.ShowEmpty();
            else
                cell.ShowFill();
        }
    }

    public void ClearArea(Vector2Int index)
    {
        foreach (var cell in GetArea(index, _cellsMatrix[index.y, index.x].Building.Name))
            cell.ClearCell();

        _cellsMatrix[index.y, index.x].ClearCell();

        if (gameObject.activeSelf)
            StartCoroutine(DelayedUpdateColor());
    }

    public void ShowBlockArea(Vector2Int index, BuildingName buildingName)
    {
        foreach (var cell in GetArea(index, buildingName))
            cell.ShowBlock();

        _cellsMatrix[index.y, index.x].ShowBlock();
    }

    private IReadOnlyList<ConstructorCell> GetArea(Vector2Int index, BuildingName buildingName)
    {
        List<ConstructorCell> cells = new List<ConstructorCell>();

        if (buildingName == BuildingName.Turret || buildingName == BuildingName.Shotgun || buildingName == BuildingName.RocketLauncher)
        {
            foreach (var neighbor in _cellsMatrix[index.y, index.x].Neighbors)
                if (IsNeighborInsideTurretArea(index, neighbor))
                    cells.Add(neighbor);
        }
        else
        {
            cells.Add(_cellsMatrix[index.y, index.x]);
        }

        return cells;
    }

    private void Create()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Vector3 position = transform.position + new Vector3(x * _cellSize, 0, -y * _cellSize);
                _cellsMatrix[y, x] = Instantiate(_cellTemplate, position, transform.rotation, transform);
                _cellsMatrix[y, x].SetIndex(new Vector2Int(x, y));
                _cells.Add(_cellsMatrix[y, x]);
            }
        }

        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                _cellsMatrix[y, x].InitNeighbors(GetNeighbors(new Vector2Int(x, y)));

        IsCreated = true;
    }

    private bool IsNeighborInsideTurretArea(Vector2Int index, ConstructorCell neighbor)
    {
        if (neighbor.Index.y == index.y - 1 && neighbor.Index.x == index.x - 1)
            return false;

        if (neighbor.Index.y == index.y - 1 && neighbor.Index.x == index.x + 1)
            return false;

        return true;
    }

    private IReadOnlyList<ConstructorCell> GetNeighbors(Vector2Int index)
    {
        List<ConstructorCell> neighbors = new List<ConstructorCell>();

        TryAddNeighbor(index.y + 1, index.x - 1);
        TryAddNeighbor(index.y + 1, index.x);
        TryAddNeighbor(index.y + 1, index.x + 1);
        TryAddNeighbor(index.y, index.x + 1);
        TryAddNeighbor(index.y, index.x - 1);
        TryAddNeighbor(index.y - 1, index.x - 1);
        TryAddNeighbor(index.y - 1, index.x);
        TryAddNeighbor(index.y - 1, index.x + 1);

        return neighbors;

        void TryAddNeighbor(int y, int x)
        {
            if (y >= 0 && x >= 0 && y < _height && x < _width)
                neighbors.Add(_cellsMatrix[y, x]);
        }
    }

    private IEnumerator DelayedUpdateColor()
    {
        yield return new WaitForEndOfFrame();
        UpdateColors();
    }
}
