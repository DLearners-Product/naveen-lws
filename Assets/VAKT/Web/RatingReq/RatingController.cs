using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RatingController : MonoBehaviour
{
  
    public string URL;
    public GameObject G_levelComplete;



    public int I_selectedStar;
    public string STR_feedback;
    public GameObject[] GA_coloredStars;
    public InputField IF_feedback;
    public GameObject G_errorMSG;
    public Button _submitButton;



    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
URL = "http://dlearners.in/template_and_games/Game_template_api-s/update_session_score.php"; // PRODUCTION
#elif UNITY_WEBGL
        URL = "https://dlearners.in/template_and_games/Game_template_api-s/update_session_score.php"; // PRODUCTION
#endif
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
        }
    }
}
