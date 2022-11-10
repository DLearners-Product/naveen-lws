using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float F_destroyTime;
  
    void Start()
    {
        Destroy(gameObject, F_destroyTime);
    }
}
