using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StackGameManager : MonoBehaviour
{
    public bool B_production;

    [Header("Instance")]
    public static StackGameManager instance;

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

    [Header("Stack gameplay")]
    public int I_qCount;
    public GameObject G_groundPrefab;
    public GameObject G_currentGround;
    public GameObject G_groundParentforclone;
    public List<GameObject> GL_clonedGrounds;
    public bool B_isGrounded;
    public GameObject G_hero;
    public float F_jumpForce;
    public float F_groundSpeed;
    public bool B_gamePause;
    public bool B_dead;
    public int I_stackCount;
    public Text TEX_qCount;
    public int I_totalQuestionCount;
    public List<string> STRL_selectedAnswers;
    public string STR_finalSelectedAnswer;
    public int I_wrongAnsCount;
    public int I_points;
    public Text TEX_points;

    [Header("Arrange gameplay")]
    public GameObject G_QuestionPage;
    public GameObject[] GA_options;
    public int I_match;


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
        G_QuestionPage.SetActive(false);
        I_qCount = -1;
        Invoke("THI_gameData", 1f);
    }


    void Update()
    {
        THI_jump(); THI_GroundMove();
    }



    public void THI_groundClone()
    {
        if (!B_gamePause)
        {
            I_stackCount++;
            THI_showQuestion();
            F_groundSpeed = Random.Range(4, 9);
            var clonedGround = Instantiate(G_groundPrefab);
            clonedGround.transform.SetParent(G_groundParentforclone.transform);
            if (G_currentGround != null)
            {
                clonedGround.transform.position = new Vector3(G_currentGround.transform.position.x + 15f, G_currentGround.transform.position.y + 1.2f);
            }
            else
            {
                clonedGround.transform.position = new Vector3(GameObject.Find("GroundStack").transform.position.x + 15f, GameObject.Find("GroundStack").transform.position.y + 1.2f);
            }
            G_currentGround = clonedGround;
            GL_clonedGrounds.Add(G_currentGround);
           
        }
    }

    void THI_jump()
    {
        if(Input.GetMouseButtonDown(0) && B_isGrounded && !B_gamePause)
        {
            G_hero.GetComponent<Rigidbody2D>().AddForce(Vector2.up * F_jumpForce); 
        }
    }

    void THI_GroundMove()
    {
        if (G_currentGround != null && !B_gamePause)
        {
            G_currentGround.transform.Translate(Vector2.left * F_groundSpeed * Time.deltaTime);
        }
    }

    void THI_restart()
    {
        I_stackCount = 0;
        G_hero.GetComponent<Animator>().Play("idle");
        B_gamePause = false;
        B_dead = false;
        G_hero.GetComponent<Collider2D>().enabled = true;
        for (int i = 0; i <GL_clonedGrounds.Count; i++)
        {
            if(GL_clonedGrounds[i]!=null)
            Destroy(GL_clonedGrounds[i]);
        }
        var baseGround = Instantiate(G_groundPrefab);
        baseGround.transform.position = new Vector3(G_hero.transform.position.x-1f, G_hero.transform.position.y - 5f);
        G_currentGround = baseGround;
        GL_clonedGrounds.Add(baseGround);
        THI_groundClone();
    }
    public void THI_delayRestart()
    {
        Invoke("THI_restart", 3f);
    }

    public void THI_showQuestion()
    {
        if (I_stackCount == 10)
        {
            I_stackCount = 0;
            B_gamePause = true;
       
            I_qCount++;
            if (I_qCount <= I_totalQuestionCount)
            {
                STR_currentQuestionID = STRL_questionID[I_qCount];
                STR_finalSelectedAnswer = "";
                STRL_selectedAnswers = new List<string>();
                G_QuestionPage.SetActive(true);
                THI_assignOptions();
              
            }
            else
            {
                Debug.Log("Game Complete");
            }
        }
    }

    public void BUT_selectAnswer()
    {
        //int x;
        //int.TryParse(EventSystem.current.currentSelectedGameObject.name,out x) ;
        //if(x==I_match+1)//correct
        //{
        //    I_match++;
        //    EventSystem.current.currentSelectedGameObject.transform.GetChild(1).gameObject.SetActive(true);
        //    EventSystem.current.currentSelectedGameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = I_match.ToString();
        //    EventSystem.current.currentSelectedGameObject.GetComponent<Button>().enabled = false;
        //    if(I_match==6)
        //    {
        //        Invoke("THI_continueGame", 2f);
        //    }
        //}
        //else //wrong
        //{
        //}

        I_match++;
        EventSystem.current.currentSelectedGameObject.transform.GetChild(1).gameObject.SetActive(true);
        EventSystem.current.currentSelectedGameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = I_match.ToString();
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().enabled = false;
        STRL_selectedAnswers.Add(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text);
        STR_finalSelectedAnswer = string.Join(",", STRL_selectedAnswers);
        if (I_match == 6)
        {
            if (STR_finalSelectedAnswer == STRL_answersDB[I_qCount])
            {
                Invoke("THI_continueGame", 2f);
                I_points = I_points + I_correctPoints;
                TEX_points.text = I_points.ToString();
            }
            else
            {
               
                I_wrongAnsCount++;
                I_match = 0;
                STRL_selectedAnswers = new List<string>();
                STR_finalSelectedAnswer = "";

                for (int i = 0; i < GA_options.Length; i++)
                {
                    GA_options[i].GetComponent<Button>().enabled = true;
                    GA_options[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                if (I_wrongAnsCount == 3)
                {
                    //if (STR_difficulty == "assistive")
                    //{
                    //    for (int i = 0; i < GA_options.Length; i++)
                    //    {
                    //        GA_options[i].GetComponent<Button>().enabled = false;
                    //        GA_options[i].transform.GetChild(1).gameObject.SetActive(false);
                    //    }
                    //    THI_revealAns();
                    //}
                    //if (STR_difficulty == "intuitive")
                    //{
                    //    for (int i = 0; i < GA_options.Length; i++)
                    //    {
                    //        GA_options[i].GetComponent<Button>().enabled = false;
                    //        GA_options[i].transform.GetChild(1).gameObject.SetActive(false);
                    //    }
                    //    THI_revealAns();
                    //}
                }
                if (I_wrongAnsCount == 2)
                {
                    if (STR_difficulty == "independent")
                    {
                        THI_continueGame();
                    }

                }
                //Invoke("THI_continueGame", 2f);
            }
        }
    }
    void THI_revealAns()
    {
        THI_ansOn();
        if (STR_difficulty == "assistive")
        {
            Invoke("THI_continueGame", 2f);
        }
        if(STR_difficulty=="intuitive")
        {
            Invoke("THI_ansOff", 2f);
        }
    }

    void THI_ansOn()
    {
        
    }

    void THI_ansOff()
    {
        for (int i = 0; i < GA_options.Length; i++)
        {
            GA_options[i].GetComponent<Button>().enabled = true;      
        }
    }

    void THI_continueGame()
    {
        I_wrongAnsCount = 0;
        I_match = 0;
        G_QuestionPage.SetActive(false);
        for (int i = 0; i < GA_options.Length; i++)
        {
            GA_options[i].GetComponent<Button>().enabled = true;
            GA_options[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        B_gamePause = false;
       // THI_groundClone();
    }

    void THI_splitOptions2(List<string> optionsarray, int start, int end)
    {
        if (end <= STRL_optionsDB.Count)
        {
//            Debug.Log(start + "-" + end);
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
        THI_splitoptions(); THI_splitAnswers();
  
        STR_currentSelectionAnswer = "";
  
        int qcount = I_qCount + 1;
        TEX_qCount.text = qcount + "/" + I_totalQuestionCount;
        I_match = 0;

  
        if (I_qCount == 0)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options1[i];
                GA_options[i].name = STRL_answers1[i];
            }
        }
        if (I_qCount == 1)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options2[i];
                GA_options[i].name = STRL_answers2[i];
            }
        }
        if (I_qCount == 2)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options3[i];
                GA_options[i].name = STRL_answers3[i];
            }
        }
        if (I_qCount == 3)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options4[i];
                GA_options[i].name = STRL_answers4[i];
            }
        }
        if (I_qCount == 4)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options5[i];
                GA_options[i].name = STRL_answers5[i];
            }
        }
        if (I_qCount == 5)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options6[i];
                GA_options[i].name = STRL_answers6[i];
            }
        }
        if (I_qCount == 6)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options7[i];
                GA_options[i].name = STRL_answers7[i];
            }
        }
        if (I_qCount == 7)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options8[i];
                GA_options[i].name = STRL_answers8[i];
            }
        }
        if (I_qCount == 8)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options9[i];
                GA_options[i].name = STRL_answers9[i];
            }
        }
        if (I_qCount == 9)
        {
            for (int i = 0; i < GA_options.Length; i++)
            {
                GA_options[i].transform.GetChild(0).GetComponent<Text>().text = STRL_options10[i];
                GA_options[i].name = STRL_answers10[i];
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

    public void BUT_startGame()
    {

        THI_restart();
        EventSystem.current.currentSelectedGameObject.SetActive(false);
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
        Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STRL_instruction[0];
    }
    public void BUT_speakerInstruction()
    {

    }
    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
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
           //         Debug.Log("audio clips fetched questions");
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
         //           Debug.Log("audio clips fetched instruction");
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
        //for (int i = 0; i < ACA_optionClips.Length; i++)
        //{
        //    if (GA_options[i] != null)
        //    {

        //        GA_options[i].GetComponent<AudioSource>().clip = ACA_optionClips[i];
        //        GA_options[i].GetComponent<Button>().onClick.AddListener(THI_playAudio);
        //    }
        //}
        for (int i = 0; i < GA_options.Length; i++)
        {
            if (GA_options[i] != null)
            {            
                GA_options[i].GetComponent<Button>().onClick.AddListener(THI_playAudio);
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
        for (int i = 0; i < GA_options.Length; i++)
        {
            if (GA_options[i] != null)
            {
                if (I_qCount == 0)
                {
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip1[i];
                    Debug.Log("audio addded trak");
                }
                if (I_qCount == 1)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip2[i];

                if (I_qCount == 2)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip3[i];

                if (I_qCount == 3)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip4[i];

                if (I_qCount == 4)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip5[i];

                if (I_qCount == 5)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip6[i];

                if (I_qCount == 6)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip7[i];

                if (I_qCount == 7)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip8[i];

                if (I_qCount == 8)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip9[i];

                if (I_qCount == 9)
                    GA_options[i].GetComponent<AudioSource>().clip = ACA_clip10[i];

   
            }
        }
    }

    void THI_splitAudios2(AudioClip[] clips, int start, int end)
    {
        if (end <= STRL_optionsDB.Count)
        {
//            Debug.Log(start + "-" + end);
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
