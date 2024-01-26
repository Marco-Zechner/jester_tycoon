using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "JesterTycoon/Building", order = 0)]
public class SOBuilding : ScriptableObject
{
    public string buildingName;
    public GameObject[] buildingPrefabs;
    public int initialBuildCost;
    public int[] upgradeCost;
}
