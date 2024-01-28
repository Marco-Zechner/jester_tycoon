using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlaceInfo : MonoBehaviour{
    public PlaceInfo parentPlace = null;
    [SerializeField]
    public List<PlaceInfo> childPlaces = new List<PlaceInfo>();
    public SOBuilding buildingInfo;

    public GameObject model;

    public int currentVisitors = 0;
    public int CurrentStage { 
        get => currentStage; 
        set {
            int money = GameManager.GetValue(GameManager.Value.Money);
            Debug.Log("Money: " + money + " Cost: " + buildingInfo.stages[value].cost.amount);
            if (money > buildingInfo.stages[value].cost.amount)
            {
                GameManager.AddValue(GameManager.Value.Money, -buildingInfo.stages[value].cost.amount);
                Debug.Log("Money: " + money);
                currentStage = value;
                if (buildingInfo != null && buildingInfo.stages.Length > currentStage)
                {
                    Destroy(model);
                    model = Instantiate(buildingInfo.stages[currentStage].prefab, transform);
                }
            }
        }
    }
    private int currentStage = 0;

    Dictionary<ResourceType, PlaceInfo> providers = new Dictionary<ResourceType, PlaceInfo>();
    Dictionary<ResourceType, List<PlaceInfo>> consumers = new Dictionary<ResourceType, List<PlaceInfo>>();

    public void GetProviders()
    {
        foreach (var consumer in consumers)
        {
            foreach (var place in consumer.Value)
            {
                place.GetProviders();
            }
        }

        foreach (var provider in providers)
        {
            provider.Value.consumers[provider.Key].Remove(this);
        }

        providers.Clear();
        consumers.Clear();
        int food = GetValueOfType(ResourceType.Food);
        if (food > 0)
        {
            consumers.Add(ResourceType.Food, null);
        }
        if (food < 0)
        {
            providers.Add(ResourceType.Food, null);
        }

        int energy = GetValueOfType(ResourceType.Energy);
        if (energy > 0)
        {
            consumers.Add(ResourceType.Energy, null);
        }
        if (energy < 0)
        {
            providers.Add(ResourceType.Energy, null);
        }

        int staff = GetValueOfType(ResourceType.Staff);
        if (staff > 0)
        {
            consumers.Add(ResourceType.Staff, null);
        }
        if (staff < 0)
        {
            providers.Add(ResourceType.Staff, null);
        }

        List<PlaceInfo> possibleProviders = GetAllInRange();

        foreach (var requiredResource in providers)
        {
            PlaceInfo provider = null;

            foreach (PlaceInfo possibleProvider in possibleProviders)
            {
                if (possibleProvider.consumers.ContainsKey(requiredResource.Key))
                {
                    provider = possibleProvider;
                    break;
                }
            }

            if (provider == null)
            {
                Debug.LogError("No provider found for " + requiredResource.Key);
            }
            else
            {
                providers[requiredResource.Key] = provider;
                provider.consumers[requiredResource.Key].Add(this);
            }
        }
    }

    private List<PlaceInfo> GetAllInRange()
    {
        BuildingManager buildingManager = FindObjectOfType<BuildingManager>();
        List<PlaceInfo> places = buildingManager.GetAllPlaces();

        List<PlaceInfo> placesInRange = new List<PlaceInfo>();

        foreach (PlaceInfo place in places)
        {
            if (place == this) continue;
            if (Vector3.Distance(place.transform.position, transform.position) <= place.buildingInfo.stages[place.CurrentStage].effectRadius)
            {
                placesInRange.Add(place);
            }
        }

        placesInRange.Sort((x, y) => Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position)));

        return placesInRange;
    }


    [Button]
    public int SpreadVisitors(int visitors)
    {
        int maxVisitors = 0;
        if (buildingInfo != null && buildingInfo.stages.Length > CurrentStage)
        {
            maxVisitors = buildingInfo.stages[CurrentStage].maxVisitors;
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

    public bool HasUpgrade()
    {
        return buildingInfo.stages.Length > CurrentStage + 1;
    }

    public int UpgradeCost()
    {
        return buildingInfo.stages[CurrentStage + 1].cost.amount;
    }

    public int GetUpgradedValueOfType(ResourceType type)
    {
        return GetValueOfTypeStage(type, CurrentStage + 1);
    }
    public int SellValue()
    {
        return buildingInfo.stages[CurrentStage].cost.amount;
    }

    public int GetValueOfType(ResourceType type)
    {
        return GetValueOfTypeStage(type, CurrentStage);
    }

    private int GetValueOfTypeStage(ResourceType type, int currentStage)
    {
        int sum = 0;

        if (buildingInfo != null && buildingInfo.constantResources != null)
        {
            foreach (DependentResource resource in buildingInfo.constantResources)
            {
                if (resource.type == type)
                {
                    switch (resource.usageType)
                    {
                        case UsageType.Produces:
                        sum += resource.amount;
                        break;
                        case UsageType.Consumes:
                        sum -= resource.amount;
                        break;
                    }
                }
            }
        }

        if (buildingInfo != null && buildingInfo.stages.Length > currentStage)
        {
            foreach (DependentResource resource in buildingInfo.stages[currentStage].resources)
            {
                if (resource.type == type)
                {
                    switch (resource.usageType)
                    {
                        case UsageType.Produces:
                        sum += resource.amount;
                        break;
                        case UsageType.Consumes:
                        sum -= resource.amount;
                        break;
                    }
                }
            }
        }
        
        return sum;
    }
}