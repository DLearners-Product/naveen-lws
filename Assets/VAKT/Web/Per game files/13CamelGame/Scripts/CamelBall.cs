using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamelBall : MonoBehaviour
{

    void Start()
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name=="Camel")
        {
            int randomDir = Random.Range(0, 3);

            if(randomDir==0)
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 100f);

            if (randomDir == 1)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 100f);


            if (randomDir == 2)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 100f);
        }


        if (collision.gameObject.name == "BoundaryR")
        {
            int randomDir = Random.Range(0, 2);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 100f);

            if (randomDir == 0)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 100f);


            if (randomDir == 1)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 100f);
        }

        if (collision.gameObject.name == "BoundaryL")
        {
            int randomDir = Random.Range(0, 2);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 100f);

            if (randomDir == 0)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 100f);


            if (randomDir == 1)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 100f);
        }

        if (collision.gameObject.name == "BoundaryT")
        {
            int randomDir = Random.Range(0, 2);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 100f);

            if (randomDir == 0)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 100f);


            if (randomDir == 1)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 100f);
        }

        if (collision.gameObject.name == "BoundaryB")
        {
            int randomDir = Random.Range(0, 2);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 100f);

            if (randomDir == 0)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 100f);


            if (randomDir == 1)
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 100f);
        }

        if (collision.gameObject.name.Contains("Boundary") || collision.gameObject.name == "Camel")
        {
            //Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
            if(GetComponent<Rigidbody2D>().velocity.magnitude>25)
            {
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * 15f;
               //Debug.Log("NORMALIZED : " + GetComponent<Rigidbody2D>().velocity.magnitude);
            }
        }
        PassageClickManager.instance.AS_weedHit.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TreasureChest(Clone)")
        {
           // collision.gameObject.GetComponent<SpriteRenderer>().sprite = PassageClickManager.instance.SPR_treasureOpen;
            // Destroy(gameObject);
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<DLcoinLerp>().enabled = true;
            PassageClickManager.instance.I_TreasureChestCollected++;
            PassageClickManager.instance.THI_cloneweedandtreasure();
            if (PassageClickManager.instance.I_TreasureChestCollected>= PassageClickManager.instance.I_numberofchesttocollect)
            {
                PassageClickManager.instance.B_camelOver = true;
                PassageClickManager.instance.G_passageQuestion.SetActive(true);
                if(GameObject.Find("MoveControls")!=null)
                GameObject.Find("MoveControls").SetActive(false);
                PassageClickManager.instance.G_passageQuestion.GetComponent<Animator>().Play("question");
                Destroy(gameObject);
            }

        }
    }

}
