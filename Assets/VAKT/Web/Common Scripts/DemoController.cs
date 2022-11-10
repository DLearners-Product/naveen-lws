using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoController : MonoBehaviour
{

    public AnimationClip AC_demo;

    private void Start()
    {
        GetComponent<Animator>().speed = 0;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == MainController.instance.G_coverPageStart)
        {
            GetComponent<Animator>().speed = 1;
            Invoke("THI_offDemo", AC_demo.length);
        }
    }
    void THI_offDemo()
    {
        gameObject.SetActive(false);
    }
}