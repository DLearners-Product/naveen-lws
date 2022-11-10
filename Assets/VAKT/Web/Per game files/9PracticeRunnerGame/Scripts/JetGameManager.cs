using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JetGameManager : MonoBehaviour
{
    [Header("Instance")]
    public static JetGameManager instance;

    [Header("Movement and Cloning")]
    public bool B_move;
    public bool B_up;
    public bool B_down;
    public float F_jetForward;
    public float F_jetUpDown;
    public GameObject G_jet;
    public GameObject G_bgPrefab;
    public GameObject G_currentBG;
    public GameObject G_camera;
    public List<GameObject> GL_clonedBG;
    public GameObject[] GA_toCollect;
    public GameObject G_clonedCollectionObject;
    public float F_toCollectCloneDuration;


    [Header("Fire")]
    public GameObject G_bullet;
    public GameObject G_firePos;
    public AnimationClip AC_fireAnim;


    [Header("Assigning")]
    public List<Sprite> SPRL_collectionSprites;
    public int I_SpriteAssignNumber;

    [Header("Collection")]
    public GameObject G_objectDisplay;
    public GameObject G_object;
    public int I_points;
    public Text TEX_points;

    [Header("Enemy")]
    public GameObject G_missile;
    public float F_missileTime;

    [Header("Other")]
    public GameObject G_levelComplete;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        I_SpriteAssignNumber = 0;
        THI_assignCollectionObjects();
    }

    void Update()
    {
        THI_keyboardControls();
        THI_jetForward();
        THI_jetControls();
        THI_jetClamp();
        THI_cameraFollow();
        THI_destroyCollectionObjects();
    }

    public void BUT_TaptoStart()
    {
        B_move = true;
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        THI_destroyCollectionObjects();
        StartCoroutine(EN_cloneMissle());
    }

    void THI_jetForward()
    {
        if (B_move)
        {
            G_jet.transform.Translate(F_jetForward * Time.deltaTime * Vector2.right);
        }
    }

    void THI_cameraFollow()
    {
        if (B_move)
        {
            G_camera.transform.position = new Vector3(G_jet.transform.position.x + 4f, G_camera.transform.position.y, -100f);
        }
    }

    void THI_jetControls()
    {
        if (B_move)
        {
            if (B_up)
            {
                G_jet.transform.Translate(F_jetUpDown * Time.deltaTime * Vector2.up);
            }
            if (B_down)
            {
                G_jet.transform.Translate(F_jetUpDown * Time.deltaTime * Vector2.down);
            }
        }
    }

    void THI_keyboardControls()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            B_up = true;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            B_up = false;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            B_down = true;
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            B_down = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BUT_fireBullet();
        }
    }

    void THI_jetClamp()
    {
        if (B_move)
        {
            Vector3 jetPos = G_jet.transform.position;
            jetPos.y = Mathf.Clamp(jetPos.y, -3.0f, 5.0f);
            G_jet.transform.position = jetPos;
        }
    }

    public void THI_cloneBG()
    {
        var clonedBG = Instantiate(G_bgPrefab);
        clonedBG.transform.SetParent(GameObject.Find("BGs").transform, false);
        clonedBG.transform.position = new Vector2(G_currentBG.transform.position.x + 22f, G_currentBG.transform.position.y);
        G_currentBG = clonedBG;
        GL_clonedBG.Add(G_currentBG);
        THI_destroyBGs();
    }

    void THI_destroyBGs()
    {
        for (int i = 0; i < GL_clonedBG.Count - 2; i++)
        {
            if (GL_clonedBG[i] != null)
            {
                Destroy(GL_clonedBG[i]);
            }
        }
        List<GameObject> presentClones = new List<GameObject>();
        presentClones.Add(GL_clonedBG[GL_clonedBG.Count - 2]);
        presentClones.Add(GL_clonedBG[GL_clonedBG.Count - 1]);
        GL_clonedBG = presentClones;
    }

    void THI_cloneToCollect()
    {
        int randomIndex = Random.Range(0, GA_toCollect.Length);
        var toCollect = Instantiate(GA_toCollect[randomIndex]);
        toCollect.transform.SetParent(GameObject.Find("toCollect").transform, false);
        toCollect.transform.position = new Vector2(G_jet.transform.position.x + 20f, toCollect.transform.position.y);
        G_clonedCollectionObject = toCollect;
        THI_assignCollectionObjects();
    }

    void THI_destroyCollectionObjects()
    {
        if (B_move)
        {
            if (G_clonedCollectionObject != null)
            {
                if (G_clonedCollectionObject.transform.GetChild(G_clonedCollectionObject.transform.childCount - 1).gameObject.transform.position.x < G_jet.transform.position.x - 5f)
                {
                    Destroy(G_clonedCollectionObject);
                    THI_cloneToCollect();
                }
            }
        }
    }

    void THI_assignCollectionObjects()
    {
        for (int i = 0; i < G_clonedCollectionObject.transform.childCount; i++)
        {
            G_clonedCollectionObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = SPRL_collectionSprites[I_SpriteAssignNumber];
        }
        I_SpriteAssignNumber++;
        THI_gameComplete();
    }

    void THI_gameComplete()
    {
        if (I_SpriteAssignNumber == SPRL_collectionSprites.Count)
        {
            B_move = false;
            G_levelComplete.SetActive(true);
        }
    }

    void BUT_fireBullet()
    {
        if (B_move)
        {
            var bullet = Instantiate(G_bullet);
            bullet.transform.SetParent(GameObject.Find("BulletsParent").transform, false);
            bullet.transform.position = G_firePos.transform.position;
            G_jet.GetComponent<Animator>().Play("fire");
            Invoke("THI_FlyAnim", AC_fireAnim.length);
        }
    }

    void THI_FlyAnim()
    {
        if(G_jet!=null)
        G_jet.GetComponent<Animator>().Play("JetFly");
    }

    void THI_disableObjectDisplay()
    {
        G_objectDisplay.SetActive(false);
        B_move = true;
        MissileController[] missilesIG = FindObjectsOfType<MissileController>();
        foreach (MissileController mc in missilesIG)
        {
            mc.F_missleSpeed = 5;
        }
    }

    public void THI_delayDisplayObjectDisplay()
    {
        Invoke("THI_disableObjectDisplay", 2f);
    }

    void THI_cloneMissile()
    {
        if (G_jet!=null)
        {
            var missile = Instantiate(G_missile);
            missile.transform.SetParent(GameObject.Find("MissilesParent").transform, false);
            missile.transform.position = new Vector2(G_jet.transform.position.x + 15f, G_jet.transform.position.y);
            StartCoroutine(EN_cloneMissle());
        }
    }
    IEnumerator EN_cloneMissle()
    {
        yield return new WaitForSeconds(F_missileTime);
        THI_cloneMissile();
    }
}