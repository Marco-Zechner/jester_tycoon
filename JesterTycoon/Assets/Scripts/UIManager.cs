using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    [Header("Tabs")]
    public GameObject buildingTab;
    public GameObject infoTab;

    [Header("UI Elements - Building Tab")]
    public TMP_Text placeDescription;
    public List<SOBuilding> availableBuildings;
    public GameObject ListElementPrefab;
    public Transform ListElementParent;

    [Header("UI Elements - Info Tab")]
    public TMP_Text buildingName;
    public Transform durabilityBar;
    
    public TMP_Text description;
    [Header("Resources")]
    public TMP_Text ResourceEnergy;
    public TMP_Text ResourceFood;
    public TMP_Text ResourceMoney;
    public TMP_Text ResourceLaughs;

    [Header("Upgrade")]
    public TMP_Text upgradeCost;
    public Button upgradeButton;

    public void DisplayInfo(BuildingSpace space)
    {
        if (space == null) return;

        buildingTab.SetActive(space.isOccupied);
        infoTab.SetActive(!space.isOccupied);

        buildingName.text = space.buildingInfo.buildingName;
        description.text = space.buildingInfo.description;
        ResourceMoney.text = space.getValueOfType(ResourceType.Money) + " Money";
    }
}
