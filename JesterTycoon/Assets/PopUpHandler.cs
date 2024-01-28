using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpHandler : MonoBehaviour
{
     public static PopUpHandler instance;
     public PopUpUI popUpAnswer;
     public PopUpUI popUpQuestion;
     PopUpGenerator popUpGenerator;

    private void Awake()
    {
        Debug.Log("hello World");
        instance = this;
        popUpGenerator = GetComponent<PopUpGenerator>();
        popUpAnswer = new PopUpUI(transform.GetChild(0).GetChild(0));
        popUpQuestion = new PopUpUI(transform.GetChild(0).GetChild(1));
    }
    public void HidePopUp()
    {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ShowQuestionPopUp()
    {

        Time.timeScale = 0;
        popUpQuestion.Update( popUpGenerator.Questions[Random.Range(0,popUpGenerator.Questions.Count)],true);
        popUpQuestion.PopUpObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ShowAnswerPopUp(TextMeshProUGUI text)
    {

        Time.timeScale = 0;
        PopUp popup = popUpGenerator.Answer[text.text];
        popUpAnswer.Update(popup,false);
        popUpAnswer.PopUpObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        GameManager.checkValue(popup.Value, popup.amount,out int returnamount);
        Debug.Log(popup.Value + returnamount+"/"+popup.amount);
    }
}
public struct PopUpUI
{
    List<Button> buttons;
    List<TextMeshProUGUI> Answers;
    TextMeshProUGUI text;
    public GameObject PopUpObject;
    public PopUpUI(Transform popup)
    {
        PopUpObject = popup.gameObject; 
        buttons = new List<Button>();
        Answers = new List<TextMeshProUGUI>();
        text = popup.GetChild(0).GetComponent<TextMeshProUGUI>();
        for (int i = 1; i < popup.childCount; i++)
        {
            buttons.Add(popup.GetChild(i).GetComponent<Button>());
            Answers.Add(popup.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }
    
    public void Update(PopUp popUp,bool hasbutton) 
    {
        text.text = popUp.story;
        if(!hasbutton) { return; }
        for (int i = 0; i < Answers.Count; i++)
        {
            Answers[i].text = popUp.Answer[i];
        }
    }
}