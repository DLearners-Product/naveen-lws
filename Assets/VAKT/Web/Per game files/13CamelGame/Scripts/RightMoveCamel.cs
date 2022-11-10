using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightMoveCamel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        CamelController.instance.B_right = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CamelController.instance.B_right = false;
    }
}
