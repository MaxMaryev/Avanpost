using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuildingsSaver : MonoBehaviour
{

    [SerializeField] private ContainersHandler _containersHandler;
    [SerializeField] private PlayerAnimationsController _playerAnimationsController;
    [SerializeField] private BuildingSlotsDatasSO _buildingSlotsDatas;
    [SerializeField] private List<Building> _buildings;
    [SerializeField] private LightHandler _lightHandler;
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private BuildingsInitializer _buildingsInitializer;
    [SerializeField] private FencesHandler _fencesHandler;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private BuildingInstallation _buildingInstallation;

    private BuildingsSavesID _buildingsSavesID;

    public bool HasLoaded { get; private set; }

    private void OnEnable() => StartCoroutine(LoadAfterGridInitialization());

    private void Start() => StartCoroutine(Saving());

    public void SaveOnQuitGame(Building building) => _buildings.Add(building);

    public void DeleteFromSaveList(Building building)
    {
        _buildings.Remove(building);

        if (_buildingsSavesID != null)
        {
            _buildingsSavesID.Load();
            _buildingsSavesID.Identificators.Remove(building.Id);
            _buildingsSavesID.Save();
        }
    }

    private IEnumerator Saving()
    {
        float savingCooldown = 2;

        WaitForSeconds waitCooldown = new WaitForSeconds(savingCooldown);
        yield return new WaitUntil(() => HasLoaded);

        while (true)
        {
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i] == null)
                    continue;

                Save(_buildings[i]);
                yield return null;
            }

            yield return waitCooldown;
        }
    }

    private void Save(Building building)
    {
        if (building.Id == null)
        {
            building.InitBuildingData(GenerateID());
            _buildingsSavesID.Add(building.Id);
            _buildingsSavesID.Save();
        }

        Vector3 rotation = building.transform.rotation.eulerAngles;

        if (building.BuildingData.IsImmutableParametersSet == false)
            building.BuildingData.SetImmutableParameters((int)building.Name, (int)building.Class, building.Cell.Index);

        building.BuildingData.TrySetMutableParameteres(building.transform.position.x, building.transform.position.y, building.transform.position.z,
            rotation.x, rotation.y, rotation.z, building.CurrentSafetyMargin);


        if (building is Turret turret)
            building.BuildingData.SetCurrentBullets(turret.FiringWeapon.CurrentBulletsCount);

        if (building is Fence fence)
        {
            building.BuildingData.SetOrderNumber(fence.OrderNumber);
            building.BuildingData.SetChainId(fence.ChainId);
            building.BuildingData.TrySetFenceModel((int)fence.Model);
        }

        building.BuildingData.Save();
    }

    private void Load()
    {
        _buildingsSavesID = new BuildingsSavesID();
        _buildingsSavesID.Load();

        if (_buildingsSavesID.Identificators.Count == 0)
        {
            HasLoaded = true;
            return;
        }

        for (int i = 0; i < _buildingsSavesID.Identificators.Count; i++)
        {
            BuildingData buildingData = new BuildingData(_buildingsSavesID.Identificators[i]);
            buildingData.Load();
            BuildingSlotData slotData = _buildingSlotsDatas.GetSlotBy((BuildingName)buildingData.Name);
            Vector3 position = new Vector3(buildingData.PositionX, buildingData.PositionY, buildingData.PositionZ);
            Quaternion rotation = Quaternion.Euler(buildingData.RotationX, buildingData.RotationY, buildingData.RotationZ);

            //Building building = Instantiate(slotData.BuildingTemplate, position, rotation);
            //ConstructorCell cell = _constructorGrid.GetCellBy(buildingData.CellIndex);
            //building.Init(slotData.Name, slotData.Class, cell, slotData.MaxSafetyMargin, _buildingRemoving, buildingData.CurrentSafetyMargin);
            //Transform containerForCrafted = _containersHandler.GetContainerFor(building);
            //building.InitBuildingsSaver(this);
            //building.Place(containerForCrafted);
            Building building = _buildingInstallation.CreateBuilding(slotData, position, buildingData.CellIndex, rotation);
            _constructorGrid.FillArea(buildingData.CellIndex, building);
            //_buildingsInitializer.OnBuildingInstallationDone(building);

            if (building is Turret turret)
            {
                int currentBulletsCount = buildingData.CurrentBulletsCount;
                turret.FiringWeapon.InitCurrentBulletsCount(currentBulletsCount);
                
            }

            if (building is Fence fence)
            {
                FenceModel fenceModel = (FenceModel)buildingData.FenceModel;
                fence.ShowModel(fenceModel);
                fence.InitOrderNumber(buildingData.OrderNumber);
                fence.InitChainId(buildingData.ChainId);

                FenceChain fenceChain = _fencesHandler.GetChain(fence.ChainId);

                if (fenceChain == null)
                    _fencesHandler.CreateNewChain(fence.ChainId, fence);
                else
                    fenceChain.Add(fence);
            }

            if (building is Gate gate)
            {
                gate.TryAttach();
            }

            _buildingInstallation.CleareCurrentTargetBuilding();
            buildingData.Clear();
        }

        foreach (var chain in _fencesHandler.FenceChains)
        {
            chain.Update(isNeedEdjesUpdate: true);
            chain.InitNeighbors();
        }

        _buildingsSavesID.Clear();
        _buildingsSavesID.Save();
        HasLoaded = true;
    }

    private string GenerateID() => UnityEngine.Random.Range(0, int.MaxValue).ToString();

    private IEnumerator LoadAfterGridInitialization()
    {
        yield return new WaitUntil(() => _constructorGrid.IsCreated);
        Load();
    }
}

public class BuildingsSavesID : SavedObject<BuildingsSavesID>
{
    [field: SerializeField] public List<string> Identificators { get; private set; } = new List<string>();

    public BuildingsSavesID() : base(nameof(BuildingsSavesID)) { }

    public void Add(string id)
    {
        Identificators.Add(id);
        Save();
    }

    public void Clear() => Identificators.Clear();

    protected override void OnLoad(BuildingsSavesID loadedObject)
    {
        Identificators = loadedObject.Identificators;
    }
}
