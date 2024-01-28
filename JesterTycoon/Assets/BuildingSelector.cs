using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    private Controls controls;
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private float maxRaycastDistance = 1000f;

    [SerializeField]
    private LayerMask buildingLayer;

    private UIManager uiManager;

    void OnEnable()
    {
        controls ??= new Controls();
        controls.Enable();

        controls.selectPlaces.SelectPosition.performed += ctx => Select(ctx.ReadValue<Vector2>());
        controls.selectPlaces.SelectCenter.performed += ctx => Select(new Vector2(Screen.width / 2, Screen.height / 2));
    }

    void OnDisable()
    {
        controls.Disable();
    }

    public void Select(Vector2 screenPosition)
    {
        Ray ray = playerCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, buildingLayer))
        {
            ValidSelection(hit);
            return;
        }
    }

    public void ValidSelection(RaycastHit hit)
    {
        var info = hit.collider.GetComponentInParent<PlaceInfo>();

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (uiManager != null)
        {
            uiManager.DisplayInfo(info);
            uiManager.SetSellButton(info.childPlaces.Count == 0);
        }
    }
}
