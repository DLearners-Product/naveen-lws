using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterTrigger : MonoBehaviour
{
    public bool B_letterRemove;
    public AnimationClip AC_cancelInv;

    void Start()
    {
        if(gameObject.name== "CancelLetter")
        {
            B_letterRemove = true;
        }
        else
        {
            B_letterRemove = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!B_letterRemove)
        {
            //if (Input.GetMouseButtonUp(0))
            // {
            if (collision.gameObject.GetComponent<TextMesh>().text == gameObject.name)
            {
                MakeWordsManager.instance.B_cloned = false;
                MakeWordsManager.instance.B_canClick = true;
                collision.gameObject.transform.position = transform.position;
                GetComponent<Collider2D>().enabled = false;
                Destroy(collision.gameObject.GetComponent<Collider2D>());
                MakeWordsManager.instance.G_cancelLetter.SetActive(false);
                MakeWordsManager.instance.GL_letters.Add(collision.gameObject);
                MakeWordsManager.instance.I_matchCount++;
                if (MakeWordsManager.instance.I_matchCount == MakeWordsManager.instance.CHARA_letters.Length)
                {
                    MakeWordsManager.instance.THI_questionShowDelay();
                }
            }
            else
            {
                Debug.Log("Wrong match!");
            }
            //}
        }
        else
        {
            Destroy(MakeWordsManager.instance.G_clonedLetter);
            MakeWordsManager.instance.B_cloned = false;
            GetComponent<Animator>().Play("cancelLetter inv");
            Invoke("THI_off", AC_cancelInv.length);
        }
    }

    void THI_off()
    {
        gameObject.SetActive(false);
        MakeWordsManager.instance.B_canClick = true;
    }
}
