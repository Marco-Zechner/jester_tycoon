using UnityEngine;

[System.Serializable]
public class BuildingInfo
{
    [Header("Building Info")]
    public string buildingName;
    [TextArea]
    public string description;
    public int durability;

    public Resource upgradeCost;

    [Header("Resources")]
    public ResourceOverTime[] resources;

}