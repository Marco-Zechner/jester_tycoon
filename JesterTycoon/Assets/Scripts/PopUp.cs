using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PopUp 
{
        public bool hasbuttons;
        public string story;
        public List<string> Answer;
        int indexoffset;
        public PopUp(string pagetext)
        {
            story = "";
            Answer = new List<string>();
            indexoffset = 0;
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
                story += pagetext[i];
            }
            hasbuttons = false;
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
                            hasbuttons = true;
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
