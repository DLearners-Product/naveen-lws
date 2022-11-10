using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ScrollZoomController : MonoBehaviour
{

    public GameObject G_ObjectToZoom;
    public Slider SL_ZoomSlider;


    public void SliderControl()
    {
        G_ObjectToZoom.transform.localScale = new Vector2(SL_ZoomSlider.value + 1f, SL_ZoomSlider.value + 1f);
    }


    




    
}
