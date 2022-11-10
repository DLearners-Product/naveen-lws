using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class OrderDragManager : MonoBehaviour
{
    public bool B_production;

    [Header("Instance")]
    public static OrderDragManager instance;

    [Header("DB")]
    public string STR_difficulty;
    public string URL;
    public string SendValueURL;
    public int I_correctPoints;
    public int I_wrongPoints;
    public List<int> IL_numberValues;
    public List<string> STRL_questionsDB;
    public List<string> STRL_optionsDB;
    public List<string> STRL_answersDB;
    public string STR_currentSelectionAnswer;
    public string STR_currentQuestionID;
    public List<string> STRL_instruction;
    public List<string> STRL_options1;
    public List<string> STRL_answers1;
    public List<string> STRL_options2;
    public List<string> STRL_answers2;
    public List<string> STRL_options3;
    public List<string> STRL_answers3;
    public List<string> STRL_options4;
    public List<string> STRL_answers4;
    public List<string> STRL_options5;
    public List<string> STRL_answers5;
    public List<string> STRL_options6;
    public List<string> STRL_answers6;
    public List<string> STRL_options7;
    public List<string> STRL_answers7;
    public List<string> STRL_options8;
    public List<string> STRL_answers8;
    public List<string> STRL_options9;
    public List<string> STRL_answers9;
    public List<string> STRL_options10;
    public List<string> STRL_answers10;
    public List<string> STRL_difficulty;


    [Header("Game")]
    public int I_qCount;
    public Text TEX_qCount;
    public int I_totalQuestionCount;
    public List<GameObject> GL_optionsBubble;
    public Vector2[] VA_startPos;
    public int I_matches;
    public int I_points;
    public Text TEX_points;
    public GameObject G_caterpillarHead;
    public GameObject G_levelComplete;
    public List<string> STRL_childAnswer;
    public GameObject[] GA_answersObject;
    public int I_wrongAnsCount;
    public GameObject[] GA_hintObjects;

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;
    public List<string> STRL_questionID;

    [Header("AUDIO DB")]
    public List<string> STRL_questionAudios;
    public List<string> STRL_optionsAudios;
    public List<string> STRL_instructionAudios;

    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;
    public AudioClip[] ACA_clip1;
    public AudioClip[] ACA_clip2;
    public AudioClip[] ACA_clip3;
    public AudioClip[] ACA_clip4;
    public AudioClip[] ACA_clip5;
    public AudioClip[] ACA_clip6;
    public AudioClip[] ACA_clip7;
    public AudioClip[] ACA_clip8;
    public AudioClip[] ACA_clip9;
    public AudioClip[] ACA_clip10;

    [Header("Inst")]
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;

    private void Awake()
    {
        instance = this;


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
        THI_assignStartPos();
        I_qCount++;      
      
        Invoke("THI_gameData", 1f);
    }

   void THI_assignStartPos()
    {
        VA_startPos = new Vector2[GL_optionsBubble.Count];
        for (int i = 0; i < GL_optionsBubble.Count; i++)
        {
            VA_startPos[i] = GL_optionsBubble[i].transform.position;
        }
    }

    void THI_redirectTostartPos()
    {
        for (int i = 0; i < GL_optionsBubble.Count; i++)
        {
            GL_optionsBubble[i].transform.position = VA_startPos[i];
            GL_optionsBubble[i].GetComponent<Collider2D>().enabled = true;
            GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
            GL_optionsBubble[i].GetComponent<DragBubbles>().enabled = true;
        }
       
    }

    void THI_checkLevelComplete()
    {
        if (I_matches == 6)
        {
          
            
          
            
            STR_currentSelectionAnswer = string.Join(",", STRL_childAnswer);

         //   Debug.Log(childAnswer);
         //   Debug.Log(STRL_answersDB[I_qCount]);
            if (STR_currentSelectionAnswer == STRL_answersDB[I_qCount])
            {
              //  Debug.Log("RAKKUU!!");
                I_qCount++;
                if (I_qCount < I_totalQuestionCount)
                {
                    G_caterpillarHead.GetComponent<Animator>().Play("smile");
                    I_wrongAnsCount = 0;
                    THI_increaseScore();
                    THI_TrackGameData("1");
                    STRL_childAnswer = new List<string>();
                    THI_assignOptions();
                    THI_redirectTostartPos();
                }
                else
                {
                    G_caterpillarHead.GetComponent<Animator>().Play("smile");
                    G_levelComplete.SetActive(true);
                    if (MainController.instance.mode == "live")
                    {
                        StartCoroutine(IN_sendDataToDB());
                    }
                }
            }
            else
            {
                G_caterpillarHead.GetComponent<Animator>().Play("sad");
                if (I_points >= I_wrongPoints)
                {
                    I_points = I_points - I_wrongPoints;
                }
                else
                {
                    I_points = 0;
                }
                THI_TrackGameData("0");
                TEX_points.text = I_points.ToString();

                I_wrongAnsCount++;
                if(I_wrongAnsCount==3)
                {
                    if(STR_difficulty == "assistive")
                    {
                        I_wrongAnsCount = 0;
                        THI_showHintObjects();
                        Invoke(nameof(THI_offHint), 3f);
                    }
                    if(STR_difficulty == "intuitive")
                    {
                        THI_showHintObjects();
                        Invoke(nameof(THI_offHint), 3f);
                    }
                }
                if (I_wrongAnsCount == 2)
                {
                    if (STR_difficulty == "independent")
                    {
                        I_qCount++;
                        if (I_qCount < I_totalQuestionCount)
                        {
                            I_wrongAnsCount = 0;
                            STRL_childAnswer = new List<string>();
                            THI_assignOptions();
                            THI_redirectTostartPos();
                        }
                        else
                        {
                            G_levelComplete.SetActive(true);
                            if (MainController.instance.mode == "live")
                            {
                                StartCoroutine(IN_sendDataToDB());
                            }
                        }
                    }
                }

                STRL_childAnswer = new List<string>();
                THI_assignOptions();
                THI_redirectTostartPos();
            }
        }
    }

    void THI_showHintObjects()
    {
        for(int i = 0; i < GA_hintObjects.Length; i++)
        {
            GA_hintObjects[i].SetActive(true);

                if(I_qCount==0)
            GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers1[i];

            if (I_qCount == 1)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers2[i];

            if (I_qCount == 2)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers3[i];

            if (I_qCount == 3)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers4[i];

            if (I_qCount == 4)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers5[i];

            if (I_qCount == 5)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers6[i];

            if (I_qCount == 6)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers7[i];

            if (I_qCount == 7)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers8[i];

            if (I_qCount == 8)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers9[i];

            if (I_qCount == 9)
                GA_hintObjects[i].transform.GetChild(0).GetComponent<Text>().text = STRL_answers10[i];

        }
       
    }

    void THI_offHint()
    {
        for (int i = 0; i < GA_hintObjects.Length; i++)
        {
            GA_hintObjects[i].SetActive(false);
        }

        if (STR_difficulty == "assistive")
        {
            I_qCount++;
            if (I_qCount < I_totalQuestionCount)
            {
                STRL_childAnswer = new List<string>();
                THI_assignOptions();
                THI_redirectTostartPos();
            }
            else
            {
                G_levelComplete.SetActive(true);
                if (MainController.instance.mode == "live")
                {
                    StartCoroutine(IN_sendDataToDB());
                }
            }
        }
    }

    public void THI_TrackGameData(string analysis)
    {
        DBmanager caterpillarDB = new DBmanager();
        caterpillarDB.question_id = STR_currentQuestionID;
        caterpillarDB.answer = STR_currentSelectionAnswer;
        caterpillarDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(caterpillarDB);
        STRL_gameData.Add(toJson);
        STR_Data = string.Join(",", STRL_gameData);
    }

    public IEnumerator IN_sendDataToDB()
    {
        WWWForm form = new WWWForm();
        form.AddField("child_id", MainController.instance.STR_childID);
        form.AddField("game_id", MainController.instance.STR_GameID);
        form.AddField("game_details", "[" + STR_Data + "]");


  //    Debug.Log("child id : " + MainController.instance.STR_childID);
  //     Debug.Log("game_id  : " + MainController.instance.STR_GameID);
//       Debug.Log("game_details: " + "[" + STR_Data + "]");

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

    public void THI_delayCheckLevelComplete()
    {
        Invoke("THI_checkLevelComplete", 4f);
    }

    public void THI_increaseScore()
    {
        I_points = I_points + I_correctPoints;
        TEX_points.text = I_points.ToString();
    }


    void THI_splitOptions2(List<string> optionsarray, int start, int end)
    {
        if (end <= STRL_optionsDB.Count)
        {
           // Debug.Log(start +"-"+ end);
            optionsarray[0] = STRL_optionsDB[start];
            optionsarray[1] = STRL_optionsDB[start + 1];
            optionsarray[2] = STRL_optionsDB[start + 2];
            optionsarray[3] = STRL_optionsDB[start + 3];
            optionsarray[4] = STRL_optionsDB[start + 4];
            optionsarray[5] = STRL_optionsDB[start + 5];
        }

    }

    void THI_splitoptions()
    {

        I_totalQuestionCount = STRL_questionsDB.Count;

        int startCount = 0;
        int splitCount = 6;

        THI_splitOptions2(STRL_options1, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options2, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options3, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options4, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options5, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options6, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options7, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options8, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options9, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitOptions2(STRL_options10, startCount, splitCount);

    }


    void THI_splitAnswers()
    {

        if (STRL_answersDB.Count > 0)
        {
            string[] a1 = STRL_answersDB[0].Split(',');
            for (int i = 0; i < a1.Length; i++)
            {
                STRL_answers1[i] = a1[i];
            }
        }

        if (STRL_answersDB.Count > 1)
        {
            string[] a2 = STRL_answersDB[1].Split(',');
            for (int i = 0; i < a2.Length; i++)
            {
                STRL_answers2[i] = a2[i];
            }
        }

        if (STRL_answersDB.Count > 2)
        {
            string[] a3 = STRL_answersDB[2].Split(',');
            for (int i = 0; i < a3.Length; i++)
            {
                STRL_answers3[i] = a3[i];
            }
        }


        if (STRL_answersDB.Count > 3)
        {
            string[] a4 = STRL_answersDB[3].Split(',');
            for (int i = 0; i < a4.Length; i++)
            {
                STRL_answers4[i] = a4[i];
            }
        }

        if (STRL_answersDB.Count > 4)
        {
            string[] a5 = STRL_answersDB[4].Split(',');
            for (int i = 0; i < a5.Length; i++)
            {
                STRL_answers5[i] = a5[i];
            }
        }
        if (STRL_answersDB.Count > 5)
        {

            string[] a6 = STRL_answersDB[5].Split(',');
            for (int i = 0; i < a6.Length; i++)
            {
                STRL_answers6[i] = a6[i];
            }
        }

        if (STRL_answersDB.Count > 6)
        {

            string[] a7 = STRL_answersDB[6].Split(',');
            for (int i = 0; i < a7.Length; i++)
            {
                STRL_answers7[i] = a7[i];
            }
        }


        if (STRL_answersDB.Count > 7)
        {

            string[] a8 = STRL_answersDB[7].Split(',');
            for (int i = 0; i < a8.Length; i++)
            {
                STRL_answers8[i] = a8[i];
            }
        }

        if (STRL_answersDB.Count > 8)
        {

            string[] a9 = STRL_answersDB[8].Split(',');
            for (int i = 0; i < a9.Length; i++)
            {
                STRL_answers9[i] = a9[i];
            }
        }

        if (STRL_answersDB.Count > 9)
        {

            string[] a10 = STRL_answersDB[9].Split(',');
            for (int i = 0; i < a10.Length; i++)
            {
                STRL_answers10[i] = a10[i];
            }
        }
    }


    void THI_assignOptions()
    {
        G_caterpillarHead.GetComponent<Animator>().Play("idle");
        THI_splitoptions(); THI_splitAnswers();
        for(int i = 0; i <GA_answersObject.Length;i++)
        {
            GA_answersObject[i].GetComponent<Collider2D>().enabled = true;
        }
        STR_currentSelectionAnswer = "";
        G_caterpillarHead.GetComponent<Animator>().Play("idle");
        int qcount = I_qCount + 1;
        TEX_qCount.text = qcount + "/" + I_totalQuestionCount;
        I_matches = 0;
        STR_currentQuestionID = STRL_questionID[I_qCount];
        if (I_qCount == 0)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options1[i];
                GL_optionsBubble[i].name = STRL_answers1[i];
            }
        }
        if (I_qCount == 1)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options2[i];
                GL_optionsBubble[i].name = STRL_answers2[i];
            }
        }
        if (I_qCount == 2)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options3[i];
                GL_optionsBubble[i].name = STRL_answers3[i];
            }
        }
        if (I_qCount == 3)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options4[i];
                GL_optionsBubble[i].name = STRL_answers4[i];
            }
        }
        if (I_qCount == 4)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options5[i];
                GL_optionsBubble[i].name = STRL_answers5[i];
            }
        }
        if (I_qCount == 5)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options6[i];
                GL_optionsBubble[i].name = STRL_answers6[i];
            }
        }
        if (I_qCount == 6)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options7[i];
                GL_optionsBubble[i].name = STRL_answers7[i];
            }
        }
        if (I_qCount == 7)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options8[i];
                GL_optionsBubble[i].name = STRL_answers8[i];
            }
        }
        if (I_qCount == 8)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options9[i];
                GL_optionsBubble[i].name = STRL_answers9[i];
            }
        }
        if (I_qCount == 9)
        {
            for (int i = 0; i < GL_optionsBubble.Count; i++)
            {
                GL_optionsBubble[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options10[i];
                GL_optionsBubble[i].name = STRL_answers10[i];
            }
        }
        THI_assignAudios();
    }

    void THI_gameData()
    {
        //THI_getPreviewData();
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

    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios);
        STR_difficulty = STRL_difficulty[0];
        StartCoroutine(EN_getAudioClips());
        THI_assignOptions();
        Debug.Log("Preview mode");
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
          //  Debug.Log("GAME DATA: " + www.downloadHandler.text);
            MyJSON json = new MyJSON();
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios);
            StartCoroutine(EN_getAudioClips());
            STR_difficulty = STRL_difficulty[0];
            THI_assignOptions();
         //   Debug.Log("live mode");
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

    public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_questionAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

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

                {
                    ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www);
