using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamelController : MonoBehaviour
{
    public static CamelController instance;
    public float F_speed;
    public float F_jump;
    public bool B_left;
    public bool B_right;
    public bool B_canJump;



    private void Start()
    {
        instance = this;
    }


    void Update()
    {
        if (!PassageClickManager.instance.B_camelOver)
        {
            THI_movement(); THI_clampPosition();
        }
    }

    void THI_movement()
    {

    
        if(Input.GetKey(KeyCode.A) || B_left)
        {
           transform.Translate(F_speed * Time.deltaTime * Vector2.left);
            GetComponent<Animator>().Play("camelrun");
            transform.eulerAngles = new Vector2(transform.rotation.x, 0f);
        //  PassageClickManager.instance.AS_run.Play();
        }
        else if (Input.GetKey(KeyCode.D) || B_right)
        {
          transform.Translate(F_speed * Time.deltaTime * Vector2.left);
            GetComponent<Animator>().Play("camelrun");
            transform.eulerAngles = new Vector2(transform.rotation.x, 180f);
        //    PassageClickManager.instance.AS_run.Play();
        }
        else
        {
            GetComponent<Animator>().Play("camelIdle");
       //     PassageClickManager.instance.AS_run.Stop();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            BUT_Jump();

        }
    }


    void THI_clampPosition()
    {
        Vector3 camelPos = transform.position;

        camelPos.x = Mathf.Clamp(camelPos.x, -7f, 7f);

        transform.position = camelPos;
    }


    public void BUT_Jump()
    {
        if (B_canJump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, F_jump), ForceMode2D.Impulse);
            PassageClickManager.instance.AS_jump.Play();
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name== "BoundaryB")
        {
          B_canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BoundaryB")
        {
            B_canJump = false;
        }
    }

}
