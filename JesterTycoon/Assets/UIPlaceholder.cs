using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlaceholder : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private Material selectedMaterial;

    private BuildingSpace buildingSpace;

    public void Select(BuildingSpace placeSpace)
    {
        if (buildingSpace != null)
        {
            buildingSpace.GetComponentInChildren<Renderer>().material = material;
        }

        buildingSpace = placeSpace;
        Debug.Log("Selected " + placeSpace.name);

        buildingSpace.GetComponentInChildren<Renderer>().material = selectedMaterial;
    }

    public void Unselect()
    {
        if (buildingSpace != null)
        {
            buildingSpace.GetComponentInChildren<Renderer>().material = material;
        }
    }
}
