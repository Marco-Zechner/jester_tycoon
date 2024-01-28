using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventRaycast : MonoBehaviour
{
    [SerializeField]
    private BuildingManager buildingManager;
    [SerializeField]
    private GameObject propUp;

    // Update is called once per frame
    void Update()
    {
        buildingManager.active2 = !propUp.activeSelf;
    }
}
