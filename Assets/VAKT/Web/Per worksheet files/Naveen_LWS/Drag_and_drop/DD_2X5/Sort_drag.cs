using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Sort_drag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 mousePos;
    public Vector2 initalPos;

    float F_diff_X, F_diff_Y;
    bool isdrag;
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
           
            Sort_DD_Main.OBJ_sort_DD_Main.STR_dummy = this.name;
            if (otherGameObject.transform.childCount == 0)
            {
                // Debug.Log("Empty Occupy");
                if (this.transform.parent.name == "Content")
                {
                    Sort_DD_Main.OBJ_sort_DD_Main.THI_Count(true);
                }
                this.transform.SetParent(otherGameObject.transform, false);
                this.transform.position = otherGameObject.transform.position;
                this.transform.localScale = new Vector2(0.75f, 0.75f);

            }
            else
            {
                if (this.name != otherGameObject.transform.GetChild(0).name)
                {
                    //  Debug.Log("already available other");

                    if (this.transform.parent.name != "Content")
                    {
                        Sort_DD_Main.OBJ_sort_DD_Main.THI_Count(false);
                    }
                    Sort_DD_Main.OBJ_sort_DD_Main.THI_BacktoOriginalpos();
                    this.transform.position = initalPos;
                }
                else
                {
                    this.transform.SetParent(otherGameObject.transform, false);
                    this.transform.position = otherGameObject.transform.position;
                    this.transform.localScale = new Vector2(0.75f, 0.75f);
                }

            }
        }
        else
        {
            Sort_DD_Main.OBJ_sort_DD_Main.STR_dummy = this.name;
            //  Debug.Log("no other");

            if (this.transform.parent.name != "Content")
            {
                Sort_DD_Main.OBJ_sort_DD_Main.THI_Count(false);
            }
            Sort_DD_Main.OBJ_sort_DD_Main.THI_BacktoOriginalpos();
            this.transform.position = initalPos;
        }
    }


    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.transform.parent.name == "Drop")
        {
            otherGameObject = other.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.transform.parent.name == "Drop")
        {
            otherGameObject = null;
        }
    }



}
