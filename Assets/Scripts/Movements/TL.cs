using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TL : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressTurnL = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressTurnL = false;
    }
}
