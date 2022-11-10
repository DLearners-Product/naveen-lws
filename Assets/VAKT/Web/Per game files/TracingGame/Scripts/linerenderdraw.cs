using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class linerenderdraw : MonoBehaviour
{
  
  
    public GameObject brush;
    public Image IM_traceImage;
    LineRenderer[] LA_Clones;
    LineRenderer currentLineRenderer;
    Vector2 lastPos;

    private void Start()
    {
        //  IM_traceImage.sprite = GET from DATABASE
       // IM_traceImage.preserveAspect = true;
    }

    private void Update()
    {
        Drawing();
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos != lastPos)
            {
                RaycastHit2D hit2d = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit2d.collider!=null && hit2d.collider.name == "Artboard")
                {
                    AddAPoint(mousePos);
                    lastPos = mousePos;
                }      
            }
        }
        else
        {
            currentLineRenderer = null;
        }
    }


    public void BUT_erase()
    {
        for (int i = 0; i < LA_Clones.Length; i++)
        {
            if(LA_Clones[i]!=null)
            Destroy(LA_Clones[i].gameObject);
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        brushInstance.transform.SetParent(this.transform);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        LA_Clones = GameObject.FindObjectsOfType<LineRenderer>();
        //because you gotta have 2 points to start a line renderer, 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
       
    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

}