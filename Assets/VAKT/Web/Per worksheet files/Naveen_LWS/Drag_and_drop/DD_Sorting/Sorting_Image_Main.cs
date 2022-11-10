using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Sorting_Image_Main : MonoBehaviour
{
    public static Sorting_Image_Main OBJ_Sorting_Image_Main;
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
    public Sprite[] SPRA_Options;
    
    public GameObject[] GA_Answer;
   
    public GameObject G_OptionImage;
    public GameObject G_OptionText;
    public Transform T_OptPos;
    int  I_Qcount, I_DroppedCount,I_Ans_img_txt,I_Counter;
    public List<Vector3> V3_Opt_Pos;
    public List<GameObject> GA_Options;
    public string[] Names;
    public Text TEX_emojiResult;
    public GameObject G_emoji;
    public GameObject G_coverPage;
    public AudioSource AS_CLick;
    bool B_Wrong,B_Image;

    public string STR_dummy;


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
        OBJ_Sorting_Image_Main = this;
        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA
        }
        else
        {
            URL = "http://20.120.84.12/Test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://20.120.84.12/Test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }
    }

    void Start()
    {
        I_Qcount  = 0;
        G_levelComplete.SetActive(false);
        G_errorMSG.SetActive(false);
        I_Counter = 0;
        THI_gameData();
       
    }

    public void BUT_Restart()
    {
       // I_correctAnsCount = 0;
        
        int j = I_Qcount + 1;
        TEX_questionCount.text = "" + j + "/" + (STRL_questions.Count);

        STR_selectedAnswer = "";
       // STRL_gameData = new List<string>();
       // STR_Data = "";

        I_Counter = I_Counter - Names.Length;
        Debug.Log("I_Counter restart = " + I_Counter);

        for (int i = 0; i < GA_Options.Count; i++)
        {
            Destroy(GA_Options[i]);
        }
       // GA_Options = null;
        GA_Options = new List<GameObject>();
        V3_Opt_Pos = new List<Vector3>();
        THI_ShowQuestion();
       
        
        // Names = null;
    }
    public void THI_BacktoOriginalpos()
    {

        for (int i = 0; i < GA_Options.Count; i++)
        {
            if (STR_dummy == GA_Options[i].name)
            {
               // if (GA_Options[i] != null)
              //  {
                    // Debug.Log("Debug.Log(already available other);");
                    GA_Options[i].transform.SetParent(T_OptPos, false);
                    GA_Options[i].transform.localScale = new Vector2(1f, 1f);

                    GA_Options[i].transform.position = V3_Opt_Pos[i];
               // }

            }
        }
    }

    public void THI_Count(bool plus)
    {
        if (plus)
        {
            if (I_DroppedCount < IL_numbers[3])
            {
                // Debug.Log("I_Qcount++");
                I_DroppedCount++;
            }
        }
        else
        {
            if (I_DroppedCount > 0)
            {
                // Debug.Log("I_Qcount--");
                I_DroppedCount--;
            }
        }
        Debug.Log("I_DroppedCount ="+ I_DroppedCount);
       
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

    public void THI_ShowQuestion()
    {
        I_DroppedCount=0;
        T_OptPos.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = true;
        B_Wrong = false;

        for (int i = 0; i < GA_Answer.Length; i++)
        {
            GA_Answer[i].SetActive(false);
        }

        int j = I_Qcount + 1;
        TEX_questionCount.text = "" + j + "/" + (STRL_questions.Count);

        TEX_question.text = STRL_questions[I_Qcount];
        TEX_question.GetComponent<AudioSource>().clip = ACA__questionClips[I_Qcount];
        STR_currentQuestionID = STRL_questionID[I_Qcount];
        
        GA_Answer[I_Ans_img_txt].SetActive(true);

        for (int i = 0; i < GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.childCount; i++)
        {
            GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
           
            if(I_Qcount>0)
            {
                if (GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).childCount != 0)
                {
                    Destroy(GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).gameObject);
                }
            }
        }

        Names = (STRL_answers[I_Qcount].Split(','));
      //  Debug.Log("I_Counter before clone = " + I_Counter);
        for (int i = I_Counter; i < I_Counter + Names.Length; i++)
        {
            

            if(B_Image)
            {
               // Debug.Log("Image clonning");
                GameObject G_Dummy = Instantiate(G_OptionImage);
                G_Dummy.transform.SetParent(T_OptPos, false);
                G_Dummy.name = SPRA_Options[i].name;
                G_Dummy.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Options[i];
               /* if(Names.Length>7)
                {
                    G_Dummy.transform.localScale = new Vector3(0.75f, 0.75f);
                }
                else
                {
                    G_Dummy.transform.localScale = new Vector3(1f, 1f);
                }*/

                G_Dummy.GetComponent<AudioSource>().clip = ACA_optionClips[i];
                G_Dummy.SetActive(true);
                GA_Options.Add(G_Dummy);
            }
            else
            {
                GameObject G_Dummy = Instantiate(G_OptionText);
                G_Dummy.transform.SetParent(T_OptPos, false);
                G_Dummy.name = STRL_options[i];
                G_Dummy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i];
                G_Dummy.GetComponent<AudioSource>().clip = ACA_optionClips[i];
                G_Dummy.SetActive(true);
                GA_Options.Add(G_Dummy);
            }
            

        }
       // Debug.Log(I_Qcount);
        


        for (int i = 0; i < Names.Length; i++)
        {
            GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).name = Names[i];
            GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
            GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).GetComponent<Collider2D>().enabled = true;
        }
        I_Counter = I_Counter + Names.Length;

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

        // Debug.Log("I_Counter after display = " + I_Counter);
        Invoke("THI_Layout_out", 1f);
    }

    public void BUT_Speaker()
    {
        TEX_question.GetComponent<AudioSource>().Play();
    }

    public void THI_Layout_out()
    {
        T_OptPos.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = false;

        for(int i=0;i<GA_Options.Count;i++)
        {
            if(GA_Options[i]!=null)
            {
                V3_Opt_Pos.Add(GA_Options[i].transform.position);
            }
        }

      //  T_OptPos.gameObject.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
    }

    public void THI_worksheetComplete()
    {

        G_levelComplete.SetActive(true);

        TEX_resultText.text = "Score : " + I_correctAnsCount + " / " + STRL_questions.Count;

        MainController.instance.I_correctPoints = I_correctAnsCount;
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        THI_checkEmojiResult();
       // TEX_resultText.text = "You have answered " + I_correctAnsCount + " / " + STRL_questions.Count + " questions correctly";
        StartCoroutine(IN_sendDataToDB());

    }

    public void THI_Counter()
    {
        I_DroppedCount++;
        if (G_errorMSG.activeInHierarchy)
            G_errorMSG.SetActive(false);

    }


    public void BUT_Check()
    {
        if (I_DroppedCount < IL_numbers[3])
        {
            G_errorMSG.SetActive(true);
        }
        else
        {
            for (int i = 0; i < GA_Answer[I_Ans_img_txt].transform.GetChild(0).childCount; i++)
            {
                if (GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).gameObject.activeInHierarchy)
                {
                   // Debug.Log(GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).name);
                     if (i == 0) { STR_selectedAnswer = GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).name; }
                     else { STR_selectedAnswer = STR_selectedAnswer+"," +GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).name; }
                    Debug.Log(GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).name+ " STR_Selected = " + STR_selectedAnswer);
                    
                    if (GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).name !=
                        GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).name)
                    {
                        Debug.Log(GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).name + " != " 
                           + GA_Answer[I_Ans_img_txt].transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).name);

                        B_Wrong = true;
                    }
                }
            }

            if (B_Wrong)
            {
                THI_TrackGameData("0");
            }
            else
            {
                THI_TrackGameData("1");
                I_correctAnsCount++;
                Debug.Log("Correct =" + I_correctAnsCount);
            }
            if (I_Qcount < STRL_questions.Count - 1)
            {
                GA_Options = new List<GameObject>();
                V3_Opt_Pos = new List<Vector3>();
                I_Qcount++;
                THI_ShowQuestion();

            }
            else
            {
                THI_worksheetComplete();
                Debug.Log("Correct =" + I_correctAnsCount);
            }

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
            json.Temp_type_2_LWS(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios,
                     STRL_details, STRL_cover_img_link);


            STR_childName = STRL_details[0];
            STR_childGrade = STRL_details[1];
            STR_worksheetName = STRL_details[2];
            STR_worksheerInstruction = STRL_instruction[0];
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(EN_getAudioClips1());
            StartCoroutine(EN_getAudioClips2());
            StartCoroutine(IN_CoverImage());
            Check_Imagecontent();


            Debug.Log("live");
            THI_assignValues();
            // THI_ShowQuestion();
        }
    }
    void Check_Imagecontent()
    {
        if (STRL_options[0].Contains(".png"))
        {
            StartCoroutine(IN_downloadlImg());
            B_Image = true;
            I_Ans_img_txt = 0;
        }
        else
        {
            B_Image = false;
            I_Ans_img_txt = 1;
           // THI_ShowQuestion();
        }
    }
    public IEnumerator IN_downloadlImg()
    {
        SPRA_Options = new Sprite[STRL_options.Count];
        for (int i = 0; i < STRL_options.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_options[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                
                    SPRA_Options[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    string[] Names = (STRL_options[i].Split('/'));
                    string[] Finalname = (Names[Names.Length - 1].Split('.'));

                    SPRA_Options[i].name = Finalname[0];
               
            }
        }
      //  THI_ShowQuestion();
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

        StartCoroutine(EN_getAudioClips());
        StartCoroutine(EN_getAudioClips1());
        StartCoroutine(EN_getAudioClips2());
        StartCoroutine(IN_CoverImage());

        Check_Imagecontent();

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

       /* for (int i = 0; i < STRL_optionsAudios.Count; i++)
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

                string[] Names = (STRL_optionsAudios[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                // Debug.Log("Q_Image name = " + Finalname[0]);
                ACA_optionClips[i].name = Finalname[0];
                
                Debug.Log("audio clips fetched options");

            }
        }

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
        }*/
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

                string[] Names = (STRL_optionsAudios[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                // Debug.Log("Q_Image name = " + Finalname[0]);
                ACA_optionClips[i].name = Finalname[0];

                Debug.Log("audio clips fetched options");

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

                ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                TEX_worksheetInstruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];

                Debug.Log("audio clips fetched instruction");

            }
        }
    }

   
}