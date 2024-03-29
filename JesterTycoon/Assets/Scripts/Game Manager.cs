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
            Debug.LogWarning("PopUpHandler is not in the scene");
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

    public static void AddValue(Value valueType, int offset)
    {
        int valuetocheck = Values[valueType];
        Values[valueType] = valuetocheck + offset;
    }

    public static int GetValue(Value valueType)
    {
        return Values[valueType];
    }

    static IEnumerator EventPrompt(float delay,MonoBehaviour mono)
    {
        yield return new WaitForSeconds(delay);
        PopUpHandler.instance.ShowQuestionPopUp();

        mono.StartCoroutine(EventPrompt(delay,mono));
    }
}