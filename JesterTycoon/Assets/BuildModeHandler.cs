using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeHandler : MonoBehaviour
{
    public BuildingManager buildModeObject;
    public GameObject selectionModeObject;

    public void SetBuildMode(bool buildMode)
    {
        buildModeObject.SetBuildMode(buildMode);
        selectionModeObject.SetActive(!buildMode);
    }
}
