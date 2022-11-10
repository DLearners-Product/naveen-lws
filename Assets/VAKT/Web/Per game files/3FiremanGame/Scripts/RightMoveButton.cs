using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        FiremanController.instance.B_moveRight = true;
        FiremanController.instance.AS_run.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FiremanController.instance.B_moveRight = false;
        FiremanController.instance.AS_run.Stop();
    }
}
