using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBuildingToFirstGame : MonoBehaviour
{
    [SerializeField] private TutorialStage _tutorialStage;
    [SerializeField] private BuildingSlotsDatasSO _buildingSlotsSO;
    [SerializeField] private List<BuildingIdentificator> _buildingIdentificators = new List<BuildingIdentificator>();
    [SerializeField] private List<BuildingIdentificator> _buildingIdentificatorsForSkipTutorial = new List<BuildingIdentificator>();
    [SerializeField] private BuildingsSaver _buildingsSaver;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private FencesHandler _fencesHandler;

    private List<Building> _buildings = new List<Building>();
    private List<Fence> _fences = new List<Fence>();

    public IReadOnlyList<BuildingIdentificator> BuildingIdentificators => _buildingIdentificators;

    private void OnEnable()
    {
        _tutorialStage.Entered += OnStageEntered;
    }

    private void OnDisable()
    {
        _tutorialStage.Entered -= OnStageEntered;
    }

    private void OnStageEntered()
    {
        SpawnStarterBuildings(false);

        _tutorialStage.Completed += OnTutorialStageCompleted;
    }

    public void SpawnStarterBuildings(bool isSave)
    {
        int index = 0;

        foreach (var buildingIdentificator in isSave == false ? _buildingIdentificators : _buildingIdentificatorsForSkipTutorial)
        {
            ConstructorCell cell = _constructorGrid.GetCellBy(buildingIdentificator.StartPosition);

            BuildingSlotData slotData;
            Building building;

            if (buildingIdentificator.Identifier == BuildingName.None)
            {
                FenceChain fenceChain = _fencesHandler.CreateNewChain(fences: _fences.ToArray());
                fenceChain.Update(isNeedEdjesUpdate: true);
                fenceChain.InitNeighbors();
                _fences.Clear();
            }
            else
            {
                slotData = _buildingSlotsSO.GetSlotBy(buildingIdentificator.Identifier);
                building = _buildingInstallation.CreateBuilding(slotData, cell.transform.position, cell.Index);

                if (isSave == false)
                    _buildingsSaver.DeleteFromSaveList(building);

                _buildings.Add(building);
                _buildingInstallation.CleareCurrentTargetBuilding();

                if (building is Fence fence)
                {
                    fence.InitOrderNumber(index);
                    _fences.Add(fence);
                    index++;
                }

                if (building is Gate gate)
                    gate.TryAttach();
            }
        }
    }

    private void OnTutorialStageCompleted(TutorialStage obj)
    {
        _buildings = _buildings.Where(building => building != null).ToList();

        for (int i = 0; i < _buildings.Count; i++)
        {
            _buildingsSaver.SaveOnQuitGame(_buildings[i]);
        }

        obj.Completed -= OnTutorialStageCompleted;
    }
}
