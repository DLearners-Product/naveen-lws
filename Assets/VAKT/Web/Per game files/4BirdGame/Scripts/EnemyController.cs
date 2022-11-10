using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float F_moveSpeed;

    void Start()
    {
        if (!BirdgameManager.instance.B_dead)
        {
            if (transform.position.y > 4)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 1);
            }
            if (transform.position.y < 1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 1);
            }
        }
        else
        {
            BirdgameManager.instance.I_enemiesIG = 0;
            Destroy(gameObject);
        }
    }


    void Update()
    {
        transform.Translate(Vector2.left * F_moveSpeed * Time.deltaTime);
        if(transform.position.x<BirdgameManager.instance.G_HeroBird.transform.position.x-12)
        {
            BirdgameManager.instance.I_enemiesIG = 0;
            Destroy(gameObject);
        }
    }
}
