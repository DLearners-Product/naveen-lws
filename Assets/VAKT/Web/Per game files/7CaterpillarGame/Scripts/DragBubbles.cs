using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragBubbles : MonoBehaviour
{
    //  bool B_correctMatch;
    bool B_matched;
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
            //if (!B_correctMatch)
            //{
            //    transform.position = startPos;
            //}
            //else
            //{
            //    transform.position = G_collisionObject.transform.position;
            //    OrderDragManager.instance.I_matches++;
            //    if(OrderDragManager.instance.I_matches==6)
            //    {
            //        OrderDragManager.instance.G_caterpillarHead.GetComponent<Animator>().Play("smile");
            //    }    
            //    OrderDragManager.instance.THI_delayCheckLevelComplete();
            //    OrderDragManager.instance.THI_increaseScore();
            //    G_collisionObject.transform.GetChild(0).gameObject.SetActive(false);
            //    transform.GetChild(0).GetComponent<Text>().color = Color.green;
            //    GetComponent<Collider2D>().enabled = false;
            //    this.enabled = false;            
            //}
            if(!B_matched)
            {
                transform.position = startPos;
            }
            else
            {
                transform.position = G_collisionObject.transform.position;
                OrderDragManager.instance.STRL_childAnswer.Add(transform.GetChild(0).GetComponent<Text>().text);
                OrderDragManager.instance.I_matches++;
               
                OrderDragManager.instance.THI_delayCheckLevelComplete();
              //  OrderDragManager.instance.THI_increaseScore();
                G_collisionObject.transform.GetChild(0).gameObject.SetActive(false);
             //   transform.GetChild(0).GetComponent<Text>().color = Color.green;
                GetComponent<Collider2D>().enabled = false;
                G_collisionObject.GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
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
        if(collision.tag=="SortingOptions")
        {
            B_matched = true;
            G_collisionObject = collision.gameObject;
        }
        //if (this.gameObject.name == collision.gameObject.name)
        //{
        //    B_correctMatch = true;
        //    G_collisionObject = collision.gameObject;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "SortingOptions")
        {
            B_matched = false;
            G_collisionObject = collision.gameObject;
        }
        //if (this.gameObject.name == collision.gameObject.name)
        //{
        //    B_correctMatch = false;
        //    G_collisionObject = null;
        //}
    }
}