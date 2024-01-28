using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("Tabs")]
    public GameObject buildingTab;
    public GameObject infoTab;

    [Header("UI Elements - Building Tab")]
    public List<SOBuilding> availableBuildings;
    public UnityEvent<SOBuilding> aktivebuilding;
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
    public TMP_Text NextEnergy;
    public TMP_Text NextFood;
    public TMP_Text NextMoney;
    public TMP_Text NextLaughs;

    [Header("Upgrade")]
    public TMP_Text upgradeCost;
    public Button upgradeButton;
    public GameObject NextEnergyArrowUp;
    public GameObject NextFoodArrowUp;
    public GameObject NextMoneyArrowUp;
    public GameObject NextLaughsArrowUp;
    public GameObject NextEnergyArrowDown;
    public GameObject NextFoodArrowDown;
    public GameObject NextMoneyArrowDown;
    public GameObject NextLaughsArrowDown;
    public Button Sell;

    public Button showBuildingTab;
    public Button toggleBuildMode;

    [Header("UI Events")]
    [HideInInspector]
    public UnityEvent OnUpgrade;
    [HideInInspector]
    public UnityEvent OnSell;
    public UnityEvent<bool> OnBuildModeChanged;
    private bool buildMode = false;

    void Start()
    {
        Sell.onClick.AddListener(() => OnSell.Invoke());
        upgradeButton.onClick.AddListener(() => {
            OnUpgrade.Invoke();
            DisplayInfo(currentSpace);
        });
        showBuildingTab.onClick.AddListener(() => {
            buildingTab.SetActive(!buildingTab.activeSelf);
            if (buildingTab.activeSelf)
            {
                OnBuildModeChanged.Invoke(false);
                buildMode = false;
                toggleBuildMode.interactable = false;
            }
        });
        toggleBuildMode.onClick.AddListener(() => {
            buildMode = !buildMode;
            OnBuildModeChanged.Invoke(buildMode);
        });
    }

    public void HideInfo()
    {
        infoTab.SetActive(false);
    }

    public void HideBuildingTab()
    {
        buildingTab.SetActive(false);
        toggleBuildMode.interactable = true;
    }

    private PlaceInfo currentSpace;

    public void DisplayInfo(PlaceInfo space)
    {
        currentSpace = space;
        if (currentSpace == null) return;
        infoTab.SetActive(true);
        int MoneyDiff = currentSpace.GetUpgradedValueOfType(ResourceType.Money) - currentSpace.GetValueOfType(ResourceType.Money);
        int FoodDiff = currentSpace.GetUpgradedValueOfType(ResourceType.Food) - currentSpace.GetValueOfType(ResourceType.Food);
        int LaughsDiff = currentSpace.GetUpgradedValueOfType(ResourceType.Laughs) - currentSpace.GetValueOfType(ResourceType.Laughs);
        int EnergyDiff = currentSpace.GetUpgradedValueOfType(ResourceType.Energy) - currentSpace.GetValueOfType(ResourceType.Energy);


        buildingName.text = currentSpace.buildingInfo.buildingName;
        description.text = currentSpace.buildingInfo.description;
        ResourceMoney.text = "Money: " + currentSpace.GetValueOfType(ResourceType.Money);
        ResourceEnergy.text = "Energy: " + currentSpace.GetValueOfType(ResourceType.Energy);
        ResourceLaughs.text = "Laughs: " + currentSpace.GetValueOfType(ResourceType.Laughs);
        ResourceFood.text = "Food: " + currentSpace.GetValueOfType(ResourceType.Food);
        Sell.GetComponentInChildren<TMP_Text>().text = currentSpace.SellValue().ToString();
        NextEnergy.text = currentSpace.GetUpgradedValueOfType(ResourceType.Energy).ToString();
        NextMoney.text = currentSpace.GetUpgradedValueOfType(ResourceType.Money).ToString();
        NextLaughs.text = currentSpace.GetUpgradedValueOfType(ResourceType.Laughs).ToString();
        NextFood.text = currentSpace.GetUpgradedValueOfType(ResourceType.Food).ToString();
        
        

        if (currentSpace.HasUpgrade())
        {
            int cost = currentSpace.UpgradeCost();

            int leftOver = 0;
            if (GameManager.checkValue(GameManager.Value.Money, -cost, out leftOver))
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }

            upgradeCost.text = currentSpace.UpgradeCost().ToString();
        }
        else
        {
            upgradeButton.interactable = false;
            upgradeCost.text = "Max Lvl";
        }

        NextMoneyArrowDown.SetActive(MoneyDiff < 0);
        NextMoneyArrowUp.SetActive(MoneyDiff > 0);
        NextEnergyArrowDown.SetActive(EnergyDiff < 0);
        NextEnergyArrowUp.SetActive(EnergyDiff > 0);
        NextFoodArrowDown.SetActive(FoodDiff< 0);
        NextFoodArrowUp.SetActive(FoodDiff > 0);
        NextLaughsArrowDown.SetActive(LaughsDiff < 0);
        NextLaughsArrowUp.SetActive(LaughsDiff > 0);
    }

    public void SetSellButton(bool active)
    {
        Sell.interactable = active;
    }

    public void Setaktivebuilding(SOBuilding building)
    {
        aktivebuilding.Invoke(building);
        HideBuildingTab();
        OnBuildModeChanged.Invoke(true);
        buildMode = true;
    }

}