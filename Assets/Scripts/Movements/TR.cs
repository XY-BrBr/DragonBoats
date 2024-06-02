using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressTurnR = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HelmsmanController.Instance.pressTurnR = false;
    }
}
