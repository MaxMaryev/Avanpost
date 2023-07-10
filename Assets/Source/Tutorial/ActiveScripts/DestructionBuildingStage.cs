using System;
using System.Collections.Generic;
using UnityEngine;

public class DestructionBuildingStage : TutorialStage
{
    [SerializeField] private ContainersHandler _containersHandler;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        foreach (var building in _containersHandler.GetBuildings())
            if (building.Name != BuildingName.Mine && building.Name != BuildingName.Oil_lamp)
                building.SafetyMarginChanged += OnSafetyMarginChanged;
        
    }

    private void OnSafetyMarginChanged(float maxSafety, float currentSafety, Building destructabe = null)
    {
        if (currentSafety <= 0)
        {
            foreach (var building in _containersHandler.GetBuildings())
            {
                if (building.Name != BuildingName.Mine && building.Name != BuildingName.Oil_lamp)
                {
                    building.SafetyMarginChanged -= OnSafetyMarginChanged;
                }
            }

            Completed?.Invoke(this);
        }
    }

    protected override void OnExit()
    {

    }
}
