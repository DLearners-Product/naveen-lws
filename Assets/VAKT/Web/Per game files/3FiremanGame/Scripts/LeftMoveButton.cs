using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        FiremanController.instance.B_moveLeft = true;
        FiremanController.instance.AS_run.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FiremanController.instance.B_moveLeft = false;
        FiremanController.instance.AS_run.Stop();
    }
}
