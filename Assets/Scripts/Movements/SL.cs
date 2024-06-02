using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SL : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressShakeL = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressShakeL = false;
    }
}
