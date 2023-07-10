using System.Collections.Generic;
using UnityEngine;

public class BlockDestructionBuilding : MonoBehaviour
{
    [SerializeField] private TutorialStage _tutorialStage;
    [SerializeField] private ContainersHandler _containersHandler;

    [SerializeField] private List<Building> _buildings = new List<Building>();

    public IReadOnlyList<Building> Buildings => _buildings;

    private void Start()
    {
        foreach (var building in _containersHandler.GetBuildings())
            _buildings.Add(building);

        foreach (var building in _buildings)
        {
            //building.IsBlockDestruction = true;
            //building.ShowBreakButton(false);
            //building.ShowRepairBar(false);
        }
    }
}
