using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public static class GameManager
{
    enum Value { Money, Laughs, Food, Power, Visitors };
    static Dictionary<Value,int> Values = new Dictionary<Value, int>() ;
    public static void start(int money,int MultieplierConserionRate,float PopUpDelay,MonoBehaviour Mono)
    {
        Values.Add(Value.Money,money);
        Values.Add(Value.Laughs, 0);
        Values.Add(Value.Food, 0);
        Values.Add(Value.Power, 0);
        Values.Add(Value.Visitors, 0);
        Mono.StartCoroutine(EventPrompt(PopUpDelay,Mono));
    }

    static bool checkValue(Value valueType, int offset,out int ReturnAmount)
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
        checkValue(Value.Money, 100, out int ReturnAmount);
        mono.StartCoroutine(EventPrompt(delay,mono));
    }
}