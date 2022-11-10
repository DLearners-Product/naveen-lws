using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PassageClickManager : MonoBehaviour
{

    public bool B_production;

    [Header("Instance")]
    public static PassageClickManager instance;

    [Header("Gameplay")]
    public string STR_clickedWord;
    public string[] STRA_specialCharacters;
    public TextMeshProUGUI TM_passage;
    public int I_qCount;
    public TextMeshProUGUI TM_question;

    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("DB")]
    public string STR_passage;
    public List<string> STRL_questions;
    public List<string> STRL_answers;
    public int I_correctPoints;
    public int I_wrongPoints;
    

    [Header("CAMEL")]
    public Sprite SPR_treasureOpen;
    public int I_numberofchesttocollect;
    public int I_TreasureChestCollected;
    public int I_cloneCount;
    public GameObject G_passageQuestion;
    public GameObject G_treasurePrefab;
    public GameObject G_weedPrefab;
    public GameObject[] GA_weedPos;
    public GameObject[] GA_treasureChestPos;
    public GameObject G_currentTreasureChest;
    public AnimationClip AC_questionPassageInv;
    public int I_points;
    public Text TEX_points;
    public Text TEX_qCount;
    public bool B_camelOver;
    public GameObject G_gameComplete;



    [Header("Audios")]
    public AudioSource AS_coin;
    public AudioSource AS_wrong;
    public AudioSource AS_run;
    public AudioSource AS_jump;
    public AudioSource AS_weedHit;
    public AudioSource AS_BGM;


    private void Awake()
    {
        instance = this;


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
    private void Start()
    {
        I_qCount = -1;
        StartCoroutine(EN_getValues());
    }

    void THI_assignPassage()
    {
        TM_passage.text = STR_passage;
    }

    public IEnumerator EN_getValues()
    {

        WWWForm form = new WWWForm();
        form.AddField("game_id", "133");
        // Debug.Log("GAME ID : " + MainController.instance.STR_GameID);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {

            MyJSON json = new MyJSON();
            json.PassageClickTemp(www.downloadHandler.text);
            THI_assignPassage();
            THI_showQuestion();
        }
    }


    public void THI_showQuestionDelay()
    {
        Invoke("THI_showQuestion", AC_questionPassageInv.length-1f);
    }

    public void THI_showQuestion()
    {
        I_qCount++;
        TEX_qCount.text = I_qCount + "/" + STRL_questions.Count;
        if(I_qCount<STRL_questions.Count)
        {
            TM_question.text = STRL_questions[I_qCount];
            //G_passageQuestion.SetActive(false);
          //  if(G_currentTreasureChest!=null)
          //  Destroy(G_currentTreasureChest);
          if(!B_camelOver)
            THI_cloneweedandtreasure();
        }
        else
        {
            G_gameComplete.SetActive(true);
            AS_BGM.Stop();
            Debug.Log("Level  Complete!");
        }
    }

    public void THI_cloneweedandtreasure()
    {
        I_cloneCount++;

        if (I_cloneCount == 1)
        {
            var weed = Instantiate(G_weedPrefab);
            int randomposweed = Random.Range(0, GA_weedPos.Length);
            weed.transform.position = GA_weedPos[randomposweed].transform.position;         
        }
        var treasureChest = Instantiate(G_treasurePrefab);
        int randomposTreasure = Random.Range(0, GA_treasureChestPos.Length);
        treasureChest.transform.position = GA_treasureChestPos[randomposTreasure].transform.position;
        G_currentTreasureChest = treasureChest;

    }

}
