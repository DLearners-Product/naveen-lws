using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public AnimationClip AC_introAnim;

    void Start()
    {
        Invoke("THI_hideScreen", AC_introAnim.length);
    }

    void THI_hideScreen()
    {
        gameObject.SetActive(false);
    }

}
