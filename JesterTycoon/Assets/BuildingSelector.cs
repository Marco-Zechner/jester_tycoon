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

    private PlaceInfo selectedPlaceInfo;

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
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found");
            return;
        }

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
        selectedPlaceInfo = info;

        uiManager.DisplayInfo(info);
        uiManager.SetSellButton(info.childPlaces.Count == 0);
        uiManager.OnUpgrade.RemoveAllListeners();
        uiManager.OnUpgrade.AddListener(() => selectedPlaceInfo.CurrentStage++);

        uiManager.OnSell.RemoveAllListeners();
        uiManager.OnSell.AddListener(() => {
            if (selectedPlaceInfo.parentPlace != null)
            {
                selectedPlaceInfo.parentPlace.childPlaces.Remove(selectedPlaceInfo);
            }
            Destroy(selectedPlaceInfo.gameObject);
            uiManager.HideInfo();
        });
    }
}
