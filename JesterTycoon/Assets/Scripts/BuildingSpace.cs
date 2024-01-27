using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpace : MonoBehaviour
{
    public bool isOccupied = false;
    public int initialCostMultiplier = 1;
    public PlaceInfo placeInfo;
    public BuildingInfo buildingInfo;

    public int getValueOfType(ResourceType type)
    {
        int sum = 0;

        if (buildingInfo != null && buildingInfo.resources != null)
        {
            foreach (DependentResource resource in buildingInfo.resources)
            {
                if (resource.type == type)
                {
                    sum += resource.amount;
                }
            }
        }
        
        return sum;
    }

    public bool hasUpgrade()
    {
        return false; 
    }

    public int getUpgradedValueOfType(ResourceType type)
    {
        return 0;
    }
    public int sellValue()
    {
        return 0;
    }
}
