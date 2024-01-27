using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RaycastSelector : MonoBehaviour
{
    private Controls controls;
    private Camera playerCamera;

    [SerializeField]
    private float maxRaycastDistance = 1000f;

    [SerializeField]
    private LayerMask selectionMask;

    [Header("Events")]
    public UnityEvent onSelectEmpty;
    public UnityEvent<BuildingInfo> onSelectBuilding;
    public UnityEvent onUnselect;

    void OnEnable()
    {
        controls ??= new Controls();
        controls.Enable();
        playerCamera = GetComponent<Camera>();

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
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, selectionMask))
        {
            ValidSelection(hit);
            return;
        }
        onUnselect.Invoke();
    }

    public void ValidSelection(RaycastHit hit)
    {
        BuildingSpace placeSpace = hit.collider.GetComponentInParent<BuildingSpace>();
        if (placeSpace != null)
        {
            if (placeSpace.isOccupied)
            {
                onSelectBuilding.Invoke(placeSpace.buildingInfo);
            }
            else
            {
                onSelectEmpty.Invoke();
            }
        }
    }
}
