using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
public class Sort_DD_Img_Main : MonoBehaviour
{
    public static Sort_DD_Img_Main OBJ_sort_DD_Img_Main;
    public bool B_production;

    [Header("DB")]
    public string URL;
    public string SendValueURL;
    public string STR_childName;
    public string STR_childGrade;
    public string STR_worksheetName;
    public string STR_worksheerInstruction;
    public List<int> IL_numbers;
    public List<string> STRL_details;
    public List<string> STRL_questions;
    public List<string> STRL_answers;
    public List<string> STRL_options;
    public List<string> STRL_difficulty;
    public List<int> IL_numberValues;
    public List<string> STRL_instruction;
    public List<string> STRL_questionID;
    public List<string> STRL_questionAudios;
    public List<string> STRL_optionsAudios;
    public List<string> STRL_instructionAudios;
    public List<string> STRL_cover_img_link;
    public List<string> STRL_passageDetail;
    public string STR_difficulty;
    public string STR_currentQuestionAnswer;


    [Header("Gameplay")]
    public int I_optCountType;
    public int I_questionCount;
    public Text TEX_questionCount;
    public Text TEX_question;
    public string STR_selectedAnswer;
    public string STR_selectedQuestion;
    public GameObject G_errorMSG;
    public Text TEX_worksheetInstruction;
    public Text TEX_childName;
    public Text TEX_childGrade;
    public Text TEX_worksheetName;
    public Color COL_selectedColor;
    public int I_correctAnsCount;
    public GameObject G_levelComplete;
    public Text TEX_resultText;
    public Sprite[] SPRA_Question;

    public GameObject[] GA_Questions;
    public GameObject[] GA_Options;
    GameObject G_QParent;
    public string STR_dummy;

    int  I_Qcount;

    public Text TEX_emojiResult;
    public GameObject G_emoji;
    public GameObject G_coverPage;
    public AudioSource AS_CLick;


    [Header("GAME DATA")]
    public string STR_currentQuestionID;
    public List<string> STRL_gameData;
    public string STR_Data;


    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;