//                    Debug.Log("audio clips fetched questions");
                }
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


                {
                    ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www);
//                    Debug.Log("audio clips fetched options");
                }
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
                {
                    ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                    Debug.Log("audio clips fetched instruction");
                }
            }
        }

        THI_assignAudioClips();
    }

    public void THI_assignAudioClips()
    {
        //    Debug.Log("question clips length == " + ACA__questionClips.Length);
        //      Debug.Log("options clips length == " + ACA_optionClips.Length);
        //        Debug.Log("instruction clips length == " + ACA_instructionClips.Length);

        //for (int i = 0; i < ACA__questionClips.Length; i++)
        //{
        //    if (GL_questionText[i] != null)
        //    {
        //        GL_questionText[i].gameObject.AddComponent<AudioSource>();
        //        GL_questionText[i].gameObject.GetComponent<AudioSource>().playOnAwake = false;
        //        GL_questionText[i].gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[i];
        //        GL_questionText[i].gameObject.AddComponent<Button>();
        //        GL_questionText[i].gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        //    }
        //}
        for (int i = 0; i < GL_optionsBubble.Count; i++)
        {
            if (GL_optionsBubble[i] != null)
            {

            
                GL_optionsBubble[i].GetComponent<Button>().onClick.AddListener(THI_playAudio);
            }
        }
      
        THI_splitAudios();
        THI_assignAudios();
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }
//        Debug.Log("audio clips assigned to objects");
    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    void THI_assignAudios()
    {
        for (int i = 0; i < GL_optionsBubble.Count; i++)
        {
            if (GL_optionsBubble[i] != null)
            {
                if (I_qCount == 0)
                {
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip1[i];
                    Debug.Log("audios assigned");
                }

                if (I_qCount == 1)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip2[i];

                if (I_qCount == 2)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip3[i];

                if (I_qCount == 3)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip4[i];

                if (I_qCount == 4)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip5[i];

                if (I_qCount == 5)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip6[i];

                if (I_qCount == 6)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip7[i];

                if (I_qCount == 7)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip8[i];

                if (I_qCount == 8)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip9[i];

                if (I_qCount == 9)
                    GL_optionsBubble[i].GetComponent<AudioSource>().clip = ACA_clip10[i];


            }
        }
    }
    void THI_splitAudios2(AudioClip[] clips, int start, int end)
    {
        if (end <= STRL_optionsDB.Count)
        {
          //  Debug.Log(start + "-" + end);
            clips[0] = ACA_optionClips[start];
            clips[1] = ACA_optionClips[start + 1];
            clips[2] = ACA_optionClips[start + 2];
            clips[3] = ACA_optionClips[start + 3];
            clips[4] = ACA_optionClips[start + 4];
            clips[5] = ACA_optionClips[start + 5];
        }

    }
    void THI_splitAudios()
    {

        I_totalQuestionCount = STRL_questionsDB.Count;

        int startCount = 0;
        int splitCount = 6;

        THI_splitAudios2(ACA_clip1, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip2, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip3, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip4, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip5, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip6, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip7, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip8, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip9, startCount, splitCount);
        startCount = splitCount;
        splitCount = splitCount + 6;

        THI_splitAudios2(ACA_clip10, startCount, splitCount);

    }
}
