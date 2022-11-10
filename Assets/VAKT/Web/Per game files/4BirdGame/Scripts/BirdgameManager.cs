using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Networking;

public class BirdgameManager : MonoBehaviour
{
    [Header("Instance")]
    public static BirdgameManager instance;


    public bool B_production;

    [Header("Movement")]
    public GameObject G_HeroBird;
    public Animator AN_heroBird;
    public float F_moveSpeed;
    public float F_jumpForce;
    public bool B_dead;
    Transform T_CameraFollow;
    GameObject G_camera;
    Vector3 V3_startPos;
    public Vector3 V3_diePos;

    [Header("Enemy")]
    public int I_enemiesIG;
    public GameObject[] GA_enemiesPrefabs;
    public bool B_insideMortarRange;
    public float F_mortarTimer;
    public Text TEX_mortarTimer;

    [Header("Points")]
    public int I_points;
    public Text Tex_points;


    [Header("DB")]
    public List<string> STRL_difficulty;
    public string STR_difficulty;
    public string URL;
    public string SendValueURL;
    public List<int> IL_numberValues;
    public List<string> STRL_questionsDB;
    public List<string> STRL_optionsDB;
    public List<string> STRL_answersDB;
    public List<string> STRL_instruction;
    public int I_totalQuestionCount;
    public int I_correctPoints;
    public int I_wrongPoints;

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;
    public List<string> STRL_questionID;

    [Header("Instruction")]
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;

    [Header("Other")]
    public string STR_currentQuestionID;
    public string STR_currentCorrectAnswer;
    public string STR_currentSelectionAnswer;
    public List<Text> GL_questionText;
    public List<Text> GL_optionText;
    public int I_questionCount;
    public int I_wrongAnsCount;
    public Text TEX_questionCount;
    public List<GameObject> GL_questions;
    public GameObject G_question;
    public GameObject G_levelComplete;
    public GameObject G_respawnCheckpoint;
    public GameObject G_cancelRespawnCheckpoint;
    public GameObject G_currentQuestion;
    public List<GameObject> GL_enemiesIG;
    public GameObject G_startScreen;
    public GameObject G_blimp;


    [Header("Init")]
    public bool B_gameStart;
    public bool B_called;
    public GameObject G_backstory;
    public GameObject G_demo;
    public AnimationClip AC_backstory;
    public AnimationClip AC_demo;
    public AnimationClip AC_camera;

    [Header("Audios")]
    public AudioSource AS_evilBirdSpawn;
    public AudioSource AS_evilBirdHit;
    public AudioSource AS_flyPress;
    public AudioSource AS_flyRelease;
    public AudioSource AS_groundHit;
    public AudioSource AS_mortarSuck;
    public AudioSource AS_correct;
    public AudioSource AS_wrong;
    public AudioSource AS_BGM;


    [Header("AUDIO DB")]
    public List<string> STRL_questionAudios;
    public List<string> STRL_optionsAudios;
    public List<string> STRL_instructionAudios;

    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;



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

    private void Start()
    {
        G_HeroBird = GameObject.FindGameObjectWithTag("HeroBird");
        T_CameraFollow = G_HeroBird.transform.GetChild(0).transform;
        G_camera = GameObject.FindGameObjectWithTag("MainCamera");
        AN_heroBird = G_HeroBird.GetComponent<Animator>();
        V3_startPos = G_HeroBird.transform.position;
        G_backstory.SetActive(false);
        G_demo.SetActive(false);
        I_questionCount = -1;


        Invoke("THI_gameData", 1f); // live
      //  StartCoroutine(EN_GetData()); // testing
    }

    void THI_onBackstoryComplete()
    {
        G_backstory.SetActive(false);
        G_demo.SetActive(true);
        Invoke("THI_onDemoComplete", AC_demo.length);
    }

    void THI_onDemoComplete()
    {
        G_demo.SetActive(false);
        G_camera.GetComponent<Animator>().enabled = true;
        Invoke("THI_onCameraAnimComplete", AC_camera.length);
    }

    void THI_onCameraAnimComplete()
    {
        G_startScreen.SetActive(true);
        G_blimp.SetActive(true);
        B_gameStart = true;
    }


    public void BUT_startGame()
    {
        F_moveSpeed = 2f;
        F_jumpForce = 10f;
        G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
        G_camera.GetComponent<Animator>().enabled = false;
        StartCoroutine(EN_cloneEnemyDuration());
        Destroy(EventSystem.current.currentSelectedGameObject);
    }

