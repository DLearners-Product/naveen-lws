using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassageQController : MonoBehaviour
{
    [Header("Passage")]
    public string STR_passage;
    public TextMeshProUGUI TMP_passageText;

    [Header("DB")]
    public List<string> STRL_questions;
    public List<string> STRL_answerKeywords;

    [Header("Gameplay")]
    public int I_questionCount;
    public int I_points;
    public GameObject G_passage;
    public GameObject[] GA_questionsText;
    public GameObject[] GA_questionReqButtons;
    public GameObject G_passageReqButton;
    public GameObject G_levelComplete;
    public Text TEX_questionCount;
    public Text TEX_pointsText;
    public AnimationClip AC_questionExit;

    void Start()
    {
        I_questionCount = 0;
        TEX_questionCount.text = I_questionCount + "/" + STRL_questions.Count;
        THI_controlbuttons(true, false);
        THI_assignVals();
    }

    void THI_assignVals()
    {
        TMP_passageText.text = STR_passage;
        for(int i = 0; i < STRL_questions.Count; i++)
        {
            GA_questionsText[i].GetComponent<TextMeshProUGUI>().text = STRL_questions[i];
        }
    }
    public void BUT_ConfirmPassage()
    {
        G_passage.GetComponent<Animator>().Play("passageExit");
        GA_questionsText[I_questionCount].transform.parent.GetComponent<Animator>().Play("questionEntry");
        THI_controlbuttons(false, true);
    }
    void THI_controlbuttons(bool passage, bool question)
    {
        for (int i = 0; i < GA_questionReqButtons.Length; i++)
        {
            GA_questionReqButtons[i].SetActive(question);
        }
        G_passageReqButton.SetActive(passage);
    }
    public void BUT_backButtonQuestion()
    {
        G_passage.GetComponent<Animator>().Play("passageEntry");
        GA_questionsText[I_questionCount].transform.parent.GetComponent<Animator>().Play("questionExit");
        THI_controlbuttons(true,false);
    }
    public void BUT_nextQuestion()
    {
        GA_questionsText[I_questionCount].transform.parent.GetComponent<Animator>().Play("questionExit");
        THI_awardPoints();
        I_questionCount++;     
        if (I_questionCount < STRL_questions.Count)
        {
            GA_questionsText[I_questionCount].transform.parent.GetComponent<Animator>().Play("questionEntry");
            TEX_questionCount.text = I_questionCount + "/" + STRL_questions.Count;
        }
        else
        {
            THI_controlbuttons(false,false);
            Invoke("THI_levelComplete", AC_questionExit.length);
        }
    }
    void THI_levelComplete()
    {
        G_levelComplete.SetActive(true);
    }
    void THI_awardPoints()
    {
        InputField IF_currentAns = GA_questionsText[I_questionCount].transform.parent.transform.GetChild(1).GetComponent<InputField>();
        string STR_currentAns = IF_currentAns.text.ToLower();
        string STR_correctAns = STRL_answerKeywords[I_questionCount].ToLower();

        if (STR_currentAns.Contains(STR_correctAns)) 
        {
            I_points += 10;
            TEX_pointsText.text = I_points.ToString();
        }
    }

}
