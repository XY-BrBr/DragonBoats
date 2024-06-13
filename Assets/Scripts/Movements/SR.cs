using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Animator> anim;

    public void OnPointerDown(PointerEventData eventData)
    {
        anim.AnimaSetBool("DoAnim", true);
        HelmsmanController.Instance.pressShakeR = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        anim.AnimaSetBool("DoAnim", false);
        HelmsmanController.Instance.pressShakeR = false;
    }
}
