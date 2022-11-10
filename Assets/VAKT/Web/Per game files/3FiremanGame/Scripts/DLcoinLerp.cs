using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLcoinLerp : MonoBehaviour
{
    public Transform T_endPos;
    public float F_lerpTime;

    private void Start()
    {
        if (GameObject.Find("Fireman")!=null)
        {
            T_endPos = GameObject.Find("Fireman").transform;
        }
        if (GameObject.Find("2CrateGame") != null)
        {
            transform.parent.GetComponent<Animator>().enabled = false;
        }
        if(GameObject.Find("8CamelGame") !=null)
        {
            T_endPos = GameObject.Find("CoinLerp").transform;
            PassageClickManager.instance.AS_coin.Play();
            PassageClickManager.instance.I_points++;
            PassageClickManager.instance.TEX_points.text = PassageClickManager.instance.I_points.ToString();
        }
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        Debug.Log("coin lerp!");
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0,0), F_lerpTime);
        transform.position = Vector2.Lerp(transform.position, T_endPos.position, F_lerpTime);
    }


}
