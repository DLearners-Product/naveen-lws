using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float F_missleSpeed;

    void Start()
    {

    }

    void Update()
    {
        if (JetGameManager.instance.B_move)
        {
            if (transform.position.x < JetGameManager.instance.G_jet.transform.position.x - 5f)
            {
                Destroy(gameObject);
            }
            transform.Translate(F_missleSpeed * Time.deltaTime * Vector2.left);
        }
    }
}
