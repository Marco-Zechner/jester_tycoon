using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateValue : MonoBehaviour
{
    [SerializeField]
    private TMP_Text valueText;

    [SerializeField]
    private GameManager.Value valueType;

    void Update()
    {
        valueText.text = GameManager.Values[valueType].ToString();
    }
}
