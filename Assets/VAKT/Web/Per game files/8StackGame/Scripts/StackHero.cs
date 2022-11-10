using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHero : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (StackGameManager.instance != null)
        {
            if (collision.gameObject.name.Contains("Ground"))
            {
                StackGameManager.instance.B_isGrounded = true;
                StackGameManager.instance.G_hero.GetComponent<Animator>().Play("idle");
                if (collision.gameObject == StackGameManager.instance.G_currentGround || StackGameManager.instance.G_currentGround == null)
                {

                    StackGameManager.instance.THI_groundClone();
                }
            }
            if (collision.gameObject.name == "out")
            {
                StackGameManager.instance.B_gamePause = true;
                StackGameManager.instance.B_dead = true;
                StackGameManager.instance.G_hero.GetComponent<Animator>().Play("die");
                gameObject.GetComponent<Collider2D>().enabled = false;
                for (int i = 0; i < StackGameManager.instance.GL_clonedGrounds.Count; i++)
                {
                    if (StackGameManager.instance.GL_clonedGrounds[i] != null)
                    {
                        StackGameManager.instance.GL_clonedGrounds[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        StackGameManager.instance.GL_clonedGrounds[i].GetComponent<Rigidbody2D>().gravityScale = 1;
                    }
                }
                StackGameManager.instance.THI_delayRestart();

            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (StackGameManager.instance != null)
        {
            if (collision.gameObject.name.Contains("Ground"))
            {
                StackGameManager.instance.B_isGrounded = false;
                if (!StackGameManager.instance.B_dead)
                {
                    StackGameManager.instance.G_hero.GetComponent<Animator>().Play("jump");
                }
            }
        }
    }
}
