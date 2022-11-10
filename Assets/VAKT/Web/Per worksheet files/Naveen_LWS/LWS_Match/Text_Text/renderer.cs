using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderer : MonoBehaviour
{
    public Transform[] points;
    public Line_render line;

    // Start is called before the first frame update
    void Start()
    {
        line.enabled = false;
        line.setupline(points);
    }

    
}
