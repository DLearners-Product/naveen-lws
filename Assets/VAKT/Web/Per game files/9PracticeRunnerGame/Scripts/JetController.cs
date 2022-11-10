using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetController : MonoBehaviour
{
    
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name== "instantiateBG")
        {
            JetGameManager.instance.THI_cloneBG();
        }
        if(collision.gameObject.transform.parent.name.Contains("toCollect"))
        {
            JetGameManager.instance.B_move = false;
            JetGameManager.instance.G_objectDisplay.SetActive(true);
            JetGameManager.instance.G_object.GetComponent<Image>().sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            JetGameManager.instance.G_object.GetComponent<Image>().preserveAspect = true;           
            JetGameManager.instance.THI_delayDisplayObjectDisplay();
            collision.gameObject.GetComponent<SpriteRenderer>().enabled=false;
            JetGameManager.instance.I_points += 5;
            JetGameManager.instance.TEX_points.text = JetGameManager.instance.I_points.ToString();
            MissileController[] missilesIG = FindObjectsOfType<MissileController>();
            foreach(MissileController mc in missilesIG)
            {
                mc.F_missleSpeed = 0;
            }
        }
        if (collision.gameObject.name.Contains("Missile"))
        {
            Destroy(collision.gameObject);
            JetGameManager.instance.B_move = false;
            Destroy(JetGameManager.instance.G_jet);
        }
    }
}
