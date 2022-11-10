using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LWS_RatingController : MonoBehaviour
{
    public string URL;
    public GameObject G_levelComplete;



    public int I_selectedStar;
    public string STR_feedback;
    public GameObject[] GA_coloredStars;
    public InputField IF_feedback;
    public GameObject G_errorMSG;
    public Button _submitButton;
    public GameObject G_emoji;


    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
URL = "http://dlearners.in/template_and_games/Game_template_api-s/update_session_score.php"; // PRODUCTION
#elif UNITY_WEBGL
        URL = "https://dlearners.in/template_and_games/Game_template_api-s/update_session_score.php"; // PRODUCTION
#endif
    }

    public void Start()
    {
        G_emoji= G_levelComplete.transform.GetChild(1).transform.GetChild(3).gameObject;
    }
    public void IF_feedbackEndEdit()
    {
        STR_feedback = IF_feedback.text;
    }

    public void BUT_ClickStar()
    {
        I_selectedStar = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        for (int i = 0; i < GA_coloredStars.Length; i++)
        {
            GA_coloredStars[i].SetActive(false);
        }
        for (int i = 0; i < I_selectedStar; i++)
        {
            GA_coloredStars[i].SetActive(true);
        }
    }
    public void BUT_submit()
    {
        if (I_selectedStar != 0)
        {
            _submitButton.enabled = false;
            StartCoroutine(EN_storeDataInDB());
        }
        else
        {
            G_errorMSG.SetActive(true);
            _submitButton.enabled = false;
            Invoke("THI_disableErrorMsg", 2f);
        }
    }
    public void THI_disableErrorMsg()
    {
        _submitButton.enabled = true;
        G_errorMSG.SetActive(false);
    }

    IEnumerator EN_storeDataInDB()
    {

        WWWForm form = new WWWForm();
        form.AddField("score", MainController.instance.I_TotalPoints.ToString());
        form.AddField("rating", I_selectedStar.ToString());
        form.AddField("comments", IF_feedback.text);
        form.AddField("Si_no", MainController.instance.STR_responseSerial);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            _submitButton.enabled = true;
        }
        else
        {
            G_levelComplete.SetActive(true);
          //  TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;
            THI_checkEmojiResult();
        }
    }


    void THI_checkEmojiResult()
    {
        float perfect = MainController.instance.I_TotalQuestions;
        float wow = 0.75f * MainController.instance.I_TotalQuestions;
        float superb = 0.5f * MainController.instance.I_TotalQuestions;
        float goodjob = 0.25f * MainController.instance.I_TotalQuestions;

        Debug.Log("perfect = " + perfect);
        Debug.Log("wow = " + wow);
        Debug.Log("superb = " + superb);
        Debug.Log("goodjob = " + goodjob);
        Debug.Log("Answer = " + MainController.instance.I_correctPoints);

        if (MainController.instance.I_correctPoints == perfect)
        {
           // TEX_emojiResult.text = "Perfect!";
            G_emoji.GetComponent<Animator>().Play("perfect");
            Debug.Log("Answer = " + MainController.instance.I_correctPoints);
        }
        if (MainController.instance.I_correctPoints < perfect && MainController.instance.I_correctPoints >= wow)
        {
           // TEX_emojiResult.text = "Wow!";
            G_emoji.GetComponent<Animator>().Play("wow");
        }
        if (MainController.instance.I_correctPoints < wow && MainController.instance.I_correctPoints >= superb)
        {
           // TEX_emojiResult.text = "Superb!";
            G_emoji.GetComponent<Animator>().Play("super");
        }
        if (MainController.instance.I_correctPoints < superb && MainController.instance.I_correctPoints >= goodjob)
        {
          //  TEX_emojiResult.text = "Good Job!";
            G_emoji.GetComponent<Animator>().Play("goodjob");
        }
        if (MainController.instance.I_correctPoints < goodjob)
        {
           // TEX_emojiResult.text = "Can do better!";
            G_emoji.GetComponent<Animator>().Play("candobetter");
        }
       // TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;
    }
}