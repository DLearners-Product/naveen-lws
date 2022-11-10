using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject G_pauseMenu;
    public GameObject G_resumeButton, G_dashboardButton;
    public float F_volume;
    public Slider SL_volume;
    public AudioSource AS_BGM;

    private void Start()
    { 
        if (GameObject.Find("BGM") != null)
        {
            AS_BGM = GameObject.Find("BGM").GetComponent<AudioSource>();
            F_volume = AS_BGM.volume;
        }
        if(SL_volume!=null)
        SL_volume.value = F_volume;

        Time.timeScale = 1;
        if(G_dashboardButton!=null)
        G_dashboardButton.SetActive(true);
        G_resumeButton.SetActive(true);
    }

    public void SL_volumeChange()
    {
        if (AS_BGM != null && SL_volume !=null)
        {
            F_volume = SL_volume.value;
            AS_BGM.volume = F_volume;
        }
    }

    public void BUT_pause()
    {
        G_pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void BUT_resume()
    {
        G_pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void BUT_dashboard()
    {
#if UNITY_ANDROID || UNITY_IOS
        Screen.orientation = ScreenOrientation.Portrait;
        Destroy(VAKT_controller.instance.G_currentActivity);
#elif UNITY_WEBGL
      Application.ExternalEval("closeApplication()");
#endif

    }
}
