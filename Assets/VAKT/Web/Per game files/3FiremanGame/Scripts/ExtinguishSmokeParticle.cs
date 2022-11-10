using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtinguishSmokeParticle : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name == "Fire")
        {
            var coin = Instantiate(FiremanController.instance.G_coinPrefab);
            coin.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 2f);
            Destroy(other.gameObject);

            FiremanController.instance.I_fire++;
            if(FiremanController.instance.I_fire==FiremanController.instance.I_fireCount)
            {
                FiremanController.instance.G_currentPlatform.GetComponent<fireman_platform>().B_platformCleared = true;
                FiremanController.instance.B_floorCleared = true;
                FiremanController.instance.G_ladderButton.GetComponent<Button>().interactable = true;
                if(!FiremanController.instance.G_currentPlatform.GetComponent<fireman_platform>().B_questionCleared)
                {
                    FiremanController.instance.THI_showQuestion();
                    FiremanController.instance.G_currentPlatform.GetComponent<fireman_platform>().B_questionCleared = true;
                }
            }
        }
    }


}
