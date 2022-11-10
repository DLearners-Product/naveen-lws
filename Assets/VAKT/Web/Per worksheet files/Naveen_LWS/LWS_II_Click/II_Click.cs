using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class II_Click : MonoBehaviour
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
    public string STR_difficulty;
    public string STR_currentQuestionAnswer;


    [Header("Gameplay")]
    public int I_optCountType;
    public int I_questionCount;
    public Text TEX_questionCount;
    public Text TEX_question;
    public string STR_selectedAnswer;
    public GameObject G_errorMSG;
    // public GameObject G_Selected1;
    //  public GameObject G_Selected2;
    public Text TEX_worksheetInstruction;
    public Text TEX_childName;
    public Text TEX_childGrade;
    public Text TEX_worksheetName;
    public Color COL_selectedColor;
    public int I_correctAnsCount;
    public GameObject G_levelComplete;
    public Text TEX_resultText;
    public GameObject G_Qusetion;
    public GameObject[] GA_Options;
    GameObject G_Option;
    public Sprite[] SPRA_Options;
    public Sprite[] SPRA_Questions;
    public Color[] CLR_Colors;
    int I_Dummy, I_Qcount;
    int I_SelectedOptions;
    public Text TEX_emojiResult;
    public GameObject G_emoji;
    public GameObject G_coverPage;
    public AudioSource AS_CLick;
    GameObject G_Selected;

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
        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA
        }
        else
        {
            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }
    }

    void Start()
    {
        G_levelComplete.SetActive(false);
        G_errorMSG.SetActive(false);

        THI_gameData();
        I_Qcount = 0;
        I_Dummy = -1;
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
    }


    void THI_assignValues()
    {
        TEX_childName.text = STR_childName;
        TEX_childGrade.text = "Grade : " + STR_childGrade;
        TEX_worksheetName.text = STR_worksheetName;
        TEX_worksheetInstruction.text = STR_worksheerInstruction;
        int j = I_Qcount + 1;
        TEX_questionCount.text = "" + j + "/" + (STRL_questions.Count);
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

    public void THI_worksheetComplete()
    {
        AS_CLick.Play();

        G_levelComplete.SetActive(true);
        TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;

        MainController.instance.I_correctPoints = I_correctAnsCount;
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        THI_checkEmojiResult();
        StartCoroutine(IN_sendDataToDB());
    }

    public void BUT_option()
    {
        //  Debug.Log("Clicking");
        AS_CLick.Play();

        G_Selected = EventSystem.current.currentSelectedGameObject;
        //  Debug.Log("G_Selected = "+ G_Selected.name);
        for (int i = 0; i < G_Option.transform.childCount; i++)
        {
            G_Option.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }

        G_Selected.GetComponent<Image>().color = COL_selectedColor;
        // Debug.Log("G_Selected Color change= " + G_Selected.name);
        if (G_errorMSG.activeInHierarchy)
            G_errorMSG.SetActive(false);
    }

    public void BUT_Next()
    {
        AS_CLick.Play();

        if (G_Selected == null)
        {
            G_errorMSG.SetActive(true);
        }
        else
        {
            STR_selectedAnswer = G_Selected.name;

            if (STR_selectedAnswer == STR_currentQuestionAnswer)
            {
                THI_TrackGameData("1");
                I_correctAnsCount++;
            }
            else
            {
                THI_TrackGameData("0");
            }

            for (int i = 0; i < G_Option.transform.childCount; i++)
            {
                G_Option.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            if (I_Qcount < STRL_questions.Count - 1)
            {
                I_Qcount++;
                int j = I_Qcount + 1;
                TEX_questionCount.text = "" + j + "/" + (STRL_questions.Count);
                THI_ShowQusetion();
            }
            else
            {
                THI_worksheetComplete();

            }
        }




    }
    public void THI_ShowQusetion()
    {
        G_Selected = null;
        G_Qusetion.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Questions[I_Qcount];
        G_Qusetion.GetComponent<AudioSource>().clip = ACA__questionClips[I_Qcount];
        for (int i = 0; i < G_Option.transform.childCount; i++)
        {
            I_Dummy++;
            G_Option.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Options[I_Dummy];
            G_Option.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            G_Option.transform.GetChild(i).GetComponent<AudioSource>().clip = ACA_optionClips[I_Dummy];
            G_Option.transform.GetChild(i).name = SPRA_Options[I_Dummy].name;
            G_Option.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        STR_currentQuestionAnswer = STRL_answers[I_Qcount];
        STR_currentQuestionID = STRL_questionID[I_Qcount];

        if (I_Qcount == 0)
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
    public void BUT_Speaker()
    {
        G_Qusetion.GetComponent<AudioSource>().Play();
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
            json.Temp_type_2_LWS(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios,
                     STRL_details, STRL_cover_img_link);

            STR_childName = STRL_details[0];
            STR_childGrade = STRL_details[1];
            STR_worksheetName = STRL_details[2];
            STR_worksheerInstruction = STRL_instruction[0];
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(EN_getAudioClips1());
            StartCoroutine(EN_getAudioClips2());
            StartCoroutine(IN_downloadlImg());
            StartCoroutine(IN_downloadlImg1());
            StartCoroutine(IN_CoverImage());
            Debug.Log("live");
            THI_assignValues();
            // THI_ShowQuestion();
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
        SPRA_Questions = new Sprite[STRL_questions.Count];

        for (int i = 0; i < STRL_questions.Count; i++)
        {
            // Debug.Log("Coming in :" + STRL_options[i]);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_questions[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                // Debug.Log("Coming in :" + STRL_options[i]);
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                SPRA_Questions[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_questions[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Questions[i].name = Finalname[0];

                // Debug.Log("Options names "+ SPRA_Options[i].name);
            }
        }
        /*if (IL_numbers[3] == 2)
        {
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Option = GA_Options[0].gameObject;
            G_Option.SetActive(true);
        }
        else
        if (IL_numbers[3] == 3)
        {
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Option = GA_Options[1].gameObject;
            G_Option.SetActive(true);
        }
        else
        if (IL_numbers[3] == 4)
        {
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Option = GA_Options[2].gameObject;
            G_Option.SetActive(true);
        }*/
       
    }
    public IEnumerator IN_downloadlImg1()
    {
        SPRA_Options = new Sprite[STRL_options.Count];

        for (int i = 0; i < STRL_options.Count; i++)
        {
            // Debug.Log("Coming in :" + STRL_options[i]);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_options[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                // Debug.Log("Coming in :" + STRL_options[i]);
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                SPRA_Options[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_options[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Options[i].name = Finalname[0];

                // Debug.Log("Options names "+ SPRA_Options[i].name);
            }
        }

        if (IL_numbers[3] == 2)
        {
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Option = GA_Options[0].gameObject;
            G_Option.SetActive(true);
        }
        else
        if (IL_numbers[3] == 3)
        {
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Option = GA_Options[1].gameObject;
            G_Option.SetActive(true);
        }
        else
        if (IL_numbers[3] == 4)
        {
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Option = GA_Options[2].gameObject;
            G_Option.SetActive(true);
        }

    }
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        json.Temp_type_2_LWS(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios,
                     STRL_details, STRL_cover_img_link);

        STR_childName = STRL_details[0];
        STR_childGrade = STRL_details[1];
        STR_worksheetName = STRL_details[2];
        STR_worksheerInstruction = STRL_instruction[0];
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(EN_getAudioClips1());
        StartCoroutine(EN_getAudioClips2());
        StartCoroutine(IN_downloadlImg());
        StartCoroutine(IN_downloadlImg1());
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
       // ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
       // ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

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
        //ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

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
    }

}
