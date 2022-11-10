using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

public class clickText : MonoBehaviour, IPointerClickHandler
{
    public string[] defaultWords;
    public string[] splitWords;

    void Start()
    {
        Invoke("THI_addLinks", 2f);
    }
    void THI_addLinks()
    {
        splitWords = GetComponent<TextMeshProUGUI>().text.Split(' ');
        defaultWords = splitWords;
        for (int i = 0; i < splitWords.Length; i++)
        {
            splitWords[i] = "<link =" + splitWords[i] + ">" + splitWords[i] + "</link>";
        }
        GetComponent<TextMeshProUGUI>().text = string.Join(" ", splitWords);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        PassageClickManager.instance.STR_clickedWord = GetComponent<TextMeshProUGUI>().textInfo.linkInfo[TMP_TextUtilities.FindIntersectingLink(GetComponent<TextMeshProUGUI>(), Input.mousePosition, Camera.main)].GetLinkText();         
  
        THI_Wordvalidation();
       THI_highlightWord();
    }
    void THI_Wordvalidation()
    {
        for (int i = 0; i < PassageClickManager.instance.STRA_specialCharacters.Length; i++)
        {
            if (PassageClickManager.instance.STR_clickedWord.Contains(PassageClickManager.instance.STRA_specialCharacters[i]))
            {
                PassageClickManager.instance.STR_clickedWord = PassageClickManager.instance.STR_clickedWord.Replace(PassageClickManager.instance.STRA_specialCharacters[i], "");
            }
        }
    }
    void THI_highlightWord()
    {
        if (PassageClickManager.instance.STR_clickedWord == PassageClickManager.instance.STRL_answers[PassageClickManager.instance.I_qCount])
        {
            //correct
            for (int j = 0; j < splitWords.Length; j++)
            {
                if (splitWords[j].Contains(PassageClickManager.instance.STR_clickedWord))
                {
                    splitWords[j] = defaultWords[j];
                    splitWords[j] = "<color=green>" + splitWords[j] + "</color>";
                }
            }
            PassageClickManager.instance.AS_coin.Play();
            PassageClickManager.instance.I_points = PassageClickManager.instance.I_points + PassageClickManager.instance.I_correctPoints;
            PassageClickManager.instance.TEX_points.text = PassageClickManager.instance.I_points.ToString();
            GetComponent<TextMeshProUGUI>().text = string.Join(" ", splitWords);
            PassageClickManager.instance.THI_showQuestion();
           // Invoke("THI_passageQuesInv", 1f);
           
        }
        else
        {
            //wrong
            if (PassageClickManager.instance.I_points >= PassageClickManager.instance.I_wrongPoints)
            {
                PassageClickManager.instance.I_points = PassageClickManager.instance.I_points - PassageClickManager.instance.I_wrongPoints;
            }
            else
            {
                PassageClickManager.instance.I_points = 0;
            }
            PassageClickManager.instance.TEX_points.text = PassageClickManager.instance.I_points.ToString();

            for (int j = 0; j < splitWords.Length; j++)
            {
                if (splitWords[j].Contains(PassageClickManager.instance.STR_clickedWord))
                {
                    splitWords[j] = defaultWords[j];
                    splitWords[j] = "<color=red>" + splitWords[j] + "</color>";
                }
            }
            PassageClickManager.instance.AS_wrong.Play();
            GetComponent<TextMeshProUGUI>().text = string.Join(" ", splitWords);
        }

       
    }

    void THI_passageQuesInv()
    {
        PassageClickManager.instance.G_passageQuestion.GetComponent<Animator>().Play("question inv");
        PassageClickManager.instance.THI_showQuestionDelay();
    }
}