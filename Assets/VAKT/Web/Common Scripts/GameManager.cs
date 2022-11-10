using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("DATABSE URL")]
    public string URL;

    //[Header("DATABASE VALUES")]
    //public string[] STRA_gameID;
    //public string[] STRA_gameName;
    //public string[] STRA_templateName;

    //[Header("CLONE GAME BUTTONS")]
    //public GameObject G_gameButtonPrefab;
    //public Transform T_newgameContent;

    //[Header("GAME SELECTION")]
    //public GameObject[] GA_templatePrefabs;
    //public string STR_selectedGame;
    //public string STR_selectedGameID;
    //public GameObject G_currentGame;

    [Header("SCREEN TRANSITIONS")]
    public GameObject[] GA_pages;
    public InputField IF_Signupmobilenumber;
    public InputField IF_SignupOTP;
    public Text[] TEXA_OTPdisplay;
    public Text TEX_verifyOTPinfo;
    public InputField[] IFA_vd_detailsPage;
    public GameObject[] GA_vd_editbuttons;
    public GameObject G_vd_save;
    public GameObject G_vd_discard;
    public GameObject G_vd_verify;
    public GameObject G_joinUs;
    public InputField IF_sp_enterPass;
    public InputField IF_sp_re_enterPass;
    public bool B_startOTPtimer;
    public float F_otpTimer;
    public Text TEX_resendOTPtext;
    public bool B_signinWithOTP;
    public InputField IF_signInMobileNumber;


    [Header("DASHBOARD")]
    public GameObject[] GA_menuScreens;
    public GameObject[] GA_menuButtons;
    public Sprite[] SPRA_defaultMenus;
    public Sprite[] SPRA_selectedMenus;
    public Text TEX_childName;
    public Text TEX_grade;
    public Text[] TEXA_csDisplay;


    [Header("Class Schedule")]
    public GameObject G_classScheduleBoxPrefab;
    public List<GameObject> GL_classesBoxIG;
    public Transform T_cs_parent;
    public int I_classScheduleCount;

    [Header("Replay Videos")]
    public GameObject G_replayVideoBoxPrefab;
    public Transform T_rv_parent;
    public int I_replayVideosCount;

    [Header("Assignments")]
    public Text TEX_assignmentTitleText;
    public GameObject G_gamesToggle;
    public GameObject G_worksheetToggle;
    public bool B_games;
    public GameObject G_assignmentBoxPrefab;
    public int I_asGameCount;
    public Transform T_as_parent_All;
    public Transform T_as_parent_To_do;
    public Transform T_as_parent_Done;
    public Transform T_as_parent_Favs;
    public Sprite SPR_asDone;
    public Sprite SPR_asTodo;
    public Sprite SPR_Yesfav;
    public Sprite SPR_Nofav;
    public GameObject G_allGames;
    public GameObject G_todoGames;
    public GameObject G_doneGames;
    public GameObject G_favGames;
    public int I_asWorksheetCount;
    public Transform T_as_parent_All_worksheet;
    public Transform T_as_parent_To_do_worksheet;
    public Transform T_as_parent_Done_worksheet;
    public Transform T_as_parent_Favs_worksheet;
    public GameObject G_allWS;
    public GameObject G_todoWS;
    public GameObject G_doneWS;
    public GameObject G_favWS;
    public Text TEX_all;
    public Text TEX_todo;
    public Text TEX_done;
    public Image IM_fav;
    public Sprite SPR_selectedFav;
    public Sprite SPR_unselectedFav;
    public Color COL_selected;
    public Color COL_unselected;


    [Header("DB")]
    public string[] STRA_userDetails; // profile details
    public string STR_childName;
    public string STR_childGrade;
    public string[] STRA_csDisplay; // display today's class schedule details in home page
    public string[] STRA_csDay; // full schedule day data
    public string[] STRA_csTime;// full schedule time data
    public string[] STRA_csClassNumber;// full schedule class number data
    public string[] STRA_csDate;// full schedule date data
    public string[] STRA_csTeacher;// full schedule teacher data
    public string[] STRA_rvDay;// replay videos day data
    public string[] STRA_rvClassNumber;// replay videos class number data
    public string[] STRA_rvVideoURL; // replay videos url links data
    public string[] STRA_rvAbout; //replay videos about data
    public Sprite[] SPRA_asGameIcon;
    public string[] STRA_asGameName;
    public string[] STRA_asGameInfo;
    public string[] STRA_asGameStatusText;
    public string[] STRA_asGameTime;
    public string[] STRA_favourite;
    public Sprite[] SPRA_asWSIcon;
    public string[] STRA_asWSName;
    public string[] STRA_asWSInfo;
    public string[] STRA_asWSStatusText;
    public string[] STRA_asWSTime;
    public string[] STRA_asWSfavourite;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        F_otpTimer = 30f;
        TEX_resendOTPtext.GetComponent<Button>().enabled = false;
        for (int i = 0; i < IFA_vd_detailsPage.Length; i++)
        {
            IFA_vd_detailsPage[i].enabled = false;
        }
    }
    private void Update()
    {
        if(B_startOTPtimer)
        {
            F_otpTimer = F_otpTimer - 1 * Time.deltaTime;
            TEX_resendOTPtext.text = "Resend OTP in " +(int)F_otpTimer+"s";
            if(F_otpTimer<=0)
            {
                B_startOTPtimer = false;
                TEX_resendOTPtext.text = "Click to resend OTP";
                TEX_resendOTPtext.GetComponent<Button>().enabled = true;
            }
        }
    }

    void THI_pageTransition(GameObject enable, GameObject disable)
    {
        enable.SetActive(true);
        disable.SetActive(false);
    }
    public void BUT_SignInPage()
    {
        THI_pageTransition(GA_pages[1], GA_pages[0]);
    }
    public void BUT_SignUpPage()
    {
        THI_pageTransition(GA_pages[2], GA_pages[0]);
    }
    public void BUT_BackButtonSignIn()
    {
        // THI_pageTransition(GA_pages[0], GA_pages[1]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BUT_BackButtonSignUp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void THI_VerifyDetailsPage()
    {
        THI_pageTransition(GA_pages[3], GA_pages[2]);
        for(int i = 0; i <IFA_vd_detailsPage.Length; i++)
        {
            IFA_vd_detailsPage[i].text = STRA_userDetails[i];
        }
    }
    public void BUT_requestOTP()
    {
     
        if (!B_signinWithOTP)
        {
            int enteredMobileNumber;
            int.TryParse(IF_Signupmobilenumber.text, out enteredMobileNumber);

            // 1 ) mobile number present in db - screening done - payment done
            GA_pages[2].GetComponent<Animator>().Play("signuppage2"); // otp trigger from db and otp page display
            B_startOTPtimer = true;
            TEX_verifyOTPinfo.text = "Please enter the verification code sent to +91 " + IF_Signupmobilenumber.text;

            // 2 ) mobile number present in db - screening done - payment not done

            // 3) mobile number present in db- screening not done
            // G_joinUs.SetActive(true);

            // 4) mobile number not present in db

        }
        else
        {
            int enteredMobileNumber;
            int.TryParse(IF_signInMobileNumber.text, out enteredMobileNumber);

            // 1 ) mobile number present in db - screening done - payment done
            GA_pages[2].GetComponent<Animator>().Play("signuppage2"); // otp trigger from db and otp page display
            B_startOTPtimer = true;
            TEX_verifyOTPinfo.text = "Please enter the verification code sent to +91 " + IF_signInMobileNumber.text;

            // 2 ) mobile number present in db - screening done - payment not done

            // 3) mobile number present in db- screening not done
            // G_joinUs.SetActive(true);

            // 4) mobile number not present in db
        }
    }

    public void BUT_signInWithOTP()
    {
        if (IF_signInMobileNumber.text.Length == 10)
        {
            F_otpTimer = 30f;
            IF_Signupmobilenumber.text = "";
            B_signinWithOTP = true;
            IF_SignupOTP.text = "";
            for(int i = 0; i <TEXA_OTPdisplay.Length;i++)
            {
                TEXA_OTPdisplay[i].text = "";
            }
            THI_pageTransition(GA_pages[2], GA_pages[1]);
            GA_pages[2].GetComponent<Animator>().speed = 100f;
            BUT_requestOTP();
        }
    }

    public void BUT_resendOTP()
    {
        F_otpTimer = 30f;
        B_startOTPtimer = true;
        TEX_resendOTPtext.GetComponent<Button>().enabled = false;
    }

    public void BUT_verifyOTP()
    {
        B_startOTPtimer = false;
        F_otpTimer = 30f;
        GA_pages[2].GetComponent<Animator>().speed = 1f;
        if (!B_signinWithOTP)
        {
            THI_VerifyDetailsPage();
        }
        else
        {
            //sign in to dashboard using OTP

            THI_dashboardSignIn();




        }

    }

    public void THI_dashboardSignIn()
    {
        B_games = true;
        THI_pageTransition(GA_pages[5], GA_pages[3]);
        THI_getClassScheduleDetails();
        THI_getReplayVideosDetails();
        THI_getAssignmentDetails();
        G_allGames.SetActive(true);
        G_todoGames.SetActive(false);
        G_doneGames.SetActive(false);
        G_favGames.SetActive(false);

        G_allWS.SetActive(false);
        G_todoWS.SetActive(false);
        G_favWS.SetActive(false);
        G_doneWS.SetActive(false);
    }

    public void IF_otpEdit()
    {
        char[] otpsplt = IF_SignupOTP.text.ToCharArray();
        for (int i = 0; i < otpsplt.Length; i++)
        {
            TEXA_OTPdisplay[i].text = otpsplt[i].ToString();
        }
    }

    public void BUT_editDetails()
    {
        for (int i = 0; i < IFA_vd_detailsPage.Length; i++)
        {
            IFA_vd_detailsPage[i].enabled = false;
            IFA_vd_detailsPage[i].interactable = false;
            if (EventSystem.current.currentSelectedGameObject.transform.parent.GetChild(1).GetComponent<InputField>()== IFA_vd_detailsPage[i])
            {
                IFA_vd_detailsPage[i].enabled = true;
                IFA_vd_detailsPage[i].interactable = true;
            }
       }
        for(int i = 0; i < GA_vd_editbuttons.Length;i++)
        {
            GA_vd_editbuttons[i].SetActive(false);
        }
        G_vd_save.SetActive(true);
        G_vd_discard.SetActive(true);
        G_vd_verify.SetActive(false);
    }

    public void BUT_vd_discard()
    {
        for (int i = 0; i < GA_vd_editbuttons.Length; i++)
        {
            GA_vd_editbuttons[i].SetActive(true);
        }
        for (int i = 0; i < IFA_vd_detailsPage.Length; i++)
        {
            IFA_vd_detailsPage[i].interactable = true;
            IFA_vd_detailsPage[i].enabled = false;
            IFA_vd_detailsPage[i].text = STRA_userDetails[i];
        }
        G_vd_save.SetActive(false);
        G_vd_discard.SetActive(false);
        G_vd_verify.SetActive(true);
    }
    public void BUT_vd_save()
    {
        for (int i = 0; i < GA_vd_editbuttons.Length; i++)
        {
            GA_vd_editbuttons[i].SetActive(true);
        }
        for (int i = 0; i < IFA_vd_detailsPage.Length; i++)
        {
            IFA_vd_detailsPage[i].interactable = true;
            IFA_vd_detailsPage[i].enabled = false;
            STRA_userDetails[i] = IFA_vd_detailsPage[i].text;
        }
        G_vd_save.SetActive(false);
        G_vd_discard.SetActive(false);
        G_vd_verify.SetActive(true);
    }

    public void BUT_vd_verifyDetails()
    {
        THI_pageTransition(GA_pages[4], GA_pages[3]);
    }

    public void BUT_sp_SetPassword()
    {
        if (IF_sp_enterPass.text == IF_sp_re_enterPass.text)
        {
            Debug.Log("Passwords matching. redirection to dashboard");
            THI_pageTransition(GA_pages[1], GA_pages[4]);
        }
        else
        {
            Debug.Log("Passwords not matching");
        }
    }

    public void BUT_signInValidationSubmit() // sign in with password
    {
        THI_dashboardSignIn();


    }

    public void BUT_passwordVisibility(InputField passwordField)
    {
        passwordField.contentType = InputField.ContentType.Standard;
    }

    public void BUT_screeningRedirection()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.screening.dlparent");
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.OpenURL("https://apps.apple.com/in/app/dlearners-parent/id1532043292");
        }
    }

    public void BUT_DashboardMenu()
    {
        for (int i = 0; i < GA_menuButtons.Length; i++)
        {
            GA_menuScreens[4].SetActive(false);
            GA_menuScreens[i].SetActive(false);
            GA_menuButtons[i].GetComponent<Image>().sprite = SPRA_defaultMenus[i];
            if (EventSystem.current.currentSelectedGameObject.name==GA_menuScreens[i].name)
            {
                GA_menuScreens[i].SetActive(true);
                GA_menuButtons[i].GetComponent<Image>().sprite = SPRA_selectedMenus[i];
            }
        }
    }

    public void BUT_fullScheduleBack()
    {
        GA_menuScreens[4].SetActive(false);
        GA_menuScreens[0].SetActive(true);
        GA_menuButtons[0].GetComponent<Image>().sprite = SPRA_selectedMenus[0];
    }
    public void BUT_videoReplaysBack()
    {
        GA_menuScreens[2].SetActive(false);
        GA_menuScreens[0].SetActive(true);
        for(int i = 0; i <GA_menuButtons.Length;i++)
        {
            GA_menuButtons[i].GetComponent<Image>().sprite = SPRA_defaultMenus[i];
        }
        GA_menuButtons[0].GetComponent<Image>().sprite = SPRA_selectedMenus[0];
    }
    public void BUT_assignmentsBack()
    {
        GA_menuScreens[1].SetActive(false);
        GA_menuScreens[0].SetActive(true);
        for (int i = 0; i < GA_menuButtons.Length; i++)
        {
            GA_menuButtons[i].GetComponent<Image>().sprite = SPRA_defaultMenus[i];
        }
        GA_menuButtons[0].GetComponent<Image>().sprite = SPRA_selectedMenus[0];

        TEX_all.color = COL_selected;
        TEX_todo.color = COL_unselected;
        TEX_done.color = COL_unselected;
        IM_fav.sprite = SPR_unselectedFav;
        B_games = true;
        G_worksheetToggle.SetActive(false);
        G_gamesToggle.SetActive(true);
        TEX_assignmentTitleText.text = "Games";
        B_games = true;
        G_allGames.SetActive(true);
        G_todoGames.SetActive(false);
        G_doneGames.SetActive(false);
        G_favGames.SetActive(false);
        G_todoWS.SetActive(false);
        G_favWS.SetActive(false);
        G_doneWS.SetActive(false);
        G_allWS.SetActive(false);
    }

    public void BUT_fullSchedule()
    {
        for (int i = 0; i < GA_menuScreens.Length; i++)
        {
            GA_menuScreens[i].SetActive(false);
        }
        GA_menuScreens[4].SetActive(true);
      
    }

    public void BUT_changeAssignment()
    {
        TEX_all.color = COL_selected;
        TEX_todo.color = COL_unselected;
        TEX_done.color = COL_unselected;
        IM_fav.sprite = SPR_unselectedFav;
        if (B_games)
        {
            G_worksheetToggle.SetActive(true);
            G_gamesToggle.SetActive(false);
            TEX_assignmentTitleText.text = "Worksheets";
            B_games = false;
            G_allGames.SetActive(false);
            G_todoGames.SetActive(false);
            G_doneGames.SetActive(false);
            G_favGames.SetActive(false);
            G_todoWS.SetActive(false);
            G_favWS.SetActive(false);
            G_doneWS.SetActive(false);
            G_allWS.SetActive(true);

        }
        else
        {
            G_worksheetToggle.SetActive(false);
            G_gamesToggle.SetActive(true);          
            TEX_assignmentTitleText.text = "Games";
            B_games = true;
            G_allGames.SetActive(true);
            G_todoGames.SetActive(false);
            G_doneGames.SetActive(false);
            G_favGames.SetActive(false);
            G_todoWS.SetActive(false);
            G_favWS.SetActive(false);
            G_doneWS.SetActive(false);
            G_allWS.SetActive(false);
        }
    }

    void THI_getClassScheduleDetails()
    {
        I_classScheduleCount = STRA_csDay.Length;
        for (int i = 0; i < I_classScheduleCount; i++)
        {
            var cs_box = Instantiate(G_classScheduleBoxPrefab);
            cs_box.transform.SetParent(T_cs_parent, false);
            cs_box.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = STRA_csDay[i]; // day
            cs_box.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = STRA_csTime[i]; // time
            cs_box.transform.GetChild(1).GetComponent<Text>().text = STRA_csClassNumber[i] +"/"+I_classScheduleCount; // class no
            cs_box.transform.GetChild(2).GetComponent<Text>().text = STRA_csDate[i]; // date
            cs_box.transform.GetChild(3).GetComponent<Text>().text = STRA_csTeacher[i]; // teacher
            GL_classesBoxIG.Add(cs_box);
        }
        STRA_csDisplay = new string[5];
        STRA_csDisplay[0] = STRA_csDay[0];
        STRA_csDisplay[1] = STRA_csTime[0];
        STRA_csDisplay[2] = STRA_csClassNumber[0];
        STRA_csDisplay[3] = STRA_csDate[0];
        STRA_csDisplay[4] = STRA_csTeacher[0];

        STR_childName = STRA_userDetails[0];
        STR_childGrade = STRA_userDetails[3];

        TEX_childName.text = STR_childName;
        TEX_grade.text = STR_childGrade;

        for (int i = 0; i < STRA_csDisplay.Length; i++)
        {
            TEXA_csDisplay[i].text = STRA_csDisplay[i];
        }
        for(int i = 0; i <GL_classesBoxIG.Count;i++)
        {
            GL_classesBoxIG[i].GetComponent<Outline>().enabled = false;
            GL_classesBoxIG[i].transform.GetChild(4).gameObject.SetActive(false);
        }
        GL_classesBoxIG[0].GetComponent<Outline>().enabled = true;
        GL_classesBoxIG[0].transform.GetChild(4).gameObject.SetActive(true);
    }
    void THI_getReplayVideosDetails()
    {
        I_replayVideosCount = STRA_rvDay.Length;
        for (int i = 0; i < I_replayVideosCount; i++)
        {
            var rv_box = Instantiate(G_replayVideoBoxPrefab);
            rv_box.transform.SetParent(T_rv_parent, false);
            rv_box.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = STRA_rvDay[i]; // day
            rv_box.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = STRA_rvClassNumber[i]; // class
            rv_box.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = STRA_rvAbout[i]; // about
                                                                                                            // Debug.Log(STRA_rvVideoURL[i]);
            rv_box.transform.GetChild(1).name = STRA_rvVideoURL[i];
            rv_box.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(THI_openVideo);
          //  rv_box.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {Application.OpenURL(STRA_rvVideoURL[i]);}); // video
        }
    }

    public void BUT_allGames()
    {
        TEX_all.color = COL_selected;
        TEX_todo.color = COL_unselected;
        TEX_done.color = COL_unselected;
        IM_fav.sprite = SPR_unselectedFav;
        if (B_games)
        {
            G_allGames.SetActive(true);
            G_todoGames.SetActive(false);
            G_doneGames.SetActive(false);
            G_favGames.SetActive(false);
        }
        else
        {
            G_allWS.SetActive(true);
            G_todoWS.SetActive(false);
            G_doneWS.SetActive(false);
            G_favWS.SetActive(false);
        }
    }
    public void BUT_toDogames()
    {
        TEX_all.color = COL_unselected;
        TEX_todo.color = COL_selected;
        TEX_done.color = COL_unselected;
        IM_fav.sprite = SPR_unselectedFav;

        if (B_games)
        {
            G_allGames.SetActive(false);
            G_todoGames.SetActive(true);
            G_doneGames.SetActive(false);
            G_favGames.SetActive(false);
        }
        else
        {
            G_allWS.SetActive(false);
            G_todoWS.SetActive(true);
            G_doneWS.SetActive(false);
            G_favWS.SetActive(false);
        }
    }
    public void BUT_donegames()
    {
        TEX_all.color = COL_unselected;
        TEX_todo.color = COL_unselected;
        TEX_done.color = COL_selected;
        IM_fav.sprite = SPR_unselectedFav;

        if (B_games)
        {
            G_allGames.SetActive(false);
            G_todoGames.SetActive(false);
            G_doneGames.SetActive(true);
            G_favGames.SetActive(false);
        }
        else
        {
            G_allWS.SetActive(false);
            G_todoWS.SetActive(false);
            G_doneWS.SetActive(true);
            G_favWS.SetActive(false);
        }
    }
    public void BUT_favgames()
    {
        TEX_all.color = COL_unselected;
        TEX_todo.color = COL_unselected;
        TEX_done.color = COL_unselected;
        IM_fav.sprite = SPR_selectedFav;

        if (B_games)
        {
            G_allGames.SetActive(false);
            G_todoGames.SetActive(false);
            G_doneGames.SetActive(false);
            G_favGames.SetActive(true);
        }
        else
        {
            G_allWS.SetActive(false);
            G_todoWS.SetActive(false);
            G_doneWS.SetActive(false);
            G_favWS.SetActive(true);
        }
    }

    void THI_getAssignmentDetails()
    {
        I_asGameCount = STRA_asGameName.Length;
        for (int i = 0; i < I_asGameCount; i++)    //clone games in all columns and have as such
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_All, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asGameIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asGameName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asGameInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asGameStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asGameTime[i];

            //completion status
            if(STRA_asGameStatusText[i]=="Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asGameStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if(STRA_favourite[i]=="Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_favourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }
        }
        for (int i = 0; i < I_asGameCount; i++)   //clone games in to-do column and destory completed games
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_To_do, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asGameIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asGameName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asGameInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asGameStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asGameTime[i];

            //completion status
            if (STRA_asGameStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asGameStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_favourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_favourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }

            if(STRA_asGameStatusText[i]=="Done")
            {
                Destroy(as_box);
            }
        }



        for (int i = 0; i < I_asGameCount; i++)  //clone games in done column and destory not done games
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_Done, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asGameIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asGameName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asGameInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asGameStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asGameTime[i];

            //completion status
            if (STRA_asGameStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asGameStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_favourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_favourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }

            if (STRA_asGameStatusText[i] == "To-do")
            {
                Destroy(as_box);
            }
        }
        for (int i = 0; i < I_asGameCount; i++)   //clone games in favs column and destory not favourite games
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_Favs, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asGameIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asGameName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asGameInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asGameStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asGameTime[i];

            //completion status
            if (STRA_asGameStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asGameStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_favourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_favourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }

            if (STRA_favourite[i] == "No")
            {
                Destroy(as_box);
            }
        }



        I_asWorksheetCount = STRA_asWSName.Length;
        for (int i = 0; i < I_asWorksheetCount; i++)    //clone games in all columns and have as such
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_All_worksheet, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asWSIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asWSName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asWSInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asWSStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asWSTime[i];

            //completion status
            if (STRA_asWSStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asWSStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_asWSfavourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_asWSfavourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }
        }
        for (int i = 0; i < I_asWorksheetCount; i++)    //clone games in all columns and have as such
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_To_do_worksheet, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asWSIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asWSName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asWSInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asWSStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asWSTime[i];

            //completion status
            if (STRA_asWSStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asWSStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_asWSfavourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_asWSfavourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }

            if (STRA_asWSStatusText[i] == "Done")
            {
                Destroy(as_box);
            }
        }
        for (int i = 0; i < I_asWorksheetCount; i++)    //clone games in all columns and have as such
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_Done_worksheet, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asWSIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asWSName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asWSInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asWSStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asWSTime[i];

            //completion status
            if (STRA_asWSStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asWSStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_asWSfavourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_asWSfavourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }

            if (STRA_asWSStatusText[i] == "To-do")
            {
                Destroy(as_box);
            }
        }
        for (int i = 0; i < I_asWorksheetCount; i++)    //clone games in all columns and have as such
        {
            //clone and assign other vals
            var as_box = Instantiate(G_assignmentBoxPrefab);
            as_box.transform.SetParent(T_as_parent_Favs_worksheet, false);
            as_box.transform.GetChild(0).GetComponent<Image>().sprite = SPRA_asWSIcon[i];
            as_box.transform.GetChild(1).GetComponent<Text>().text = STRA_asWSName[i];
            as_box.transform.GetChild(2).GetComponent<Text>().text = STRA_asWSInfo[i];
            as_box.transform.GetChild(5).GetComponent<Text>().text = STRA_asWSStatusText[i];
            as_box.transform.GetChild(6).GetComponent<Text>().text = STRA_asWSTime[i];

            //completion status
            if (STRA_asWSStatusText[i] == "Done")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asDone;
            }
            if (STRA_asWSStatusText[i] == "To-do")
            {
                as_box.transform.GetChild(4).GetComponent<Image>().sprite = SPR_asTodo;
            }

            //favourite status
            if (STRA_asWSfavourite[i] == "Yes")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Yesfav;
            }
            if (STRA_asWSfavourite[i] == "No")
            {
                as_box.transform.GetChild(3).GetComponent<Image>().sprite = SPR_Nofav;
            }

            if (STRA_asWSfavourite[i] == "No")
            {
                Destroy(as_box);
            }
        }
    }

    void THI_openVideo()
    {
        Application.OpenURL(EventSystem.current.currentSelectedGameObject.name);
    }

    //IEnumerator IN_getGamesfromDB()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(URL+ "list_game_template.php");
    //    yield return www.SendWebRequest();
    //    if(www.isHttpError || www.isNetworkError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        MyJSON json = new MyJSON();
    //        Debug.Log(www.downloadHandler.text);
    //        json.FetchGames(www.downloadHandler.text);
    //        THI_cloneGameButtonPrefabs();
    //    }
    //}

    //void THI_cloneGameButtonPrefabs()
    //{
    //    for (int i = 0; i < STRA_gameName.Length; i++)
    //    {
    //        var gameButton = Instantiate(G_gameButtonPrefab);
    //        gameButton.transform.SetParent(T_newgameContent, false);
    //        gameButton.transform.GetChild(0).GetComponent<Text>().text = STRA_gameName[i];
    //        gameButton.name = STRA_templateName[i];
    //        gameButton.transform.GetChild(1).name = STRA_gameID[i];
    //        gameButton.GetComponent<Button>().onClick.AddListener(THI_gameButtonClick);
    //    }
    //}

    //void THI_gameButtonClick()
    //{
    //    STR_selectedGame = EventSystem.current.currentSelectedGameObject.name;
    //    STR_selectedGameID = EventSystem.current.currentSelectedGameObject.transform.GetChild(1).name;
    //    for (int i = 0; i <GA_templatePrefabs.Length; i++)
    //    {
    //        if(GA_templatePrefabs[i].name==STR_selectedGame)
    //        {
    //            var game = Instantiate(GA_templatePrefabs[i]);
    //            game.transform.SetParent(GameObject.Find("ImmersiveApp").transform, false);
    //            game.transform.SetAsFirstSibling();
    //            GA_pages[2].SetActive(false);
    //            G_currentGame = GA_templatePrefabs[i];
    //        }
    //    }
    //}
}
