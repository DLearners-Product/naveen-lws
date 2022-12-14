using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using TMPro;

public class SGM_image : MonoBehaviour
{
    [Header("DB")]
    public string URL;
    public string SendValueURL;
    public List<string> STRL_difficulty;
    public string STR_difficulty;
    public int I_totalQuestionCount;
    public List<int> IL_numberValues;
    public List<string> STRL_questionsDB;
    public List<string> STRL_questionID;
    public List<string> STRL_optionsDB;
    public List<string> STRL_answersDB;
    public List<string> STRL_optionsSpriteName;
    public List<Sprite> SPRL_questionSprite;
    public List<Sprite> SPRL_optionsSprite;
    public List<string> STRL_instruction;
    public int I_correctPoints;
    public int I_wrongPoints;

    [Header("Gameplay")]
    public static SGM_image instance;
    public int I_wrongAnsCount;
    public Animator AN_squirrel;
    public int I_currentQuestionCount;
    public int I_currentOptionReqCount;
    public int I_lastOptionReqCount;
    public Image IM_currentQuestion;
    public string STR_currentQuestion;
    public string STR_currentCorrectAnswer;
    public string STR_clickedAnswer;
    int I_1stSprite;
    int I_2ndSprite;
    int I_3rdSprite;
    string STR_1stOptionName;
    string STR_2ndOptionName;
    string STR_3rdOptionName;
    bool B_called;
    int z;
    public GameObject G_ansDisplay;
    public Image IM_answerDisplay;
    public Text TEX_annswerDisplay;
    public GameObject[] GA_options;
    public GameObject G_levelComp;
    public int I_points;
    public Text TEX_points;
    public Text TEX_questionCount;
    public string STR_currentQuestionID;

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;


    [Header("Instruction")]
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;

    [Header("Audios")]
    public AudioSource AS_bgm;
    public AudioSource AS_correct;
    public AudioSource AS_wrong;

    [Header("AUDIO DB")]
    public List<string> STRL_questionAudios;
    public List<string> STRL_optionsAudios;
    public List<string> STRL_instructionAudios;


    void Start()
    {
        instance = this;
        I_currentQuestionCount = -1;
        G_instructionPage.SetActive(false);
        I_totalQuestionCount = STRL_questionsDB.Count;
        I_points = 0;
        TEX_points.text = I_points.ToString();
        int curqcount = I_currentQuestionCount + 1;
        TEX_questionCount.text = curqcount + "/" + I_totalQuestionCount;
        Invoke("THI_gameData", 1f);
    }

    public void BUT_optionClick()
    {
        //string clickedanswer = EventSystem.current.currentSelectedGameObject.name;
        //string[] clickedanswersplit = clickedanswer.Split('/');
        //int lastelement = clickedanswersplit.Length - 1;
        //string lastelementanswer = clickedanswersplit[lastelement];
        //string[] lastelementsplit = lastelementanswer.Split('.');
        STR_clickedAnswer = EventSystem.current.currentSelectedGameObject.name;

        if (STR_clickedAnswer.Contains(STR_currentCorrectAnswer))
        {
            //correct
            I_points = I_points + I_correctPoints;
            TEX_points.text = I_points.ToString();
            AN_squirrel.Play("happy");
            G_ansDisplay.SetActive(true);
            IM_answerDisplay.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
            IM_answerDisplay.preserveAspect = true;
            
            string[] clickedanswersplit = STR_clickedAnswer.Split('/');
            int lastelement = clickedanswersplit.Length - 1;
            string lastelementanswer = clickedanswersplit[lastelement];
            string[] lastelementsplit = lastelementanswer.Split('.');
            TEX_annswerDisplay.text = lastelementsplit[0];
            THI_TrackGameData("1");
            Invoke("THI_showQuestion", 3f);
        }
        else
        {
            //wrong
            if (I_points > I_wrongPoints)
            {
                I_points -= I_wrongPoints;
            }
            else
            {
                if (I_points > 0)
                {
                    I_points = 0;
                }
            }
            THI_TrackGameData("0");
            TEX_points.text = I_points.ToString();
            AN_squirrel.Play("sad");
            Invoke("THI_squirrelIdle", 1.5f);

            I_wrongAnsCount++;
            if (I_wrongAnsCount == 3)
            {
                if (STR_difficulty == "assistive")
                {
                    // show ans and go to next question
                    G_ansDisplay.SetActive(true);
                    for (int i = 0; i < GA_options.Length; i++)
                    {
                        if (GA_options[i].name.Contains(STRL_answersDB[I_currentQuestionCount]))
                        {
                            IM_answerDisplay.sprite = GA_options[i].GetComponent<Image>().sprite;
                        }
                    }
                    TEX_annswerDisplay.text = STRL_answersDB[I_currentQuestionCount];

                    Invoke("THI_showQuestion", 2.5f);

                }
                if (STR_difficulty == "intuitive")
                {
                    // show ans and make the child click to go to next question
                    G_ansDisplay.SetActive(true);
                    for (int i = 0; i < GA_options.Length; i++)
                    {
                        if (GA_options[i].name.Contains(STRL_answersDB[I_currentQuestionCount]))
                        {
                            IM_answerDisplay.sprite = GA_options[i].GetComponent<Image>().sprite;
                        }
                    }
                    TEX_annswerDisplay.text = STRL_answersDB[I_currentQuestionCount];
                    Invoke("THI_disableAnsDisplay", 1.5f);
                }
            }
            if (I_wrongAnsCount == 2)
            {
                if (STR_difficulty == "independent")
                {
                    // dont show ans and go to next question
                    THI_showQuestion();

                }
            }

        }
    }

    void THI_disableAnsDisplay()
    {
        G_ansDisplay.SetActive(false);
    }

