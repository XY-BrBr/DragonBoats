using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TL : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Animator> anim;

    public void OnPointerDown(PointerEventData eventData)
    {
        anim.AnimaSetBool("DoAnim", true);
        HelmsmanController.Instance.pressTurnL = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        anim.AnimaSetBool("DoAnim", false);
        HelmsmanController.Instance.pressTurnL = false;
    }
}
