using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Math_type : MonoBehaviour
{
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
    public GameObject G_Question;
    public string STR_selectedAnswer;
    public GameObject G_errorMSG;
    public Text TEX_worksheetInstruction;
    public Text TEX_childName;
    public Text TEX_childGrade;
    public Text TEX_worksheetName;
    public int I_correctAnsCount;
    public GameObject G_levelComplete;
    public GameObject G_Method;
    public Text TEX_resultText;
    public GameObject G_emoji;
    public GameObject G_coverPage;
    public AudioSource AS_CLick;
    public GameObject G_OptionHolder;
    public GameObject G_OptionsLayout;
    public Sprite[] SPRA_Question;
    public Text TEX_emojiResult;
    public GameObject G_MethodButton;
    public Color[] CLR_Colors;
    bool B_Error;
    int I_dummy;

    [Header("GAME DATA")]
    public string STR_currentQuestionID;
    public List<string> STRL_gameData;
    public string STR_Data;


    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;
    public AudioClip[] ACA_MethodClips;


    private void Awake()
    {
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
        G_levelComplete.SetActive(false);
        G_errorMSG.SetActive(false);
        I_questionCount = -1;
        //TEX_questionCount.text = "" + I_questionCount + "/" + (STRL_questions.Count);
        THI_gameData();
    }


    void THI_assignValues()
    {
        TEX_childName.text = STR_childName;
        TEX_childGrade.text = "Grade : " + STR_childGrade;
        TEX_worksheetName.text = STR_worksheetName;
        TEX_worksheetInstruction.text = STR_worksheerInstruction;
    }

     public void THI_Check()
     {
       //  Debug.Log("Checking");
        if (STR_selectedAnswer == STR_currentQuestionAnswer)
        {
            THI_TrackGameData("1");
            I_correctAnsCount++;
        }
        else
        {
            THI_TrackGameData("0");
        }

        MoveNext();


    }

    void MoveNext()
    {
        if (I_questionCount < STRL_questions.Count-1)
        {
            I_questionCount++;
            THI_ShowQuestion();
        }
        else
        {
            G_levelComplete.SetActive(true);
            TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;

            MainController.instance.I_correctPoints = I_correctAnsCount;
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            THI_checkEmojiResult();
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


    public void THI_ShowQuestion()
    {
        if(G_OptionsLayout.transform.childCount!=0)
        {
            for (int i = 0; i < G_OptionsLayout.transform.childCount;i++)
            {
                Destroy(G_OptionsLayout.transform.GetChild(i).gameObject);
            }
        }
        
        
        STR_currentQuestionID = STRL_questionID[I_questionCount];
        STR_currentQuestionAnswer = STRL_answers[I_questionCount];

        int k = I_questionCount + 1;
        TEX_questionCount.text = "" + k + "/" + (STRL_questions.Count);

        G_Question.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Question[I_questionCount];
        G_Question.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
        G_Question.GetComponent<AudioSource>().clip = ACA__questionClips[I_questionCount];

        string[] Answers = STR_currentQuestionAnswer.Split(',');

        for (int i=0;i<STRL_options.Count;i++)
        {
            GameObject G_Dummy = Instantiate(G_OptionHolder);
            G_Dummy.transform.GetChild(0).GetComponent<Text>().text = STRL_options[i];
            G_Dummy.transform.GetChild(1).name = Answers[i];
            G_Dummy.transform.GetChild(1).GetComponent<Image>().color = CLR_Colors[i];
            G_Dummy.transform.SetParent(G_OptionsLayout.transform, false);
        }
        B_Error = false;

        if (I_questionCount == 0)
        {
            TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().Play();
            // Debug.Log("Playing Instruction audio = " + TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().clip.length);
            Invoke(nameof(BUT_Speaker), TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().clip.length);
        }
        else
        {
            BUT_Speaker();
        }
    }

    void BUT_Speaker()
    {
        G_Question.GetComponent<AudioSource>().Play();
    }

    public void BUT_Next()
    {
        if (G_errorMSG.activeInHierarchy)
            G_errorMSG.SetActive(false);
        // STR_selectedAnswer= inputfields
       // Debug.Log("Next");
        I_dummy = 0;
        AS_CLick.Play();
        STR_selectedAnswer = "";
       
       

        for (int i=0;i<G_OptionsLayout.transform.childCount;i++)
        {
            if(G_OptionsLayout.transform.GetChild(i).transform.GetChild(1).GetComponent<InputField>().text !="")
            {
                if (i == 0) 
                { 
                    STR_selectedAnswer = G_OptionsLayout.transform.GetChild(i).transform.GetChild(1).GetComponent<InputField>().text;
                }
                else
                {
                    STR_selectedAnswer = STR_selectedAnswer + "," +G_OptionsLayout.transform.GetChild(i).transform.GetChild(1).GetComponent<InputField>().text;
                }
                I_dummy++;

                if(I_dummy==STRL_options.Count)
                {
                    Debug.Log("I_Dummy =" + I_dummy);
                    B_Error = true;

                }
                else
                {
                    Debug.Log("I_Dummy!=count =" + I_dummy);
                    B_Error = false;
                }
            }
        }

        if(!B_Error)
        {
            G_errorMSG.SetActive(true);
        }
        else
        {
            G_errorMSG.SetActive(false);
            THI_Check();
        }    
    }
   
   
    void THI_gameData()
    {
         //THI_getPreviewData();
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
            StartCoroutine(IN_downloadlImg());
            StartCoroutine(IN_CoverImage());
            Debug.Log("live");
            THI_assignValues();

        }
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
    public IEnumerator IN_downloadlImg()
    {
        SPRA_Question = new Sprite[STRL_questions.Count];
        Debug.Log("Downloading Image");
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

                SPRA_Question[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_questions[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Question[i].name = Finalname[0];
            }
        }

        if(STRL_passageDetail!=null)
        {
            UnityWebRequest www1 = UnityWebRequestTexture.GetTexture(STRL_passageDetail[0]);
            yield return www1.SendWebRequest();
            if (www1.isNetworkError || www1.isHttpError)
            {
                Debug.Log(www1.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www1.downloadHandler).texture;

                G_Method.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                G_Method.GetComponent<Image>().preserveAspect = true;
            }
            G_MethodButton.SetActive(true);
        }
        else
        {
            G_MethodButton.SetActive(false);
        }
       

        //  THI_ShowQuestion();
        // Invoke("THI_ShowQuestion",2f);
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
        StartCoroutine(IN_downloadlImg());
        StartCoroutine(IN_CoverImage());
        THI_assignValues();
        Debug.Log("Preview");

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
        AS_CLick.Play();
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
      //  ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
      //  ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];
      //  ACA_MethodClips = new AudioClip[1];

      

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

                {
                    ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                    //   G_option1.GetComponent<AudioSource>().clip = ACA_optionClips[I_option1Count];
                    //  I_option2Count = I_option1Count + 1;
                    // G_option2.GetComponent<AudioSource>().clip = ACA_optionClips[I_option2Count];
                    // Debug.Log(I_option1Count + "    " + I_option2Count);
                    Debug.Log("audio clips fetched options");
                }
            }
        }

    }
    public IEnumerator EN_getAudioClips2()
    {
       // ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
       // ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];
        ACA_MethodClips = new AudioClip[1];

       

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
                {
                    ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                    TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];

                    Debug.Log("audio clips fetched instruction");
                }
            }
        }

        if (STRL_passageDetail[1] != null)
        {
            UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(STRL_passageDetail[1], AudioType.MPEG);
            yield return www1.SendWebRequest();
            if (www1.result == UnityWebRequest.Result.ConnectionError || www1.isHttpError || www1.isNetworkError)
            {
                Debug.Log(www1.error);
            }
            else
            {
                ACA_MethodClips[0] = DownloadHandlerAudioClip.GetContent(www1);
                G_Method.GetComponent<AudioSource>().clip = ACA_MethodClips[0];

                Debug.Log("audio clips fetched instruction");

            }
        }

        // THI_ShowQuestion();
        // MoveNext();

    }
    public void BUT_Start()
    {
        MoveNext();
    }



}