    private void Update()
    {
        if(GameObject.Find("CoverImage")==null && !B_called)
        {
            G_backstory.SetActive(true);
            B_called = true;
            Invoke("THI_onBackstoryComplete", AC_backstory.length);
        }
        if (!B_dead)
        {
            THI_heroBirdMove();
            THI_heroBirdJump();

            if(B_insideMortarRange)
            {
                F_mortarTimer = F_mortarTimer - 1 * Time.deltaTime;
                F_mortarTimer = (float)System.Math.Round(F_mortarTimer, 2);
                TEX_mortarTimer.text = F_mortarTimer.ToString();
              
                if (F_mortarTimer<=0)
                {
                    Debug.Log("pulled by blimp mortar");
                    AS_mortarSuck.Play();
                    V3_diePos = new Vector3(G_HeroBird.transform.position.x, 2.5f);
                    B_dead = true;
                    G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = -1f;
                    TEX_mortarTimer.text = "";
                    G_HeroBird.GetComponent<Collider2D>().enabled = false;
                    THI_destroyEnemies();
                    THI_restart();
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                AS_flyPress.Play();
            }
            if(Input.GetMouseButtonUp(0))
            {
                AS_flyRelease.Play();
            }
        }
    }

    private void FixedUpdate()
    {
       // THI_HerobirdRotation();
    }

    private void LateUpdate()
    {
        if (!B_dead)
        {
            THI_cameraFollow();
        }
    }

    private void THI_heroBirdMove()
    {
        G_HeroBird.transform.Translate(Vector2.right * F_moveSpeed * Time.deltaTime);
    }

    private void THI_heroBirdJump()
    {
        if (Input.GetMouseButton(0))
        {
            G_HeroBird.GetComponent<Rigidbody2D>().AddForce(Vector2.up * F_jumpForce);
        }
    }

    private void THI_cameraFollow()
    {
        if(B_gameStart)
        G_camera.transform.position = new Vector3(T_CameraFollow.position.x, G_camera.transform.position.y, -10f);
    }

    private void THI_HerobirdRotation()
    {
        if (Input.GetMouseButton(0))
        {
            G_HeroBird.GetComponent<Rigidbody2D>().MoveRotation(G_HeroBird.GetComponent<Rigidbody2D>().rotation + 7.5f * Time.fixedDeltaTime);
        }
        else
        {
            G_HeroBird.GetComponent<Rigidbody2D>().MoveRotation(G_HeroBird.GetComponent<Rigidbody2D>().rotation - 5f * Time.fixedDeltaTime);
        }
    }

    private void THI_cloneEnemy()
    {
        if (I_enemiesIG == 0)
        {
            AS_evilBirdSpawn.Play();
            I_enemiesIG =1;
            int randomEnemy = Random.Range(0, GA_enemiesPrefabs.Length);
            var enemy = Instantiate(GA_enemiesPrefabs[randomEnemy]);
            enemy.transform.SetParent(GameObject.Find("Enemies").transform);
            enemy.transform.position = new Vector3(G_HeroBird.transform.position.x + 20f, G_HeroBird.transform.position.y);
            GL_enemiesIG.Add(enemy);
        }
    }

    private IEnumerator EN_cloneEnemyDuration()
    {
        while (!B_dead)
        {
            int randomCloneTime = Random.Range(2, 9);
//            Debug.Log("random clone timer : " + randomCloneTime);
            yield return new WaitForSeconds(randomCloneTime);
            THI_cloneEnemy();
        }
    }
    public void THI_destroyEnemies()
    {
        for(int i = 0; i < GL_enemiesIG.Count; i++)
        {
            if(GL_enemiesIG[i]!=null)
            {
                Destroy(GL_enemiesIG[i]);
            }
        }
        GL_enemiesIG = new List<GameObject>();
    }
    public void THI_restart()
    {
        Invoke("THI_restartGame", 2f);
    }
    void THI_restartGame()
    {
        if (I_points >= 5)
        {
            I_points = I_points - 5;
        }
        else
        {
            I_points = 0;
        }

        if (I_points < 10)
        {
            B_dead = false;
            B_insideMortarRange = false;
            AN_heroBird.Play("fly");
            G_HeroBird.GetComponent<Collider2D>().enabled = true;
            G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
            G_HeroBird.transform.position = V3_startPos;           
        }
        else
        {
            Time.timeScale = 0;
            G_respawnCheckpoint.SetActive(true);
            G_cancelRespawnCheckpoint.SetActive(true);
        }
        Tex_points.text = I_points.ToString();
    }
    public void BUT_respawnCheckpoint()
    {
        I_points = I_points - 10;
        Tex_points.text = I_points.ToString();
        B_dead = false;
        B_insideMortarRange = false;
        AN_heroBird.Play("fly");
        G_HeroBird.GetComponent<Collider2D>().enabled = true;
        G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
        G_HeroBird.transform.position = V3_diePos;
        Time.timeScale = 1;
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        G_cancelRespawnCheckpoint.SetActive(false);
    }
    public void BUT_cancelRespawnCheckpoint()
    {
        B_dead = false;
        B_insideMortarRange = false;
        AN_heroBird.Play("fly");
        G_HeroBird.GetComponent<Collider2D>().enabled = true;
        G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
        G_HeroBird.transform.position = V3_startPos;
        Time.timeScale = 1;
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        G_respawnCheckpoint.SetActive(false);
    }

    public void BUT_cancelRespawn()
    {
        B_dead = false;
        B_insideMortarRange = false;
        AN_heroBird.Play("fly");
        G_HeroBird.GetComponent<Collider2D>().enabled = true;
        G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
        G_HeroBird.transform.position = V3_startPos;
        Time.timeScale = 1;
    }
    void THI_gameData()
    {
    //  THI_getPreviewData();
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
//            Debug.Log("GAME DATA: " + www.downloadHandler.text);
            MyJSON json = new MyJSON();
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios);
            STR_difficulty = STRL_difficulty[0];
            StartCoroutine(EN_getAudioClips());
            THI_assignValues();
        }
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
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (DownloadHandlerAudioClip.GetContent(www) != null)
                {
                    ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }

