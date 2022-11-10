using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LWS_Wordsearch : MonoBehaviour
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
    public int I_questionCount;
    public Text TEX_questionCount;
    public string STR_selectedAnswer;
    public GameObject G_errorMSG;
    public Text TEX_worksheetInstruction;
    public Text TEX_question;
    public Text TEX_childName;
    public Text TEX_childGrade;
    public Text TEX_worksheetName;
    public int I_correctAnsCount;
    public GameObject G_levelComplete;
    public Text TEX_resultText;
    public GameObject G_emoji;
    public GameObject G_coverPage;
    public AudioSource AS_CLick;
    public Text TEX_emojiResult;
    public GameObject G_Options;
    public GameObject G_Question;
    public GameObject G_RenPos;
    public GameObject G_OptPrefab, G_QuePrefab,G_RenPrefab;
    public Color[] CLR_Colors;
    public Color CLR_Selected;
    public bool B_Start, B_End;
    public GameObject G_Render,G_Selected;
    public string STR_Word;
    int I_Ccount;
    public List<GameObject> GL_Words;
    
    bool B_Wrong;

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
        I_questionCount = -1;
        //TEX_questionCount.text = "" + I_questionCount + "/" + (STRL_questions.Count);
        THI_gameData();
    }
    public void THI_Words()
    {
        G_errorMSG.SetActive(false);
        G_Selected.GetComponent<Image>().color = CLR_Selected;
        string currentletter = G_Selected.name;
        STR_Word = STR_Word + currentletter;
       // Debug.Log("Word = " + STR_Word);
        GL_Words.Add(G_Selected);
        if (STR_Word.Length==1)
        {
            if (B_Start)
            {
                THI_Start();
            }
        }
       
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldpoint, Vector2.zero);

            if (hit.collider != null)
            {
                G_Selected = hit.collider.gameObject;
                
                /*if (B_Start)
                {
                    THI_Start();
                }*/
                THI_Words();
                G_Selected.GetComponent<Collider2D>().enabled = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
           THI_End();
        }

    }

  
    void THI_assignValues()
    {
        TEX_childName.text = STR_childName;
        TEX_childGrade.text = "Grade : " + STR_childGrade;
        TEX_worksheetName.text = STR_worksheetName;
        TEX_worksheetInstruction.text = STR_worksheerInstruction;
    }

    public void BUT_WorksheetCompleted()
    {
        if (I_questionCount < STRL_questions.Count - 1)
        {
            G_errorMSG.SetActive(true);
           // I_questionCount++;
            
        }
        else
        {
            G_levelComplete.SetActive(true);
            TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;
            THI_checkEmojiResult();

            MainController.instance.I_correctPoints = I_correctAnsCount;
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

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
        // STR_currentQuestionID = STRL_questionID[I_questionCount];
        // STR_currentQuestionAnswer = STRL_answers[I_questionCount];

        int k = I_questionCount + 1;
        TEX_questionCount.text =k + "/" + (STRL_questions.Count);

        for (int i = 0; i < STRL_questions.Count; i++)
        {
            GameObject Q_Dummy = Instantiate(G_QuePrefab);
            Q_Dummy.GetComponent<Text>().text = STRL_questions[i];
            Q_Dummy.GetComponent<AudioSource>().clip = ACA__questionClips[i];
            Q_Dummy.transform.SetParent(G_Question.transform, false);
        }

        if(IL_numbers[3]==1)
        {
            for (int i = 0; i < STRL_options.Count; i++)
            {
                string[] dummy = STRL_options[i].Split(',');
                for (int j = 0; j < dummy.Length; j++)
                {
                    GameObject G_Opt = Instantiate(G_OptPrefab);
                    G_Opt.transform.GetChild(0).GetComponent<Text>().text = dummy[j];
                    G_Opt.name = dummy[j];
                    G_Opt.transform.SetParent(G_Options.transform, false);
                }
                dummy = new string[0];
            }
        }
        else
        {
            for (int i = 0; i < STRL_options.Count; i++)
            {
                    GameObject G_Opt = Instantiate(G_OptPrefab);
                    G_Opt.transform.GetChild(0).GetComponent<Text>().text = STRL_options[i];
                    G_Opt.name = STRL_options[i];
                    G_Opt.transform.SetParent(G_Options.transform, false);
            }
        }
       

        B_Start = true;
        B_End = false;

    }

    public void THI_Start()
    {
        B_Start = false;
        B_End = true;
       // G_Render = Instantiate(G_RenPrefab);
       // Debug.Log("Render_Line");
       // G_Render.transform.SetParent(G_RenPos.transform, false);
       // G_Render.transform.GetChild(0).GetComponent<renderer>().points[0] = G_Selected.transform;
       // G_Render.transform.GetChild(0).GetComponent<renderer>().enabled = true;
       // G_Selected = null;
    }
    public void THI_End()
    {
        if(STR_Word!="")
        {
            B_Wrong = true;
            STR_selectedAnswer = STR_Word;
            STR_Word = "";
            B_End = false;
            // Debug.Log("Ending");
            /*for (int k = 0; k < GL_Words.Count; k++)
            {
                GL_Words[k].GetComponent<Collider2D>().enabled = true;
            }*/
            for (int i = 0; i < STRL_questions.Count; i++)
            {
                if (STRL_questions[i].Contains(STR_selectedAnswer))
                {
                    STR_currentQuestionID = STRL_questionID[i];
                    STR_currentQuestionAnswer = STRL_answers[i];
                    // Debug.Log(STR_currentQuestionAnswer);
                }
            }

            for (int i = 0; i < STRL_answers.Count; i++)
            {
                if (STRL_answers[i] == STR_selectedAnswer)
                {
                    B_Wrong = false;
                    // Selected_Answers.Add(STR_selectedAnswer);
                    I_correctAnsCount++;
                    THI_TrackGameData("1");
                    G_Question.transform.GetChild(i).GetComponent<Text>().color = CLR_Colors[I_Ccount];
                    G_Question.transform.GetChild(i).GetComponent<AudioSource>().Play();
                    for (int k = 0; k < GL_Words.Count; k++)
                    {
                        GL_Words[k].GetComponent<Image>().color = CLR_Colors[I_Ccount];
                        // GL_Words[k].GetComponent<Collider2D>().enabled = true;
                    }
                    if (I_Ccount < 9) { I_Ccount++; } else { I_Ccount = 0; }
                    I_questionCount++;
                    int x = I_questionCount + 1;
                    TEX_questionCount.text = "" + x + "/" + (STRL_questions.Count);

                    GL_Words = new List<GameObject>();
                    B_Start = true;
                }

            }

            WrongAttempt();
            GL_Words = new List<GameObject>();

        }
       
       
       // G_Selected = null;
    }

    void WrongAttempt()
    {
        if(B_Wrong)
        {
            for (int k = 0; k < GL_Words.Count; k++)
            {
                GL_Words[k].GetComponent<Collider2D>().enabled = true;
                GL_Words[k].GetComponent<Image>().color = Color.white;
            }
            STR_currentQuestionID = STRL_questionID[0];
            THI_TrackGameData("0");
        }
            
    }
    void THI_gameData()
    {
        // THI_getPreviewData();
        // THI_nextQuestion();
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
          //  StartCoroutine(IN_downloadlImg());
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(EN_getAudioClips1());
            StartCoroutine(EN_getAudioClips2());
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
  
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        json.Temp_type_2_LWS(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios,
                     STRL_details, STRL_cover_img_link);

        STR_childName = STRL_details[0];
        STR_childGrade = STRL_details[1];
        STR_worksheetName = STRL_details[2];
        STR_worksheerInstruction = STRL_instruction[0];
        // StartCoroutine(IN_downloadlImg());
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(EN_getAudioClips1());
        StartCoroutine(EN_getAudioClips2());
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
       
        // ACA_MethodClips = new AudioClip[1];

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
        // ACA_MethodClips = new AudioClip[1];

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
       // ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
       // ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];
        // ACA_MethodClips = new AudioClip[1];

       

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

    public void BUT_Start()
    {
        TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().Play();
        THI_ShowQuestion();
       // Debug.Log("Play INstruction audio");
    }

}