    private void Awake()
    {
        OBJ_sort_DD_Img_Main = this;
        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_2.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA
        }
        else
        {
            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }
    }

    void Start()
    {
        G_QParent = GA_Questions[0].transform.parent.gameObject;
      //  Debug.Log(G_QParent.name);
        G_levelComplete.SetActive(false);
        G_errorMSG.SetActive(false);
        TEX_questionCount.text = "" + I_Qcount + "/" + (GA_Questions.Length);
        THI_gameData();
    }


    public void BUT_Restart()
    {
        I_correctAnsCount = 0;
        I_Qcount = 0;
        TEX_questionCount.text = "" + I_Qcount + "/" + (GA_Questions.Length);

        STRL_gameData = new List<string>();
        STR_Data = "";

        for (int i=0;i<GA_Questions.Length;i++)
        {
            GA_Questions[i].transform.SetParent(G_QParent.transform, false);
            GA_Questions[i].transform.localScale = new Vector2(1f, 1f);
           // GA_Questions[i].GetComponent<Img_drag>().enabled = true;
        }
        for (int i = 0; i < GA_Options.Length; i++)
        {
            for (int k = 1; k < GA_Options[i].transform.parent.childCount; k++)
            {
                GA_Options[i].transform.parent.GetChild(k).GetComponent<Collider2D>().enabled = true;
            }
        }
           
    }

    public void THI_BacktoOriginalpos()
    {

        for (int i = 0; i < GA_Questions.Length; i++)
        {
            if (STR_dummy == GA_Questions[i].name)
            {
                G_QParent.GetComponent<VerticalLayoutGroup>().enabled = false;
                   // Debug.Log("Debug.Log(already available other);");
                   GA_Questions[i].transform.SetParent(G_QParent.transform, false);
                GA_Questions[i].transform.localScale = new Vector2(1f, 1f);
            }
        }
        G_QParent.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    public void THI_Count(bool plus)
    {
        if (plus)
        {
            if (I_Qcount < GA_Questions.Length)
            {
                // Debug.Log("I_Qcount++");
                I_Qcount++;
            }
        }
        else
        {
            if (I_Qcount > 0)
            {
                // Debug.Log("I_Qcount--");
                I_Qcount--;
            }
        }
        //Debug.Log(I_Qcount);
        TEX_questionCount.text = "" + I_Qcount + "/" + (GA_Questions.Length);
        if (G_errorMSG.activeInHierarchy)
            G_errorMSG.SetActive(false);
    }



    void THI_assignValues()
    {
        TEX_childName.text = STR_childName;
        TEX_childGrade.text = "Grade : " + STR_childGrade;
        TEX_worksheetName.text = STR_worksheetName;
        TEX_worksheetInstruction.text = STR_worksheerInstruction;
    }
    public void BUT_Start()
    {
        TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().Play();
        THI_ShowQuestion();
    }

    void THI_ShowQuestion()
    {
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].name = STRL_options[i];
            GA_Options[i].GetComponent<TextMeshProUGUI>().text = STRL_options[i];
            GA_Options[i].GetComponent<AudioSource>().clip = ACA_optionClips[i];
        }
        for (int i = 0; i < GA_Questions.Length; i++)
        {
            GA_Questions[i].name = SPRA_Question[i].name; 
            GA_Questions[i].transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Question[i];
            GA_Questions[i].transform.GetChild(0).name = SPRA_Question[i].name;
            GA_Questions[i].GetComponent<AudioSource>().clip = ACA__questionClips[i];
        }
    }


    public void THI_worksheetComplete()
    {
        AS_CLick.Play();

        STRL_gameData = new List<string>();
        STR_Data = "";

        for (int i = 0; i < GA_Questions.Length; i++)
        {
            // STR_selectedAnswer = this.name;
            STR_currentQuestionID = STRL_questionID[i];
            STR_currentQuestionAnswer = STRL_answers[i];

            if (GA_Questions[i].transform.parent.transform.parent.transform.parent.name =="Drop")
            {

                STR_selectedAnswer = GA_Questions[i].transform.parent.transform.parent.GetChild(0).name;

               // Debug.Log("All : SAns = " + STR_selectedAnswer + ", CAns =" + STR_currentQuestionAnswer);

                if (STR_selectedAnswer == STR_currentQuestionAnswer)
                {
                    Debug.Log("CHECKING : SAns = " + STR_selectedAnswer + ", CAns =" + STR_currentQuestionAnswer);
                    I_correctAnsCount++;
                    THI_TrackGameData("1");
                }
                else
                {
                    Debug.Log("Wrong: SAns = "+ GA_Questions[i].name+" Sans = " + STR_selectedAnswer + ", CAns =" + STR_currentQuestionAnswer);
                    THI_TrackGameData("0");
                }

            }
            else
            {
                G_errorMSG.SetActive(true);
            }
        }


        if (I_Qcount < GA_Questions.Length)
        {
            G_errorMSG.SetActive(true);
        }
        else
        {
            G_levelComplete.SetActive(true);

            TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;

            THI_checkEmojiResult();

            MainController.instance.I_correctPoints = I_correctAnsCount;
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            //TEX_resultText.text = "You have answered " + I_correctAnsCount + " / " + STRL_questions.Count + " questions correctly";
            StartCoroutine(IN_sendDataToDB());
        }
    }

   

    void THI_checkEmojiResult()
    {
        float perfect = STRL_questions.Count;
        float wow = 0.75f * STRL_questions.Count;
        float superb = 0.5f * STRL_questions.Count;
        float goodjob = 0.25f * STRL_questions.Count;

        Debug.Log("perfect = " + perfect);
        Debug.Log("wow = " + wow);
        Debug.Log("superb = " + superb);
        Debug.Log("goodjob = " + goodjob);

        if (I_correctAnsCount == perfect)
        {
            TEX_emojiResult.text = "Perfect!";
            G_emoji.GetComponent<Animator>().Play("perfect");
        }
        if (I_correctAnsCount < perfect && I_correctAnsCount >= wow)
        {
            TEX_emojiResult.text = "Wow!";
            G_emoji.GetComponent<Animator>().Play("wow");
        }
        if (I_correctAnsCount < wow && I_correctAnsCount >= superb)
        {
            TEX_emojiResult.text = "Superb!";
            G_emoji.GetComponent<Animator>().Play("super");
        }
        if (I_correctAnsCount < superb && I_correctAnsCount >= goodjob)
        {
            TEX_emojiResult.text = "Good Job!";
            G_emoji.GetComponent<Animator>().Play("goodjob");
        }
        if (I_correctAnsCount < goodjob)
        {
            TEX_emojiResult.text = "Can do better!";
            G_emoji.GetComponent<Animator>().Play("candobetter");
        }
        TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;
    }
    void THI_gameData()
    {
        // THI_getPreviewData();
        //  THI_nextQuestion();
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(EN_GetData()); // live game in portal
        }
        if (MainController.instance.mode == "preview")
        {
            // preview data in html game generator
            THI_getPreviewData();
        }
        Debug.Log("MODE : " + MainController.instance.mode);
    }


    IEnumerator EN_GetData()
    {
        WWWForm form = new WWWForm();
        form.AddField("game_id", MainController.instance.STR_GameID);


        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //   Debug.Log("GAME DATA: " + www.downloadHandler.text);
            MyJSON json = new MyJSON();
            json.Temp_type_1_LWS(www.downloadHandler.text, IL_numbers, STRL_instruction, STRL_instructionAudios, STRL_questions,
                STRL_answers, STRL_questionAudios, STRL_questionID, STRL_options, STRL_optionsAudios, STRL_details, STRL_cover_img_link, STRL_passageDetail);

            STR_childName = STRL_details[0];
            STR_childGrade = STRL_details[1];
            STR_worksheetName = STRL_details[2];
            STR_worksheerInstruction = STRL_instruction[0];
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(EN_getAudioClips1());
            StartCoroutine(EN_getAudioClips2());
            StartCoroutine(IN_CoverImage());
             StartCoroutine(IN_downloadlImg());
            Debug.Log("live");
            THI_assignValues();
           
        }
    }
    public IEnumerator IN_downloadlImg()
    {
        SPRA_Question = new Sprite[STRL_questions.Count];
        for (int i = 0; i < STRL_questions.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_questions[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (GA_Questions[i] != null)
                {
                   // Debug.Log("Q_Image Asign = "+ STRL_questions[i]);
                    // GA_Questions[i].transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    SPRA_Question[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    string[] Names = (STRL_questions[i].Split('/'));
                    string[] Finalname = (Names[Names.Length - 1].Split('.'));

                   // Debug.Log("Q_Image name = " + Finalname[0]);
                    SPRA_Question[i].name = Finalname[0];
                }



            }
        }
       // THI_ShowQuestion();
    }
    public IEnumerator IN_CoverImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_cover_img_link[0]);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (STRL_cover_img_link != null)
            {
                G_coverPage.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

    }

    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        json.Temp_type_1_LWS(MainController.instance.STR_previewJsonAPI, IL_numbers, STRL_instruction, STRL_instructionAudios, STRL_questions,
            STRL_answers, STRL_questionAudios, STRL_questionID, STRL_options, STRL_optionsAudios, STRL_details, STRL_cover_img_link, STRL_passageDetail);

        STR_childName = STRL_details[0];
        STR_childGrade = STRL_details[1];
        STR_worksheetName = STRL_details[2];
        STR_worksheerInstruction = STRL_instruction[0];

        StartCoroutine(EN_getAudioClips());
        StartCoroutine(EN_getAudioClips1());
        StartCoroutine(EN_getAudioClips2());
        StartCoroutine(IN_CoverImage());
        StartCoroutine(IN_downloadlImg());
        THI_assignValues();
        Debug.Log("Preview");
        // THI_ShowQuestion();
    }

    public void THI_TrackGameData(string analysis)
    {
        DBmanager lws1 = new DBmanager();
        lws1.question_id = STR_currentQuestionID;
        lws1.answer = STR_selectedAnswer;
        lws1.analysis = analysis;


        string toJson = JsonUtility.ToJson(lws1);
        STRL_gameData.Add(toJson);
        STR_Data = string.Join(",", STRL_gameData);
    }

    public IEnumerator IN_sendDataToDB()
    {
        WWWForm form = new WWWForm();
        form.AddField("child_id", MainController.instance.STR_childID);
        form.AddField("game_id", MainController.instance.STR_GameID);



        form.AddField("game_details", "[" + STR_Data + "]");


        Debug.Log("child id : " + MainController.instance.STR_childID);
        Debug.Log("game_id  : " + MainController.instance.STR_GameID);
        Debug.Log("game_details: " + "[" + STR_Data + "]");

        UnityWebRequest www = UnityWebRequest.Post(SendValueURL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Sending data to DB failed : " + www.error);
        }
        else
        {
            MyJSON json = new MyJSON();
            json.THI_onGameComplete(www.downloadHandler.text);
            Debug.Log("Sending data to DB success : " + www.downloadHandler.text);
        }
    }

    public void BUT_confirm()
    {
#if UNITY_ANDROID || UNITY_IOS
Screen.orientation = ScreenOrientation.Portrait;
VAKT_controller.instance.BUT_gameLWSintroBack();
Destroy(VAKT_controller.instance.G_currentActivity);
// app logics will come here
#elif UNITY_WEBGL
        Application.ExternalEval("closeApplication()");
#endif
    }


    public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
      //  ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
      //  ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

        for (int i = 0; i < STRL_questionAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_questionAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {


                ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("audio clips fetched questions");
                //  TEX_question.gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_questionCount];

            }
        }

       

       
    }
    public IEnumerator EN_getAudioClips1()
    {
       // ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
       // ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

      

        for (int i = 0; i < STRL_optionsAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_optionsAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www);


                Debug.Log("audio clips fetched options");

            }
        }


    }
    public IEnumerator EN_getAudioClips2()
    {
      //  ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
      //  ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

       /* for (int i = 0; i < STRL_questionAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_questionAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {


                ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("audio clips fetched questions");
                //  TEX_question.gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_questionCount];

            }
        }

        for (int i = 0; i < STRL_optionsAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_optionsAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www);


                Debug.Log("audio clips fetched options");

            }
        }*/

        for (int i = 0; i < STRL_instructionAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_instructionAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {

                ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];

                Debug.Log("audio clips fetched instruction");

            }
        }
    }
}
