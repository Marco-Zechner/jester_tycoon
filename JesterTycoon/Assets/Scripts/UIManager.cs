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



    public void DisplayInfo(PlaceInfo space)
    {
        if (space == null) return;
        int MoneyDiff = space.getUpgradedValueOfType(ResourceType.Money) - space.getValueOfType(ResourceType.Money);
        int FoodDiff = space.getUpgradedValueOfType(ResourceType.Food) - space.getValueOfType(ResourceType.Food);
        int LaughsDiff = space.getUpgradedValueOfType(ResourceType.Laughs) - space.getValueOfType(ResourceType.Laughs);
        int EnergyDiff = space.getUpgradedValueOfType(ResourceType.Energy) - space.getValueOfType(ResourceType.Energy);


        buildingName.text = space.buildingInfo.buildingName;
        description.text = space.buildingInfo.description;
        ResourceMoney.text = "Money: " + space.getValueOfType(ResourceType.Money);
        ResourceEnergy.text = "Energy: " + space.getValueOfType(ResourceType.Energy);
        ResourceLaughs.text = "Laughs: " + space.getValueOfType(ResourceType.Laughs);
        ResourceFood.text = "Food: " + space.getValueOfType(ResourceType.Food);
        Sell.GetComponentInChildren<TMP_Text>().text = space.sellValue().ToString();
        NextEnergy.text = space.getUpgradedValueOfType(ResourceType.Energy).ToString();
        NextMoney.text = space.getUpgradedValueOfType(ResourceType.Money).ToString();
        NextLaughs.text = space.getUpgradedValueOfType(ResourceType.Laughs).ToString();
        NextFood.text = space.getUpgradedValueOfType(ResourceType.Food).ToString();
        
        

        if (space.hasUpgrade())
        {
            int cost = space.UpgradeCost();

            int leftOver = 0;
            if (GameManager.checkValue(GameManager.Value.Money, -cost, out leftOver))
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }

            upgradeCost.text = space.UpgradeCost().ToString();
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

}