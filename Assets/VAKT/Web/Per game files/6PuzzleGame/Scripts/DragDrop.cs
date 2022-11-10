using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
     bool B_correctMatch;
    Vector2 startPos;
    bool B_isDragging;
    GameObject G_collisionObject;

    void Start()
    {
        startPos = transform.position;
    }


    void Update()
    {
        if (B_isDragging)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousepos;
        }
        else
        {
            if (!B_correctMatch)
            {
                transform.position = startPos;
            }
            else
            {
                transform.position = G_collisionObject.transform.position;
                PuzzleGM.instance.I_matchCount++;
                PuzzleGM.instance.THI_checkLevelComplete();
                Destroy(this);
                Destroy(GetComponent<Collider2D>());
            }
        }
    }
    private void OnMouseDown()
    {
        B_isDragging = true;
    }

    private void OnMouseUp()
    {
        B_isDragging = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.gameObject.name == collision.gameObject.name)
        {
            B_correctMatch = true;
            G_collisionObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.name == collision.gameObject.name)
        {
            B_correctMatch = false;
            G_collisionObject = null;
        }
    }
}