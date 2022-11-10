using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGM : MonoBehaviour
{
    [Header("Instance")]
    public static PuzzleGM instance;

    [Header("DB")]
    public string STR_questionImageURL;
    public Sprite SPR_questionImage;
    public List<string> STRL_answerImagesURL;
    public List<Sprite> SPRL_answerImages;

    [Header("Objects")]
    public SpriteRenderer SR_question;
    public SpriteRenderer[] SRA_questionSplit;
    public SpriteRenderer[] SRA_answers;
   

    [Header("Game Complete")]
    public int I_matchCount;
    public GameObject G_levelComplete;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        I_matchCount = 0;
        THI_assignSprites();
    }

    private void THI_assignSprites()
    {
        for(int i = 0; i <SRA_answers.Length; i++)
        {
            SRA_answers[i].sprite = SPRL_answerImages[i];
            SRA_questionSplit[i].sprite = SPRL_answerImages[i];
        }
        SR_question.sprite = SPR_questionImage;
    }

    public void THI_checkLevelComplete()
    {
        if(I_matchCount==9)
        {
            Invoke("THI_levelComplete", 2f);
        }
    }

    void THI_levelComplete()
    {
        G_levelComplete.SetActive(true);
    }
}
