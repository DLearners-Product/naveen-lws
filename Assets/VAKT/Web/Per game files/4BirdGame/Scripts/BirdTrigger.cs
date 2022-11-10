using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.parent.name == "Ground")
        {
            BirdgameManager.instance.AS_groundHit.Play();
            BirdgameManager.instance.V3_diePos = new Vector3(BirdgameManager.instance.G_HeroBird.transform.position.x, 2.5f);
            BirdgameManager.instance.B_dead = true;
            BirdgameManager.instance.AN_heroBird.Play("birddie");
            BirdgameManager.instance.G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0;
            BirdgameManager.instance.THI_restart();
           // Destroy(collision.gameObject);
          //  BirdgameManager.instance.THI_destroyEnemies();
        }
        if (collision.gameObject.transform.parent.name == "Enemies")
        {
            BirdgameManager.instance.AS_evilBirdHit.Play();
            BirdgameManager.instance.V3_diePos = new Vector3(BirdgameManager.instance.G_HeroBird.transform.position.x, 2.5f);
            BirdgameManager.instance.B_dead = true;
            BirdgameManager.instance.AN_heroBird.Play("birddie");
            BirdgameManager.instance.G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = -0.25f;
            BirdgameManager.instance.THI_restart();
            Destroy(collision.gameObject);
          // BirdgameManager.instance.THI_destroyEnemies();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "coin")
        {
            Destroy(collision.gameObject);
            BirdgameManager.instance.I_points++;
            BirdgameManager.instance.Tex_points.text = BirdgameManager.instance.I_points.ToString();
        }
        if (collision.gameObject.name == "mortar")
        {
            if (!BirdgameManager.instance.B_dead)
            {
                BirdgameManager.instance.B_insideMortarRange = true;
                BirdgameManager.instance.F_mortarTimer = 3f;
                collision.gameObject.transform.parent.GetComponent<Animator>().Play("blimpPull");
            }
        }
        if (collision.gameObject.transform.parent.name == "Questions")
        {
            BirdgameManager.instance.G_question.SetActive(true);
            BirdgameManager.instance.THI_showQuestion();
            BirdgameManager.instance.G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = -1;
            BirdgameManager.instance.F_moveSpeed= 0;
            BirdgameManager.instance.F_jumpForce = 0;
            Destroy(collision.gameObject.GetComponent<Collider2D>());
            BirdgameManager.instance.G_currentQuestion = collision.gameObject;
            Time.timeScale = 0;
            Input.ResetInputAxes();
        }
        if (collision.gameObject.transform.parent.name == "Queen")
        {
            Destroy(BirdgameManager.instance.G_blimp);
            BirdgameManager.instance.G_HeroBird.GetComponent<Collider2D>().enabled = false;
            BirdgameManager.instance.G_HeroBird.GetComponent<Rigidbody2D>().gravityScale = 0;
            BirdgameManager.instance.G_HeroBird.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            BirdgameManager.instance.THI_delayGameComplete();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "mortar")
        {
            if (!BirdgameManager.instance.B_dead)
            {
                BirdgameManager.instance.B_insideMortarRange = false;
                BirdgameManager.instance.TEX_mortarTimer.text = "";
                collision.gameObject.transform.parent.GetComponent<Animator>().Play("blimpidle");
            }
        }
    }
}