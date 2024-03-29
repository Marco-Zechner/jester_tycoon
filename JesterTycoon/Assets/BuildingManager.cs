using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildings = new();

    [SerializeField]
    private List<SOBuilding> buildingSOs = new();

    [SerializeField]
    private int currentBuildingIndex = 0;
    private GameObject currentGround;
    private GameObject currentBuilding;
    private PlaceCollisionChecker collisionChecker;
    private Vector3 currentBuildingPosition;
    private float currentBuildingRotation = 0;
    [SerializeField]
    private GameObject ObjectHolder;

    [SerializeField]
    private GameObject roadPrefab;
    [SerializeField]
    private GameObject groundPrefab;
    private Controls controls;

    [SerializeField]
    private float rotationSpeed = 10f;

    [Header("Map")]
    [SerializeField] [OnValueChanged("UpdateMap")]
    private Vector2 gameMapSize = new(100, 100);
    [SerializeField]
    private Transform mapPlane;
    [SerializeField]
    private LayerMask groundLayer;

    private UIManager uiManager;

    private bool buildModeActive = false;
    public bool active2 = true;

    public List<PlaceInfo> GetAllPlaces()
    {
        List<PlaceInfo> places = new List<PlaceInfo>();
        foreach (var building in buildings)
        {
            if (building == null) continue;
            places.Add(building.GetComponent<PlaceInfo>());
        }
        return places;
    }

    void Start()
    {
        
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (uiManager != null)
        {
            uiManager.aktivebuilding.AddListener(OnSelectBuilding);
        }
        else
        {
            Debug.LogWarning("UIManager not found. This is a fallback system. Use new System instead with the UIManager");
        }
    }

    void OnEnable()
    {
        controls ??= new Controls();
        controls.Enable();

        UpdateMap();

        //SelectBuilding(currentBuildingIndex);

    }

    void OnDisable()
    {
        controls.Disable();
    }

    private void UpdateMap()
    {
        mapPlane.localScale = new Vector3(gameMapSize.x / 10, 1, gameMapSize.y / 10);
    }

    public void SetBuildMode(bool buildMode)
    {
        buildModeActive = buildMode;
    }

    private void Update() {
        if (currentGround == null) return;

        if (buildModeActive == false ||active2 == false)
        {
            currentBuildingPosition = new Vector3(0, -100, 0);
            currentGround.transform.SetPositionAndRotation(currentBuildingPosition, Quaternion.Euler(0, currentBuildingRotation, 0));
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(controls.placeMap.Position.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
        {
            currentBuildingPosition = new Vector3(hit.point.x, 0, hit.point.z);
            currentBuildingPosition.x = Mathf.Clamp(currentBuildingPosition.x, - gameMapSize.x / 2, gameMapSize.x / 2);
            currentBuildingPosition.z = Mathf.Clamp(currentBuildingPosition.z, -gameMapSize.y / 2, gameMapSize.y / 2);
        }
        else
        {
            currentBuildingPosition = new Vector3(0, -100, 0);
            currentGround.transform.SetPositionAndRotation(currentBuildingPosition, Quaternion.Euler(0, currentBuildingRotation, 0));
            return;
        }

        currentBuildingRotation += controls.placeMap.Rotate.ReadValue<float>() * Time.deltaTime * rotationSpeed;

        currentGround.transform.SetPositionAndRotation(currentBuildingPosition, Quaternion.Euler(0, currentBuildingRotation, 0));


        if (controls.placeMap.Place.triggered && collisionChecker.isOccupied == false)
        {
            PlaceBuilding();
        }
    }

    [Button]
    public void SelectBuilding(int index)
    {
        if (uiManager != null)
        {
            Debug.LogWarning("UIManager found. Use new System instead");
            return;
        }
        Debug.LogWarning("UIManager not found. This is a fallback system. Use new System instead with the UIManager");

        if (currentGround == null)
        {
            currentGround = Instantiate(groundPrefab, transform);
            collisionChecker = currentGround.GetComponentInChildren<PlaceCollisionChecker>();
            collisionChecker.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        currentBuildingIndex = index;
        if (currentBuilding != null)
            Destroy(currentBuilding);
        currentBuilding = Instantiate(buildingSOs[index].stages[0].prefab);
        currentBuilding.transform.parent = currentGround.transform;
        currentBuilding.transform.localPosition = Vector3.zero;
        currentBuilding.transform.localRotation = Quaternion.identity;

    }

    private SOBuilding selectedBuilding;

    public void OnSelectBuilding(SOBuilding selectedBuilding)
    {
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager not found");
            return;
        }

        if (currentGround == null)
        {
            currentGround = Instantiate(groundPrefab, transform);
            collisionChecker = currentGround.GetComponentInChildren<PlaceCollisionChecker>();
            collisionChecker.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        currentBuildingIndex = -1;
        if (currentBuilding != null)
            Destroy(currentBuilding);
        currentBuilding = Instantiate(selectedBuilding.stages[0].prefab);
        currentBuilding.transform.parent = currentGround.transform;
        currentBuilding.transform.localPosition = Vector3.zero;
        currentBuilding.transform.localRotation = Quaternion.Euler(-90,0,0);

        this.selectedBuilding = selectedBuilding;
    }

    public void PlaceBuilding()
    {
        var buildingGround = Instantiate(groundPrefab, currentBuildingPosition, Quaternion.Euler(0, currentBuildingRotation, 0), ObjectHolder.transform);
        buildingGround.GetComponent<PlaceInfo>().model = Instantiate(currentBuilding, currentBuildingPosition, Quaternion.Euler(-90, currentBuildingRotation, 0), buildingGround.transform);        

        if(uiManager != null)
        {
            buildingGround.GetComponent<PlaceInfo>().buildingInfo = selectedBuilding;
        }
        else
        {
            Debug.LogWarning("UIManager not found. This is a fallback system. Use new System instead with the UIManager");
            buildingGround.GetComponent<PlaceInfo>().buildingInfo = buildingSOs[currentBuildingIndex];
        }

        int cost = buildingGround.GetComponent<PlaceInfo>().buildingInfo.stages[0].cost.amount;
        if (GameManager.GetValue(GameManager.Value.Money) < cost)
        {
            Destroy(buildingGround);
            return;
        }
        GameManager.AddValue(GameManager.Value.Money, -cost);

        buildings.Add(buildingGround);
        ConnectClosest(buildingGround);
    }

    public void ConnectClosest(GameObject buildingGround)
    {
        GameObject closestBuilding = null;
        float distance = float.MaxValue;
        for (int i = buildings.Count - 1; i >= 0; i--)
        {
            if (buildings[i] == null)
            {
                buildings.RemoveAt(i);
            }
        }

        foreach (var otherBuilding in buildings)
        {
            if (otherBuilding == buildingGround)
            {
                continue;
            }

            float currentDistance = Vector3.Distance(buildingGround.transform.position, otherBuilding.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closestBuilding = otherBuilding;
            }
        }

        PlaceInfo other = closestBuilding.GetComponent<PlaceInfo>();
        PlaceInfo current = buildingGround.GetComponent<PlaceInfo>();
        other.childPlaces.Add(current);
        current.parentPlace = other;

        GameObject road = Instantiate(roadPrefab, buildingGround.transform.position, Quaternion.identity, buildingGround.transform);
        road.transform.localScale = new Vector3(1, 1, distance);
        road.transform.LookAt(closestBuilding.transform);
        road.transform.position = (buildingGround.transform.position + closestBuilding.transform.position) / 2;
    }
}
