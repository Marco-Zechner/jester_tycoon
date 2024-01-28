using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlaceInfo : MonoBehaviour{
    public PlaceInfo parentPlace = null;
    [SerializeField]
    public List<PlaceInfo> childPlaces = new List<PlaceInfo>();
    public SOBuilding buildingInfo;

    public int currentVisitors = 0;
    public int currentStage = 0;

    [Button]
    public int SpreadVisitors(int visitors)
    {
        int maxVisitors = 0;
        if (buildingInfo != null && buildingInfo.stages.Length > currentStage)
        {
            maxVisitors = buildingInfo.stages[currentStage].maxVisitors;
        }

        if (visitors > maxVisitors)
        {
            int visitorsToSpread = visitors - maxVisitors;
            currentVisitors = maxVisitors;
            if (childPlaces.Count > 0)
            {
                foreach (PlaceInfo child in childPlaces)
                {
                    visitorsToSpread = child.SpreadVisitors(visitorsToSpread);
                }
                return visitorsToSpread;
            }
            else
            {
                return visitorsToSpread;
            }
        }
        else
        {
            currentVisitors = visitors;
            return 0;
        }
    }

    public bool hasUpgrade()
    {
        return buildingInfo.stages.Length > currentStage + 1;
    }

    public int UpgradeCost()
    {
        return buildingInfo.stages[currentStage + 1].cost.amount;
    }

    public int getUpgradedValueOfType(ResourceType type)
    {
        return getValueOfTypeStage(type, currentStage + 1);
    }
    public int sellValue()
    {
        return buildingInfo.stages[currentStage].cost.amount;
    }

    public int getValueOfType(ResourceType type)
    {
        return getValueOfTypeStage(type, currentStage);
    }

    private int getValueOfTypeStage(ResourceType type, int currentStage)
    {
        int sum = 0;

        if (buildingInfo != null && buildingInfo.constantResources != null)
        {
            foreach (DependentResource resource in buildingInfo.constantResources)
            {
                if (resource.type == type)
                {
                    sum += resource.amount;
                }
            }
        }

        if (buildingInfo != null && buildingInfo.stages.Length > currentStage)
        {
            foreach (DependentResource resource in buildingInfo.stages[currentStage].resources)
            {
                if (resource.type == type)
                {
                    sum += resource.amount;
                }
            }
        }
        
        return sum;
    }
}