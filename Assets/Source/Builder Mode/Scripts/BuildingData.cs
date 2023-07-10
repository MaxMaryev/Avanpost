using UnityEngine;
using UnityEngine.UIElements;

public class BuildingData : SavedObject<BuildingData>
{
    public BuildingData(string guid) : base(guid) { }

    [field: SerializeField] public int Name { get; private set; }
    [field: SerializeField] public int CraftItemClass { get; private set; }
    [field: SerializeField] public float PositionX { get; private set; }
    [field: SerializeField] public float PositionY { get; private set; }
    [field: SerializeField] public float PositionZ { get; private set; }
    [field: SerializeField] public float RotationX { get; private set; }
    [field: SerializeField] public float RotationY { get; private set; }
    [field: SerializeField] public float RotationZ { get; private set; }
    [field: SerializeField] public float CurrentSafetyMargin { get; private set; }
    [field: SerializeField] public Vector2Int CellIndex { get; private set; }

    [field: SerializeField] public int CurrentBulletsCount { get; private set; }
    [field: SerializeField] public int FenceModel { get; private set; }
    [field: SerializeField] public int OrderNumber { get; private set; }
    [field: SerializeField] public int ChainId { get; private set; }

    public bool IsImmutableParametersSet { get; private set; }

    public void TrySetMutableParameteres(float positionX, float positionY, float positionZ,
        float rotationX, float rotationY, float rotationZ, float currentSafetyMargin)
    {
        if (PositionX != positionX)
            PositionX = positionX;

        if (PositionY != positionY)
            PositionY = positionY;

        if (PositionZ != positionZ)
            PositionZ = positionZ;

        if (RotationX != rotationY)
            RotationX = rotationX;

        if (RotationY != rotationY)
            RotationY = rotationY;

        if (RotationZ != rotationZ)
            RotationZ = rotationZ;

        if (CurrentSafetyMargin != currentSafetyMargin)
            CurrentSafetyMargin = currentSafetyMargin;
    }

    public void SetImmutableParameters(int name, int craftItemClass, Vector2Int cellIndex)
    {
        Name = name;
        CraftItemClass = craftItemClass;
        CellIndex = cellIndex;

        IsImmutableParametersSet = true;
    }

    public void SetCurrentBullets(int currentBulletsCount)
    {
        if (currentBulletsCount != CurrentBulletsCount)
            CurrentBulletsCount = currentBulletsCount;
    }

    public void SetOrderNumber(int orderNumber)
    {
        if (OrderNumber != orderNumber)
            OrderNumber = orderNumber;
    }

    public void SetChainId(int chainId)
    {
        if(ChainId != chainId)
            ChainId = chainId;
    }

    public void TrySetFenceModel(int fenceModel)
    {
        if (fenceModel != FenceModel)
            FenceModel = fenceModel;
    }

    protected override void OnLoad(BuildingData loadedObject)
    {
        Name = loadedObject.Name;
        CraftItemClass = loadedObject.CraftItemClass;
        PositionX = loadedObject.PositionX;
        PositionY = loadedObject.PositionY;
        PositionZ = loadedObject.PositionZ;
        RotationX = loadedObject.RotationX;
        RotationY = loadedObject.RotationY;
        RotationZ = loadedObject.RotationZ;
        CurrentSafetyMargin = loadedObject.CurrentSafetyMargin;
        CellIndex = loadedObject.CellIndex;

        FenceModel = loadedObject.FenceModel;
        OrderNumber = loadedObject.OrderNumber;
        ChainId = loadedObject.ChainId;
        CurrentBulletsCount = loadedObject.CurrentBulletsCount;
    }
}