    void THI_squirrelIdle()
    {
        AN_squirrel.Play("idle");
    }

    public void THI_showQuestion()
    {
        I_wrongAnsCount = 0;
        G_ansDisplay.SetActive(false);
        AN_squirrel.Play("idle");
        I_1stSprite = I_2ndSprite = I_3rdSprite = 99;  //  default sprite val for checking and assigning in for loop
        STR_1stOptionName = STR_2ndOptionName = STR_3rdOptionName = ""; //  default name val for checking and assigning in for loop

        if (B_called)
        {
            I_currentQuestionCount++;
            I_currentOptionReqCount = I_currentOptionReqCount + 3;
        }
        else
        {
            I_currentQuestionCount++;
            I_currentOptionReqCount = I_currentOptionReqCount + 2;
        }

        if (I_currentQuestionCount < I_totalQuestionCount)
        {
            //game still running
            int curqcount = I_currentQuestionCount + 1;
            TEX_questionCount.text = curqcount + "/" + I_totalQuestionCount;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            STR_currentCorrectAnswer = STRL_answersDB[I_currentQuestionCount];
            STR_currentQuestion = STRL_questionsDB[I_currentQuestionCount];
            IM_currentQuestion.sprite = SPRL_questionSprite[I_currentQuestionCount];

            if (B_called)
            {
                z = I_lastOptionReqCount + 1;
            }
            else
            {
                z = 0;
            }
            B_called = true;

            for (int i = z; i <= I_currentOptionReqCount; i++)
            {
                if (I_1stSprite != 99 && I_2ndSprite != 99 && I_3rdSprite == 99)
                {
                    I_3rdSprite = i;
                }
                if (I_1stSprite != 99 && I_2ndSprite == 99)
                {
                    I_2ndSprite = i;
                }
                if (I_1stSprite == 99)
                {
                    I_1stSprite = i;
                }

                if (STR_1stOptionName != "" && STR_2ndOptionName != "" && STR_3rdOptionName == "")
                {
                    STR_3rdOptionName = STRL_optionsSpriteName[i];
                }
                if (STR_1stOptionName != "" && STR_2ndOptionName == "")
                {
                    STR_2ndOptionName = STRL_optionsSpriteName[i];
                }
                if (STR_1stOptionName == "")
                {
                    STR_1stOptionName = STRL_optionsSpriteName[i];
                }
            }
            GA_options[0].GetComponent<Image>().sprite = SPRL_optionsSprite[I_1stSprite];
            GA_options[1].GetComponent<Image>().sprite = SPRL_optionsSprite[I_2ndSprite];
            GA_options[2].GetComponent<Image>().sprite = SPRL_optionsSprite[I_3rdSprite];
            GA_options[0].name = STR_1stOptionName;
            GA_options[1].name = STR_2ndOptionName;
            GA_options[2].name = STR_3rdOptionName;
            I_lastOptionReqCount = I_currentOptionReqCount;

        }
        else
        {
            //game complete
            AS_bgm.Stop();
            if (MainController.instance.mode == "live")
            {
                StartCoroutine(IN_sendDataToDB());
            }
            MainController.instance.I_TotalPoints = I_points;
            G_levelComp.SetActive(true);
        }
    }
    void THI_gameData()
    {
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(EN_GetData()); // live game in portal
        }
        if (MainController.instance.mode == "preview")
        {
            // preview data in html game generator

            THI_getPreviewData();
        }
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
            Debug.Log("GAME DATA: " + www.downloadHandler.text);
            MyJSON json = new MyJSON();
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios);
            STR_difficulty = STRL_difficulty[0];
            StartCoroutine(IN_downloadOptionImages());
            StartCoroutine(IN_downloadQuestionImages());
            STRL_optionsSpriteName = STRL_optionsDB;
            I_totalQuestionCount = IL_numberValues[0];
            I_correctPoints = IL_numberValues[1];
            I_wrongPoints = IL_numberValues[2];
            MainController.instance.I_TotalQuestions = I_totalQuestionCount;
            MainController.instance.I_correctPoints = I_correctPoints;
            // THI_showQuestion();
            Invoke("THI_showQuestion", 2f);
        }
    }

    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios);
        STR_difficulty = STRL_difficulty[0];
        StartCoroutine(IN_downloadOptionImages());
        StartCoroutine(IN_downloadQuestionImages());
        STRL_optionsSpriteName = STRL_optionsDB;
        I_totalQuestionCount = IL_numberValues[0];
        I_correctPoints = IL_numberValues[1];
        I_wrongPoints = IL_numberValues[2];
        MainController.instance.I_TotalQuestions = I_totalQuestionCount;
        MainController.instance.I_correctPoints = I_correctPoints;
        Invoke("THI_showQuestion", 2f);
    }

    public IEnumerator IN_downloadOptionImages()
    {

        for (int i = 0; i < STRL_optionsDB.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_optionsDB[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                SPRL_optionsSprite.Add(Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
            }
        }
    }
    public IEnumerator IN_downloadQuestionImages()
    {

        for (int i = 0; i < STRL_questionsDB.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_questionsDB[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                SPRL_questionSprite.Add(Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
            }
        }
    }

    public void THI_TrackGameData(string analysis)
    {
        DBmanager squirrelDB = new DBmanager();
        squirrelDB.question_id = STR_currentQuestionID;
        squirrelDB.answer = STR_clickedAnswer;
        squirrelDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(squirrelDB);
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
            Debug.Log("Sending data to DB success : " + www.downloadHandler.text);
        }
    }
    public void BUT_instructionPage()
    {
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STRL_instruction[0];
    }
    public void BUT_speakerInstruction()
    {

    }
    public void BUT_closeInstruction()
    {
        G_instructionPage.SetActive(false);
    }
}
