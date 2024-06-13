using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Animator> anim;

    public void OnPointerDown(PointerEventData eventData)
    {
        anim.AnimaSetBool("DoAnim", true);
        HelmsmanController.Instance.pressTurnR = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        anim.AnimaSetBool("DoAnim", false);
        HelmsmanController.Instance.pressTurnR = false;
    }
}
