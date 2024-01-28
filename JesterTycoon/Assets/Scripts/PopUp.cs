using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public struct PopUp 
{
    public string story;
    public List<string> Answer;
    public int indexoffset;
    public GameManager.Value Value;
    public int amount;
    public PopUp(string pagetext)
    {
        story = "";
        Answer = new List<string>();
        indexoffset = 0;
        amount = 0;
        Value = new GameManager.Value();
        int offset = 0;
        // cuts the first line 
        for (int i = 0; i < pagetext.Length; i++)
        {
            if (pagetext[i] == '}')
            {
                offset = i + 1;
                break;
            }
        }
        // gets the story
        for (int i = offset; i < pagetext.Length; i++)
        {
            if (pagetext[i] == '[')
            {
                offset = i;
                break;
            }
            if(pagetext[i] == '<')
            {
                string ValueString = "";
                for (int j = 2; j+i < pagetext.Length; j++)
                {
                    if (pagetext[i+j] == '>') 
                    {
                        i += j;
                        break; 
                    }
                    ValueString += pagetext[i+j];
                }
                for (int j = 0; j < GameManager.Values.Count; j++)
                {
                    if (ValueString == GameManager.Values.ToList()[j].Key.ToString()) 
                    {
                        Value = GameManager.Values.ToList()[j].Key;
                        int ABC = 1;
                        if(pagetext[i + 1] == '+') { ABC = 1; }
                        else{ ABC = -1; }
                        string nummber = "";
                        for (global::System.Int32 k = i+2; i+k < pagetext.Length; k++)
                        {
                            if (pagetext[k] == ')') 
                            {
                                amount = int.Parse(nummber) * ABC;
                                i = k+i;
                                break; 
                            }
                            nummber += pagetext[i + k];
                        }
                        story += (amount).ToString();
                        break;
                    }   
                }



                Debug.Log(ValueString);
            }
            story += pagetext[i];
        }
        // sets the keys and there texts
        for (int i = offset; i < pagetext.Length; i++)
        {
            if ("" + pagetext[i - 1] + pagetext[i] == "[[")
            {
                string key = "";
                for (int j = i + 2; j < pagetext.Length; j++)
                {
    
                    if ("" + pagetext[j - 1] + pagetext[j] == "]]")
                    {
                        Answer.Add(key);
                        break;
                    }
                    key += pagetext[j - 1];
    
                }
            }
            i += indexoffset;
        }
    }
    
    string settext(string pagetext, int start)
    {
        string text = "";
        for (int t = start; t < pagetext.Length; t++)
        {
            switch (pagetext[t])
            {
                case ']':
                    indexoffset += t;
                    return text;
                case '{':
                    indexoffset += t - 1;
                    return text;
                default:
    
                    text += pagetext[t];
                    break;
            }
        }
        return text;
    }
    
}
