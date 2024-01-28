using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public static class GameManager
{
    public enum Value { Money, Laughs, Food, Power, Visitors };
    public static Dictionary<Value,int> Values = new Dictionary<Value, int>();

    public static void start(int money,int MultieplierConserionRate,float PopUpDelay,MonoBehaviour Mono)
    {
        Values.Add(Value.Money,money);
        Values.Add(Value.Laughs, 0);
        Values.Add(Value.Food, 0);
        Values.Add(Value.Power, 0);
        Values.Add(Value.Visitors, 0);
        if (PopUpHandler.instance == null)
        {
            Debug.LogError("PopUpHandler is not in the scene");
        }
        else
        {
            Mono.StartCoroutine(EventPrompt(PopUpDelay,Mono));  
        }
    }

    public static bool checkValue(Value valueType, int offset,out int ReturnAmount)
    {
        int valuetocheck = Values[valueType];

        if(valuetocheck - offset < 0) 
        {
            ReturnAmount = 0;
            return false;
        }
        Values[valueType] = valuetocheck -offset;
        ReturnAmount = Values[valueType];
        return true;
    }

    static IEnumerator EventPrompt(float delay,MonoBehaviour mono)
    {
        yield return new WaitForSeconds(delay);
        PopUpHandler.instance.ShowPopUp();

        mono.StartCoroutine(EventPrompt(delay,mono));
    }
}