using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public static MainController instance;


    [Header("PLATFORM")]
    public bool WEB;
    public bool MOBILE;

    [Header("MANAGERS")]
    public GameObject G_GameID;
    public GameObject G_GameManager;

    [Header("OBJECTS")]
    public GameObject G_coverPageStart;
    public Image IM_loading;
    public float startLoad;
    public float maxLoad;
    public GameObject G_coverPage;
    public bool B_enteredGame;

    [Header("ID")]
    public string STR_IDjson;
    public string STR_childID;
    public string STR_GameID;
    public bool called;

    [Header("SCORE")]
    public int I_TotalPoints;
    public int I_TotalQuestions;
    public int I_correctPoints;

    [Header("MODE")]
    public string mode;


    [Header("PREVIEW MODE")]
    public string STR_previewJsonAPI;

    [Header("INITIALIZE")]
    public int I_loadTime;
    public string STR_responseSerial;

    void Awake()
    {
        instance = this;

        if(G_coverPageStart!=null)
        G_coverPageStart.GetComponent<Button>().interactable = false;
        B_enteredGame = false;


        maxLoad = 100f;

#if UNITY_ANDROID || UNITY_IOS
        MOBILE = true;
        WEB = false;
     //   Debug.Log("MOBILE");
#elif UNITY_WEBGL
        MOBILE = false;
        WEB = true;
        //  Debug.Log("WEB");
#endif


        //if (MOBILE)
        //{
        //    STR_GameID = GameManager.instance.STR_selectedGameID;
        //    G_GameManager.SetActive(false);
        //    G_GameID.SetActive(false);
        //}
        if (WEB)
        {
            G_GameManager.SetActive(false);
            G_GameID.SetActive(true);


            //  testing
              STR_childID = "336";
              mode = "live";
           //  mode = "preview"; 

            // Live
            //  STR_GameID = "672"; // Filling que text 
            //  STR_GameID = "605"; // Image match
            //  STR_GameID = "499"; // audio Image
            //  STR_GameID = "608"; // audio text
            //  STR_GameID = "144"; // train 2,3,4X5 text text
            //  STR_GameID = "488"; // fill QIT opt_text
            //  STR_GameID = "335"; // note taking stertargy
            //  STR_GameID = "491"; // 2x5 sorting
            //  STR_GameID = "604"; // 3x3 sorting
            //  STR_GameID = "590"; // jumbled words
            //  STR_GameID = "223"; //Text arrange
            //  STR_GameID = "590"; // Formsentence words
            //  STR_GameID = "606"; // Match drag and drop
            //  STR_GameID = "276"; // Math
            //  STR_GameID = "541"; // WordSearch
            //  STR_GameID = "335"; // Notetaking
            //  STR_GameID = "505"; // II_click
                STR_GameID = "722"; // II_click


            //Live

            //   STR_GameID = "2"; //   text and text
            //   STR_GameID = "133"; //   camel game
            //   STR_GameID = "106"; // caterpillar game, stack game
            //   STR_GameID = "2";  // crate game,bird game
            //   STR_GameID = "10"; // helicopter game
            //   STR_GameID = "13"; // squirrel game,fireman game
            //   STR_GameID = "69"; // white dummy game 1 (img ques / img ans)
            //   STR_GameID = "70"; //white dummy game 2 (img ques / text ans)



            //   STR_GameID = "143"; // worksheet 1
            //   STR_GameID = "146"; // worksheet 2           audio text que, image opt
            //   STR_GameID = "145"; // worksheet 2

            //   STR_GameID = "147"; // train 2,3,4X5 text text
            //   STR_GameID = "148";  // worksheet match text text
            //   STR_GameID = "149"; //worksheet match Image Image
            //   STR_GameID = "150"; //worksheet match text Image

            //  STR_GameID = "151"; //worksheet match Image text drag and drop
            //  STR_GameID = "153"; //worksheet 3x3 Image drag and drop
            //  STR_GameID = "175"; // worksheet image drag and drop
            //  STR_GameID = "177"; // worksheet sentence text drag and drop

            // STR_GameID = "196";
            // STR_GameID = "156";   // Passage
            // STR_GameID = "202";   // Q_image, text and opt text (static)
            // STR_GameID = "203";   // Word Search
        }
    }


    private void Start()
    {
        I_loadTime = 30;
     //   Debug.Log("Load Time :  " + I_loadTime);
    }

    private void Update()
    {
        if (WEB)
        {
            if (STR_GameID != "" && STR_childID!="" && !called) // live
            {
                called = true;
                G_GameManager.SetActive(true);
            }
            if(STR_GameID==""&& STR_childID == "" && !called && mode=="preview") // preview
            {
                called = true;
                G_GameManager.SetActive(true);
               // G_coverPage.SetActive(false);
            }
        }
        if(MOBILE)
        {
            if (STR_GameID != "" && STR_childID != "" && !called)
            {
                called = true;
                G_GameManager.SetActive(true);
            }
        }

        if (IM_loading != null)
        {
            if (called && startLoad < 100f)
            {
                startLoad = startLoad + I_loadTime * Time.deltaTime;
                IM_loading.fillAmount = startLoad / maxLoad;
                if (startLoad >= maxLoad && G_coverPageStart != null)
                {
                    G_coverPageStart.GetComponent<Button>().interactable = true;
                    G_coverPageStart.GetComponent<Animator>().enabled = true;
                    IM_loading.transform.parent.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        if (G_coverPage != null)
        {
            if (!G_coverPage.activeInHierarchy && !B_enteredGame) // player has entered the game
            {
                B_enteredGame = true;
               /* if (HelicopterGameManager.instance != null)
                {
                    HelicopterGameManager.instance.AS_helicopter.Play();   // helicopter game
                }*/
            }
        }
    }
}
