using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftMoveCamel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        CamelController.instance.B_left = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CamelController.instance.B_left = false;
    }
}