        for (int i = 0; i < STRL_optionsAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_optionsAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (DownloadHandlerAudioClip.GetContent(www) != null)
                {
                    ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }

        for (int i = 0; i < STRL_instructionAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_instructionAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (DownloadHandlerAudioClip.GetContent(www) != null)
                {
                    ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }
        THI_assignAudioClips();
    }
    public void THI_assignAudioClips()
    {
        for (int i = 0; i < GL_questionText.Count; i++)
        {
            GL_questionText[i].gameObject.AddComponent<AudioSource>();
            GL_questionText[i].gameObject.GetComponent<AudioSource>().playOnAwake = false;
            GL_questionText[i].gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[i];
            GL_questionText[i].gameObject.AddComponent<Button>();
            GL_questionText[i].gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }
        for (int i = 0; i < GL_optionText.Count; i++)
        {
            GL_optionText[i].gameObject.transform.parent.gameObject.AddComponent<AudioSource>();
            GL_optionText[i].gameObject.transform.parent.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            GL_optionText[i].gameObject.transform.parent.gameObject.GetComponent<AudioSource>().clip = ACA_optionClips[i];
            GL_optionText[i].gameObject.transform.parent.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }

    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
    }
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios);
        STR_difficulty = STRL_difficulty[0];
        StartCoroutine(EN_getAudioClips());
        THI_assignValues();
    }
    void THI_assignValues()
    {

        I_totalQuestionCount = IL_numberValues[0];
        I_correctPoints = IL_numberValues[1];
        I_wrongPoints = IL_numberValues[2];

        MainController.instance.I_correctPoints = I_correctPoints;
        MainController.instance.I_TotalQuestions = I_totalQuestionCount;

        for (int i = 0; i < STRL_questionsDB.Count; i++)
        {
            GL_questionText[i].text = STRL_questionsDB[i];
        }
        for (int i = 0; i < STRL_optionsDB.Count; i++)
        {
            GL_optionText[i].text = STRL_optionsDB[i];
        }
        int qcount = I_questionCount + 1;
        TEX_questionCount.text = qcount + "/" + I_totalQuestionCount;
    }
    public void BUT_selectAnswer()
    {
        STR_currentSelectionAnswer = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
        if (STR_currentSelectionAnswer == STR_currentCorrectAnswer)
        {
            THI_TrackGameData("1");

            I_points += I_correctPoints;
            AS_correct.Play();
            G_question.SetActive(false);
            G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
            F_moveSpeed = 1.5f;
            F_jumpForce = 10f;
            Time.timeScale = 1;
            G_currentQuestion.GetComponent<Animator>().enabled = true;
            //    THI_checkLevelComplete();
        }
        else
        {
            AS_wrong.Play();
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

            I_wrongAnsCount++;
            if (I_wrongAnsCount == 3)
            {
                if (STR_difficulty == "assistive")
                {
                    // show ans and go to next question
                    for (int i = 0; i < GL_optionText.Count; i++)
                    {
                        if (GL_optionText[i].text == STRL_answersDB[I_questionCount])
                        {
                            GL_optionText[i].color = Color.green;
                            GL_optionText[i].transform.parent.GetComponent<Animator>().enabled = true;
                            GL_optionText[i].transform.parent.GetComponent<Button>().enabled = false;
                        }
                        else
                        {
                            GL_optionText[i].transform.parent.gameObject.SetActive(false);
                        }
                    }
                    StartCoroutine(EN_assisstiveOffDelay());
                    //   THI_checkLevelComplete();

                }
                if (STR_difficulty == "intuitive")
                {
                    // show ans and make the child click to go to next question
                    for (int i = 0; i < GL_optionText.Count; i++)
                    {
                        if (GL_optionText[i].text == STRL_answersDB[I_questionCount])
                        {
                            GL_optionText[i].color = Color.green;
                            GL_optionText[i].transform.parent.GetComponent<Animator>().enabled = true;
                        }
                        else
                        {
                            GL_optionText[i].transform.parent.gameObject.SetActive(false);
                        }
                    }

                }
            }
            if (I_wrongAnsCount == 2)
            {
                if (STR_difficulty == "independent")
                {
                    // dont show ans and go to next question
                    G_question.SetActive(false);
                    G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
                    F_moveSpeed = 1.5f;
                    F_jumpForce = 10f;
                    Time.timeScale = 1;
                    G_currentQuestion.GetComponent<Animator>().enabled = true;
                    //   THI_checkLevelComplete();

                }
            }
        }
        Tex_points.text = I_points.ToString();
    }

    IEnumerator EN_assisstiveOffDelay()
    {
        yield return new WaitForSecondsRealtime(2);
        G_currentQuestion.GetComponent<Animator>().enabled = true;
        G_question.SetActive(false);
        Time.timeScale = 1;
        G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0.45f;
        F_moveSpeed = 1.5f;
        F_jumpForce = 10f;
        StopCoroutine(EN_assisstiveOffDelay());

    }

    //void THI_assisstiveOffDelay()
    //{
    //    G_currentQuestion.GetComponent<Animator>().enabled = true;
    //    G_question.SetActive(false);
    //    Time.timeScale = 1;
    //}

    //void THI_checkLevelComplete()
    //{
    //    if (I_questionCount == I_totalQuestionCount - 1)
    //    {
    //        THI_gameComp();
    //    }
    //}
    public void THI_delayGameComplete()
    {
    //    Invoke("THI_stopBirdFly", 2f);
        Invoke("THI_gameComp", 3f);
    }
    void THI_stopBirdFly()
    {
        F_moveSpeed = 0;
    }
    void THI_gameComp()
    {
        AS_BGM.Stop();
        MainController.instance.I_TotalPoints = I_points;
        THI_stopBirdFly();
        G_levelComplete.SetActive(true);
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(IN_sendDataToDB());
        }
    }
    public void THI_TrackGameData(string analysis)
    {
        DBmanager birdgameDB = new DBmanager();
        birdgameDB.question_id = STR_currentQuestionID;
        birdgameDB.answer = STR_currentSelectionAnswer;
        birdgameDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(birdgameDB);
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
        Time.timeScale = 0;
    }
    public void BUT_speakerInstruction()
    {

    }
    public void BUT_closeInstruction()
    {
        G_instructionPage.SetActive(false);
        Time.timeScale = 1;
    }
    public void THI_showQuestion()
    {
        for (int i = 0; i < GL_optionText.Count; i++)
        {
            GL_optionText[i].transform.parent.gameObject.SetActive(true);
            GL_optionText[i].color = Color.white;
            GL_optionText[i].transform.parent.GetComponent<Animator>().enabled = false;
            GL_optionText[i].transform.parent.GetComponent<Button>().enabled = true;
        }

        I_questionCount++;
        Debug.Log("Q COUNT ; " + I_questionCount);
        I_wrongAnsCount = 0;
        if (I_questionCount < GL_questions.Count)
        {
            Debug.Log("COMING INSIDE THE IF CONDITION ; " + I_questionCount + "/" + GL_questions.Count);
            for (int i = 0; i < GL_questions.Count; i++)
            {
                GL_questions[i].SetActive(false);
            }
            STR_currentCorrectAnswer = STRL_answersDB[I_questionCount];
            STR_currentQuestionID = STRL_questionID[I_questionCount];
            int qcount = I_questionCount + 1;
            TEX_questionCount.text = qcount + "/" + I_totalQuestionCount;
            GL_questions[I_questionCount].SetActive(true);
            Debug.Log("LAST LINE OF THE METHOD" );
        }
    }
}