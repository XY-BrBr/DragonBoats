using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressShakeR = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressShakeR = false;
    }
}
