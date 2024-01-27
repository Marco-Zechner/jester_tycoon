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
        if (placeInfo != null && placeInfo.Resources != null)
        {
            foreach (DependentResource resource in placeInfo.Resources)
            {
                if (resource.type == type)
                {
                    sum += resource.amount;
                }
            }
        }

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
}
