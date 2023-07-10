using UnityEngine;

[System.Serializable]
public class BuildingIdentificator 
{
    [SerializeField] private Vector2Int _startPosition;
    [SerializeField] private BuildingName _identifier;

    public Vector2Int StartPosition => _startPosition;
    public BuildingName Identifier => _identifier;
}
