using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMoney : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyText;

    void Update()
    {
        moneyText.text = GameManager.Values[GameManager.Value.Money].ToString();
    }
}
