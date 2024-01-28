using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class PopUpGenerator : MonoBehaviour
{
    public List<PopUp> Questions = new List<PopUp>();
    public Dictionary<string, PopUp> Answer = new Dictionary<string, PopUp>();
    public List<string> QuestionsTitles = new List<string>();
    public List<string> AnswerTitles = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

        string path = @"Assets/Events.twee";
        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            Debug.Log("Error: story could not be found" + System.IO.Path.GetFullPath(path));
            return;
        }

        // Open the file to read from.
        string input = File.ReadAllText(path);
        int offset = 1;

        // this cuts out the first to parts that are useless for us  
        for (int i = 0; i < input.Length; i++) { if (input[i] == '}') { offset = i; break; } }
        // sets the pages 
        for (int i = offset; i < input.Length; i++)
        {
            // finds the start of the page
            if ("" + input[i - 1] + input[i] == "::")
            {
                string text = "";
                string title = "";
                // gets the title of the page
                for (int t = i + 2; t < input.Length; t++)
                {
                    offset = t;
                    if (input[t + 1] == '{') { break; }
                    title += input[t];
                }
                // gets the text of the page
                for (int j = i + 1; j < input.Length; j++)
                {

                    text += input[j];
                    // finds the ende of the page
                    if ("" + input[j - 1] + input[j] == "::" || j == input.Length - 1)
                    {
                        i = j - 1;

                        PopUp newpopup = new PopUp(text);
                        if (title[0] == '#')
                        {
                            Questions.Add(newpopup);
                            QuestionsTitles.Add(title);
                        }
                        else
                        {
                            Answer.Add(title, newpopup);
                            AnswerTitles.Add(title);
                        }
                        break;
                    }
                }
            }

        }
    }
}
