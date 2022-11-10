using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JS_GameID : MonoBehaviour
{
    public static JS_GameID instance;
    public GameObject G_gameManager;
   

    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Application.ExternalEval("OnAppReady()");     
    }

    public void JS_getID(string val)
    {
        MainController.instance.STR_IDjson = val;
        Debug.Log("json string from javascript : " + val);
        MyJSON myjson = new MyJSON();
        myjson.FetchIDs();
        MainController.instance.mode = "live";

        Debug.Log("calling JS_getID method and setting Live"); 
    }

    public void JS_getMode(string val)
    {
        MainController.instance.mode = "preview";
        MainController.instance.STR_previewJsonAPI = val;

        Debug.Log("calling JS_getMode method and setting preview"); 
    }

   
}

