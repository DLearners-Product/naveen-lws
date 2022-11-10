using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool B_rotateUp;

    public float F_shootSpeed, F_rotationUp, F_rotationDown;
   

    private void Start()
    {
        THI_randomRotation();
        THI_randomGravity();
        THI_randomSpeed();
        THI_randomRotationForce();
        Destroy(gameObject, 1f); 
    }

    void THI_randomRotation()
    {
        int randomRotation = Random.Range(0, 2);
        if (randomRotation == 0)
        {
            B_rotateUp = true;
        }
        if (randomRotation == 1)
        {
            B_rotateUp = false;
        }
    }
    void THI_randomGravity()
    {
        float randomGravity = Random.Range(-0.5f, 0.5f);
        GetComponent<Rigidbody2D>().gravityScale = randomGravity;
    }
    void THI_randomSpeed()
    {
        float Randomspeed = Random.Range(10f, 15f);
        F_shootSpeed = Randomspeed;
    }
    void THI_randomRotationForce()
    {
        float Upspeed = Random.Range(0, 0.6f);
        F_rotationUp = Upspeed;

        float Downspeed = Random.Range(0, -0.6f);
        F_rotationDown = Downspeed;
    }
    private void Update()
    {
        transform.Translate(F_shootSpeed * Time.deltaTime * Vector2.right);
        if (B_rotateUp)
        {
            transform.Rotate(new Vector3(0, 0, F_rotationUp));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, F_rotationDown));
        }       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("Missile"))
        {
            JetGameManager.instance.I_points++;
            JetGameManager.instance.TEX_points.text = JetGameManager.instance.I_points.ToString();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
