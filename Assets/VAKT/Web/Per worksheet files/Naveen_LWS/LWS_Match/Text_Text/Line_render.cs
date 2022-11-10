using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_render : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;

    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(points[1]!=null)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
       
    }
    public void setupline(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }
}
