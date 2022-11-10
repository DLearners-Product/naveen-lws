using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class FiremanController : MonoBehaviour
{

    public bool B_production;

    [Header("Instance")]
    public static FiremanController instance;

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
    public List<Sprite> SPRL_optionsSprite;
    public List<string> STRL_instruction;
    public int I_correctPoints;
    public int I_wrongPoints;

    [Header("Questions")]
    public string STR_currentQuestionID;
    public int I_wrongAnsCount;
    public int I_points;
    public Text TEX_points;
    public Text TEX_questionCount;
    public string STR_currentQuestion;
    public string STR_currentCorrectAnswer;
    public string STR_clickedAnswer;
    public int I_currentQuestionCount;
    public int I_currentOptionReqCount;
    public int I_lastOptionReqCount;
    public GameObject G_questionScreen;
    public Text TEX_currentQuestion;
    public GameObject G_ansDisplay;
    public Image IM_answerDisplay;
    public Text TEX_annswerDisplay;
    public GameObject[] GA_options;
    public GameObject G_levelComp;
    int I_1stSprite;
    int I_2ndSprite;
    int I_3rdSprite;
    string STR_1stOptionName;
    string STR_2ndOptionName;
    string STR_3rdOptionName;
    bool B_called;
    int z;
   

    [Header("Movement Logics")]
    public bool B_moveRight;
    public bool B_moveLeft;
    public float F_moveSpeed;

    [Header("Climbing Logics")]
    public float F_climbSpeed;
    public GameObject G_ladderPrefab;
    public GameObject G_currentLadder;
    public bool B_canClimb;

    [Header("Extinguish")]
    public int I_fireCount;
    public int I_fire;
    public bool B_floorCleared;
    public GameObject G_currentPlatform;
    public GameObject G_ladderButton;
    public GameObject G_extinguishButton;
    public GameObject G_coinPrefab;
    public GameObject G_extinguishFXPrefab;
    public Transform T_extinguisherPosright;
    public Transform T_extinguisherPosleft;

    [Header("Health")]
    public float F_maxHealth;
    public float F_currentHealth;
    public Image IM_health;
    public bool B_dead;
    public Vector2 SpawnPos;

    [Header("Baby")]
    public GameObject G_rope;
    public Transform T_ropeStartPos;
    public Transform T_ropeStopPos;
    public bool B_ropeStart;
    public float F_ropeLerpTimer;

    [Header("Initialization")]
    public AnimationClip AC_introCam;
    public AnimationClip AC_controlsAnim;
    public GameObject G_controlButtons;
    public GameObject G_dog;
    public bool B_dogRun;
    public float F_dogSpeed;

    [Header("Bird")]
    public GameObject G_bird1;
    public GameObject G_bird2;
    public GameObject G_currentBird;
    public Vector2 V_birdStart;
    public Vector2 V_birdEnd;
    public bool B_birdFly;

    [Header("Audios")]
    public AudioSource AS_run;
    public AudioSource AS_baby;
    public AudioSource AS_siren;
    public AudioSource AS_correct;
    public AudioSource AS_wrong;
    public AudioSource AS_heartCollect;
    public AudioSource AS_coinCollect;
    public AudioSource AS_extinguish;
    public AudioSource AS_dead;
    public AudioSource AS_ladderDeploy;
    public AudioSource AS_dogbark;
    public AudioSource AS_BGM;


    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;

    [Header("Instruction")]
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;

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

    void Start()
    {
      
        Camera.main.GetComponent<Animator>().enabled = false;
        SpawnPos = transform.position;
        G_instructionPage.SetActive(false);
        F_maxHealth = F_currentHealth = 100f;
        I_currentQuestionCount = -1;
        int curqcount = I_currentQuestionCount + 1;
        TEX_questionCount.text = "" + curqcount + "/" + I_totalQuestionCount;
        TEX_points.text = "0";
        IM_health.fillAmount = F_currentHealth / F_maxHealth;
        B_moveRight = B_moveLeft = B_canClimb = false;
        G_controlButtons.SetActive(false);
        Invoke("THI_gameData", 1f);
    }

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject==MainController.instance.G_coverPageStart && !Camera.main.GetComponent<Animator>().enabled)
        {
            Camera.main.GetComponent<Animator>().enabled = true;
            Invoke("THI_enableControlButtons", AC_introCam.length);
           
        }

        THI_keyboardControls();

        if (B_birdFly && G_currentBird != null)
        {
            G_currentBird.transform.position = Vector3.MoveTowards(G_currentBird.transform.position, V_birdEnd, 0.075f);
        }
        if (B_dogRun && G_dog != null)
        {
            G_dog.transform.Translate(Vector2.left * F_dogSpeed * Time.deltaTime);
        }

        if (!B_dead && !B_ropeStart)
        {
            if (B_moveRight)
            {
               
                transform.Translate(Vector2.right * F_moveSpeed * Time.deltaTime);
                GetComponent<Animator>().Play("firemanrun");
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (B_moveLeft)
            {
              
                transform.Translate(Vector2.left * F_moveSpeed * Time.deltaTime);
                GetComponent<Animator>().Play("firemanrun");
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                
                if (!B_canClimb && !B_dead && !B_ropeStart)
                {
                    GetComponent<Animator>().Play("firemanidle");
                }
            }
            if (B_canClimb)
            {
             
                GetComponent<Rigidbody2D>().isKinematic = true;
                transform.Translate(Vector2.up * F_climbSpeed * Time.deltaTime);
                GetComponent<Animator>().Play("firemanclimb");
            }
            else
            {
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }

        if (B_ropeStart)
        {
            transform.position = Vector3.Lerp(transform.position, T_ropeStopPos.position, F_ropeLerpTimer);
        }
    }

    void THI_enableControlButtons()
    {
        G_controlButtons.SetActive(true);
        AS_baby.Stop();
        AS_siren.Play();
        Invoke("THI_enableClickonButtons", AC_controlsAnim.length);
    }

    void THI_enableClickonButtons()
    {
        Button[] controlbuttons = G_controlButtons.GetComponentsInChildren<Button>();
        for (int i = 0; i < controlbuttons.Length; i++)
        {
            controlbuttons[i].enabled = true;
        }
        B_dogRun = true;
        AS_dogbark.Play();
        Invoke("stopExtraAudios", 10f);
        StartCoroutine(EN_birdfly());
    }

   void stopExtraAudios()
    {
        AS_dogbark.Stop();
        AS_siren.Stop();
    }

    void THI_keyboardControls()
    {
        if (!B_dead && !B_ropeStart)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                B_moveRight = true;
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                B_moveRight = false;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                B_moveLeft = true;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                B_moveLeft = false;
            }
        }
    }

    IEnumerator EN_birdfly()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            THI_birdSpawn();
        }
    }

    void THI_birdSpawn()
    {
        int randomBird = Random.Range(1, 3);
        if (randomBird == 1)
        {
            G_currentBird = Instantiate(G_bird1);
        }
        if (randomBird == 2)
        {
            G_currentBird = Instantiate(G_bird2);
        }
        int randomYpos = Random.Range(-3, 4);
        V_birdStart = new Vector2(transform.position.x - 7.5f, transform.position.y + randomYpos);
        V_birdEnd = new Vector2(transform.position.x + 15f, transform.position.y + randomYpos);
        G_currentBird.transform.position = V_birdStart;
        B_birdFly = true;
    }

    public void BUT_spawnLadder()
    {
        if (!B_dead)
        {
            AS_ladderDeploy.Play();

            if (G_currentLadder != null)
                Destroy(G_currentLadder);
            G_currentLadder = Instantiate(G_ladderPrefab);
            if (!GetComponent<SpriteRenderer>().flipX)
                G_currentLadder.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 1.25f);

            if (GetComponent<SpriteRenderer>().flipX)
                G_currentLadder.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 1.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == G_currentLadder) // ladder
        {
            B_canClimb = true;
        }
        if (collision.gameObject.tag == "DL_coin") // coin   
        {
            AS_coinCollect.Play();
            I_points = I_points + 1;
            TEX_points.text = I_points.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Heart")
        {
            AS_heartCollect.Play();
            F_currentHealth = F_maxHealth;
            IM_health.fillAmount = F_currentHealth / F_maxHealth;
            IM_health.color = Color.green;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Baby")
        {
            G_rope.SetActive(true);
            GetComponent<Animator>().Play("firemanrope");
            transform.position = T_ropeStartPos.position;
            B_ropeStart = true;
            B_moveLeft = B_moveRight = false;
            B_canClimb = false;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(collision.gameObject);
            G_controlButtons.SetActive(false);
            Invoke("levelcomplete", 4f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == G_currentLadder)
        {
            B_canClimb = false;
        }
        if (collision.gameObject.name == "Fire")
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fire")
        {
            if (F_currentHealth > 75f)
            {
                IM_health.color = Color.green;
            }
            if (F_currentHealth < 75f && F_currentHealth > 35f)
            {
                IM_health.color = Color.yellow;
            }
            if (F_currentHealth < 35f)
            {
                IM_health.color = Color.red;
            }
            if (F_currentHealth > 0)
            {
                F_currentHealth--;
                IM_health.fillAmount = F_currentHealth / F_maxHealth;
            }
            else
            {
                B_dead = true;
                AS_dead.Play();
                GetComponent<Collider2D>().enabled = false;
                F_currentHealth = 0;
                IM_health.transform.parent.transform.parent.gameObject.SetActive(false);
                GetComponent<Animator>().Play("firemandie");
                GetComponent<SpriteRenderer>().color = Color.white;
                Invoke("THI_respawn", 2f);
                return;
            }
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void THI_respawn()
    {
        B_dead = false;
        GetComponent<Collider2D>().enabled = true;
        F_currentHealth = 100f;
        IM_health.fillAmount = F_currentHealth / F_maxHealth;
        IM_health.color = Color.green;
        IM_health.transform.parent.transform.parent.gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = Color.white;
        B_moveLeft = B_moveRight = false;
        G_ladderButton.GetComponent<Button>().interactable = true;
        transform.position = SpawnPos;
        I_points = 0;
        TEX_points.text = I_points.ToString(); 
    }

    public void BUT_extinguishFire()
    {
        if (!B_dead)
        {
            AS_extinguish.Play();
            var extinguishSmoke = Instantiate(G_extinguishFXPrefab);
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                extinguishSmoke.transform.position = T_extinguisherPosright.position;
            }
            else
            {
                extinguishSmoke.transform.position = T_extinguisherPosleft.position;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "fireman_platform")
        {
            I_fire = 0;
            G_currentPlatform = collision.gameObject;
            collision.gameObject.GetComponent<fireman_platform>().enabled = true;
            I_fireCount = collision.gameObject.GetComponent<fireman_platform>().I_fireCount;
            B_floorCleared = collision.gameObject.GetComponent<fireman_platform>().B_platformCleared;
            if (B_floorCleared)
            {
                G_ladderButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                G_ladderButton.GetComponent<Button>().interactable = false;
            }
        }
    }
    void THI_gameData()
    {
       // THI_getPreviewData();
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
            STRL_optionsSpriteName = STRL_optionsDB;
            I_totalQuestionCount = IL_numberValues[0];
            I_correctPoints = IL_numberValues[1];
            I_wrongPoints = IL_numberValues[2];
            MainController.instance.I_TotalQuestions = I_totalQuestionCount;
            MainController.instance.I_correctPoints = I_correctPoints;
            StartCoroutine(EN_getAudioClips());
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
            if (www.result == UnityWebRequest.Result.ConnectionError ||www.isHttpError || www.isNetworkError)
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
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
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
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
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
        TEX_currentQuestion.gameObject.AddComponent<AudioSource>();
        TEX_currentQuestion.gameObject.GetComponent<AudioSource>().playOnAwake = false;
      
        TEX_currentQuestion.gameObject.AddComponent<Button>();
        TEX_currentQuestion.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);


        for (int i = 0; i < GA_options.Length; i++)
        {
            GA_options[i].gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
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
        StartCoroutine(IN_downloadOptionImages());
        STRL_optionsSpriteName = STRL_optionsDB;
        I_totalQuestionCount = IL_numberValues[0];
        I_correctPoints = IL_numberValues[1];
        I_wrongPoints = IL_numberValues[2];
        MainController.instance.I_TotalQuestions = I_totalQuestionCount;
        MainController.instance.I_correctPoints = I_correctPoints;
        StartCoroutine(EN_getAudioClips());
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

    void THI_disableAnswerDisplay()
    {
        G_ansDisplay.SetActive(false);
    }

    public void BUT_optionClick()
    {
        string clickedanswer = EventSystem.current.currentSelectedGameObject.name;
        string[] clickedanswersplit = clickedanswer.Split('/');
        int lastelement = clickedanswersplit.Length - 1;
        string lastelementanswer = clickedanswersplit[lastelement];
        string[] lastelementsplit = lastelementanswer.Split('.');
        STR_clickedAnswer = lastelementsplit[0];

        if (STR_clickedAnswer.Contains(STR_currentCorrectAnswer))
        {
            //correct
            AS_correct.Play();
            I_points = I_points + I_correctPoints;
            TEX_points.text = I_points.ToString();
            G_ansDisplay.SetActive(true);
            IM_answerDisplay.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
            IM_answerDisplay.preserveAspect = true;
            TEX_annswerDisplay.text = STR_currentCorrectAnswer;
            THI_TrackGameData("1");
            Invoke("disableAnswer", 3f);
        }
        else
        {
            //wrong
            AS_wrong.Play();
            THI_TrackGameData("0");
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
            TEX_points.text = I_points.ToString();


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

                    Invoke("disableAnswer", 2.5f);

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
                    Invoke("THI_disableAnswerDisplay", 2.5f);
                }
            }
            if (I_wrongAnsCount == 2)
            {
                if (STR_difficulty == "independent")
                {
                    // dont show ans and go to next question
                    disableAnswer();

                }
            }
        }
    }
    void levelcomplete()
    {
    
        MainController.instance.I_TotalPoints = I_points;
        G_levelComp.SetActive(true);
        AS_BGM.Stop();
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(IN_sendDataToDB());
        }
    }
    void disableAnswer()
    {
        G_questionScreen.SetActive(false);
    }

    public void THI_showQuestion()
    {
        I_wrongAnsCount = 0;
        G_questionScreen.SetActive(true);
        G_ansDisplay.SetActive(false);
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
            TEX_currentQuestion.text = STR_currentQuestion;
            TEX_currentQuestion.gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

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


            GA_options[0].gameObject.GetComponent<AudioSource>().clip = ACA_optionClips[I_1stSprite];
            GA_options[1].gameObject.GetComponent<AudioSource>().clip = ACA_optionClips[I_2ndSprite];
            GA_options[2].gameObject.GetComponent<AudioSource>().clip = ACA_optionClips[I_3rdSprite];



            I_lastOptionReqCount = I_currentOptionReqCount;

        }

    }

    public void THI_TrackGameData(string analysis)
    {
        DBmanager firemanDB = new DBmanager();
        firemanDB.question_id = STR_currentQuestionID;
        firemanDB.answer = STR_clickedAnswer;
        firemanDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(firemanDB);
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
    

