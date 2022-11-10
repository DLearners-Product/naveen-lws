using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class upJet : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        JetGameManager.instance.B_up = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JetGameManager.instance.B_up = false;
    }
}
