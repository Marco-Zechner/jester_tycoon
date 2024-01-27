using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "JesterTycoon/Building", order = 0)]
public class SOBuilding : ScriptableObject
{
    [Header("Building Info")]
    public string buildingName;
    [HideLabel]
    [MultiLineProperty(10)]
    public string description;
    public ResourceOverTime[] constantResources;
    [Header("Building Stages")]
    public Stage[] stages;
}

[System.Serializable]
public class Stage {
    public Resource cost;
    public GameObject prefab;
    [Header("Resources")]
    public ResourceOverTime[] resources;
}
