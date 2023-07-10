using System.Collections.Generic;
using UnityEngine;

public class ContainersHandler : MonoBehaviour
{
    [SerializeField] private Transform _obstaclesContainer;
    [SerializeField] private Transform _turretsContainer;
    [SerializeField] private Transform _trapsContainer;
    [SerializeField] private Transform _lightsContainer;

    public Transform GetContainerFor(IContainerable containerable)
    {
        if (containerable.Class == BuildingClass.Obstacle)
            return _obstaclesContainer;
        else if(containerable.Class == BuildingClass.Turret)
            return _turretsContainer;
        else if (containerable.Class == BuildingClass.Trap)
            return _trapsContainer;
        else if (containerable.Class == BuildingClass.Light)
            return _lightsContainer;

        return null;
    }

    public IReadOnlyList<Building> GetBuildings()
    {
        return GetComponentsInChildren<Building>();
    }

    public IReadOnlyList<Turret> GetTurrets()
    {
        return _turretsContainer.GetComponentsInChildren<Turret>();
    }
}
