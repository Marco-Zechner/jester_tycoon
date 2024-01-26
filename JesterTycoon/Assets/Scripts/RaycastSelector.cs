using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastSelector : MonoBehaviour
{
    private Camera playerCamera;

    [SerializeField]
    private float maxRaycastDistance = 1000f;

    [SerializeField]
    private LayerMask selectionMask;

    [Header("Events")]
    public UnityEvent<BuildingSpace> onSelectPlaceSpace;
    public UnityEvent onUnselect;

    void OnEnable()
    {
        playerCamera = GetComponent<Camera>();
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
        BuildingSpace placeSpace = hit.collider.GetComponent<BuildingSpace>();
        if (placeSpace != null)
        {
            onSelectPlaceSpace.Invoke(placeSpace);
        }
    }
}
