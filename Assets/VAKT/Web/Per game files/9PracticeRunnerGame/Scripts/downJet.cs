using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class downJet : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        JetGameManager.instance.B_down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JetGameManager.instance.B_down = false;
    }
}
