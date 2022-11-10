using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeWordsManager : MonoBehaviour
{
    [Header("Instance")]
    public static MakeWordsManager instance;

    [Header("DB")]
    public string URL;
    public string sendURL;
    public List<string> STRL_questionSpriteURL;
    public List<Sprite> SPRL_questionSprites;
    public List<string> STRL_answers;
    public string STR_CurrentAnswer;
    public int I_totalQuestionCount;

    [Header("Details")]
    public int I_questionCount;
    public int I_matchCount;
    public char[] CHARA_letters;
    public SpriteRenderer SR_currentQuestion;
    public GameObject G_clonedLetter;
    public GameObject G_letterPrefab;
    public string STR_selectedLetter;
    public bool B_cloned;
    public bool B_canClick;
    public GameObject[] GA_answerHolders;
    Vector2 clickPos;

    [Header("Other")]
    public GameObject G_cancelLetter;
    public List<GameObject> GL_letters;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        I_questionCount = -1;
        I_totalQuestionCount = STRL_questionSpriteURL.Count;
        THI_questionShow();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !B_cloned && B_canClick)
        {
            THI_detectLetters();
        }
        if(B_cloned)
        {
            THI_letterFollowMouse();
        }
    }

    void THI_detectLetters()
    {
        clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(clickPos, Vector2.zero);
        if (hit2D.collider != null && hit2D.collider.transform.parent.name == "OptionPanel")
        {
            STR_selectedLetter = hit2D.collider.gameObject.name;
            THI_cloneLetters();  
        }
    }

    void THI_cloneLetters()
    {
        G_clonedLetter = Instantiate(G_letterPrefab);
        G_clonedLetter.GetComponent<TextMesh>().text = STR_selectedLetter;
        G_clonedLetter.transform.position = clickPos;
        G_cancelLetter.SetActive(true);
        B_cloned = true;
        B_canClick = false;
        STR_selectedLetter = "";
    }

    void THI_letterFollowMouse()
    {
        if(Input.GetMouseButton(0))
        G_clonedLetter.transform.position = Vector2.Lerp(G_clonedLetter.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1f);
    }

    public void THI_questionShow()
    {
        for (int i = 0; i < GA_answerHolders.Length; i++)
        {
            GA_answerHolders[i].GetComponent<Collider2D>().enabled=true;
        }
        for (int i = 0; i <GL_letters.Count; i++)
        {
            Destroy(GL_letters[i]);
        }
        GL_letters = new List<GameObject>();
        I_matchCount = 0;
           B_canClick = true;
        for (int i = 0; i <GA_answerHolders.Length; i++)
        {
            GA_answerHolders[i].SetActive(false);
            GA_answerHolders[i].name = i.ToString();
        }

        I_questionCount++;
        if (I_questionCount<I_totalQuestionCount)
        {
            SR_currentQuestion.sprite = SPRL_questionSprites[I_questionCount];
            STR_CurrentAnswer = STRL_answers[I_questionCount];
            CHARA_letters = STR_CurrentAnswer.ToCharArray();

            THI_sortAnswerHolders();
        }
        else
        {
            Debug.Log("Game Completed!");
        }
    }

    public void THI_questionShowDelay()
    {
        Invoke("THI_questionShow", 2f);
    }

    void THI_sortAnswerHolders()
    {
        if(CHARA_letters.Length ==  1)
        {
            GA_answerHolders[4].SetActive(true);

            GA_answerHolders[4].name = CHARA_letters[0].ToString();
        }
        if (CHARA_letters.Length == 2)
        {
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);

            GA_answerHolders[4].name = CHARA_letters[0].ToString();
            GA_answerHolders[5].name = CHARA_letters[1].ToString();
        }
        if (CHARA_letters.Length == 3)
        {
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);

            GA_answerHolders[3].name = CHARA_letters[0].ToString();
            GA_answerHolders[4].name = CHARA_letters[1].ToString();
            GA_answerHolders[5].name = CHARA_letters[2].ToString();
        }
        if (CHARA_letters.Length == 4)
        {
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);

            GA_answerHolders[3].name = CHARA_letters[0].ToString();
            GA_answerHolders[4].name = CHARA_letters[1].ToString();
            GA_answerHolders[5].name = CHARA_letters[2].ToString();
            GA_answerHolders[6].name = CHARA_letters[3].ToString();
        }
        if (CHARA_letters.Length == 5)
        {
            GA_answerHolders[2].SetActive(true);
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);


            GA_answerHolders[2].name = CHARA_letters[0].ToString();
            GA_answerHolders[3].name = CHARA_letters[1].ToString();
            GA_answerHolders[4].name = CHARA_letters[2].ToString();
            GA_answerHolders[5].name = CHARA_letters[3].ToString();
            GA_answerHolders[6].name = CHARA_letters[4].ToString();
        }
        if (CHARA_letters.Length == 6)
        {
            GA_answerHolders[2].SetActive(true);
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);
            GA_answerHolders[7].SetActive(true);


            GA_answerHolders[2].name = CHARA_letters[0].ToString();
            GA_answerHolders[3].name = CHARA_letters[1].ToString();
            GA_answerHolders[4].name = CHARA_letters[2].ToString();
            GA_answerHolders[5].name = CHARA_letters[3].ToString();
            GA_answerHolders[6].name = CHARA_letters[4].ToString();
            GA_answerHolders[7].name = CHARA_letters[5].ToString();
        }
        if (CHARA_letters.Length == 7)
        {
            GA_answerHolders[1].SetActive(true);
            GA_answerHolders[2].SetActive(true);
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);
            GA_answerHolders[7].SetActive(true);

            GA_answerHolders[1].name = CHARA_letters[0].ToString();
            GA_answerHolders[2].name = CHARA_letters[1].ToString();
            GA_answerHolders[3].name = CHARA_letters[2].ToString();
            GA_answerHolders[4].name = CHARA_letters[3].ToString();
            GA_answerHolders[5].name = CHARA_letters[4].ToString();
            GA_answerHolders[6].name = CHARA_letters[5].ToString();
            GA_answerHolders[7].name = CHARA_letters[6].ToString();
        }
        if (CHARA_letters.Length == 8)
        {
            GA_answerHolders[1].SetActive(true);
            GA_answerHolders[2].SetActive(true);
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);
            GA_answerHolders[7].SetActive(true);
            GA_answerHolders[8].SetActive(true);

            GA_answerHolders[1].name = CHARA_letters[0].ToString();
            GA_answerHolders[2].name = CHARA_letters[1].ToString();
            GA_answerHolders[3].name = CHARA_letters[2].ToString();
            GA_answerHolders[4].name = CHARA_letters[3].ToString();
            GA_answerHolders[5].name = CHARA_letters[4].ToString();
            GA_answerHolders[6].name = CHARA_letters[5].ToString();
            GA_answerHolders[7].name = CHARA_letters[6].ToString();
            GA_answerHolders[8].name = CHARA_letters[7].ToString();
        }
        if (CHARA_letters.Length == 9)
        {
            GA_answerHolders[0].SetActive(true);
            GA_answerHolders[1].SetActive(true);
            GA_answerHolders[2].SetActive(true);
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);
            GA_answerHolders[7].SetActive(true);
            GA_answerHolders[8].SetActive(true);

            GA_answerHolders[0].name = CHARA_letters[0].ToString();
            GA_answerHolders[1].name = CHARA_letters[1].ToString();
            GA_answerHolders[2].name = CHARA_letters[2].ToString();
            GA_answerHolders[3].name = CHARA_letters[3].ToString();
            GA_answerHolders[4].name = CHARA_letters[4].ToString();
            GA_answerHolders[5].name = CHARA_letters[5].ToString();
            GA_answerHolders[6].name = CHARA_letters[6].ToString();
            GA_answerHolders[7].name = CHARA_letters[7].ToString();
            GA_answerHolders[8].name = CHARA_letters[8].ToString();
        }
        if (CHARA_letters.Length == 10)
        {
            GA_answerHolders[0].SetActive(true);
            GA_answerHolders[1].SetActive(true);
            GA_answerHolders[2].SetActive(true);
            GA_answerHolders[3].SetActive(true);
            GA_answerHolders[4].SetActive(true);
            GA_answerHolders[5].SetActive(true);
            GA_answerHolders[6].SetActive(true);
            GA_answerHolders[7].SetActive(true);
            GA_answerHolders[8].SetActive(true);
            GA_answerHolders[9].SetActive(true);

            GA_answerHolders[0].name = CHARA_letters[0].ToString();
            GA_answerHolders[1].name = CHARA_letters[1].ToString();
            GA_answerHolders[2].name = CHARA_letters[2].ToString();
            GA_answerHolders[3].name = CHARA_letters[3].ToString();
            GA_answerHolders[4].name = CHARA_letters[4].ToString();
            GA_answerHolders[5].name = CHARA_letters[5].ToString();
            GA_answerHolders[6].name = CHARA_letters[6].ToString();
            GA_answerHolders[7].name = CHARA_letters[7].ToString();
            GA_answerHolders[8].name = CHARA_letters[8].ToString();
            GA_answerHolders[9].name = CHARA_letters[9].ToString();
        }
    }
}
