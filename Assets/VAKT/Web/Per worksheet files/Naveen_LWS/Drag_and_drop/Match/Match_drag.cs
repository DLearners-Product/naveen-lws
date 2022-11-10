using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Match_drag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 mousePos;
    public Vector2 initalPos;

    
    GameObject otherGameObject;

    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        initalPos = this.transform.position;
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (otherGameObject != null)
        {
            Match_Drag_Main.OBJ_Match_Drag_Main.STR_dummy= this.name;
           
            if (otherGameObject.transform.childCount == 0)
            {
                // Debug.Log("Empty Occupy");
                if (this.transform.parent.name == "Options")
                {
                    Match_Drag_Main.OBJ_Match_Drag_Main.THI_Count(true);
                }
                this.transform.SetParent(otherGameObject.transform, false);
                this.transform.position = otherGameObject.transform.position;
                this.transform.localScale = new Vector2(1f, 1f); 
               
            }
            else
            {
                if (this.name != otherGameObject.transform.GetChild(0).name)
                {
                   // Debug.Log("already available other");
                    
                    if (this.transform.parent.name != "Options")
                    {
                        Match_Drag_Main.OBJ_Match_Drag_Main.THI_Count(false);
                    }
                    Match_Drag_Main.OBJ_Match_Drag_Main.THI_BacktoOriginalpos();
                }
                else
                {
                    this.transform.SetParent(otherGameObject.transform, false);
                    this.transform.position = otherGameObject.transform.position;
                    this.transform.localScale = new Vector2(1f, 1f);
                }
                   
            }
           
        }
        else
        {
            Match_Drag_Main.OBJ_Match_Drag_Main.STR_dummy = this.name;
           // Debug.Log("no other");
            
            if(this.transform.parent.name!= "Options")
            {
                Match_Drag_Main.OBJ_Match_Drag_Main.THI_Count(false);
            }
            Match_Drag_Main.OBJ_Match_Drag_Main.THI_BacktoOriginalpos();
        }
    }


    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.transform.parent.name == "Questions")
        {
            otherGameObject = other.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.transform.parent.name == "Questions")
        {
            otherGameObject = null;
        }
    }

   

}