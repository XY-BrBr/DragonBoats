using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShakeLeftBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Animator> animators;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.isShakeRight = false;
        GameManager.Instance.isShaking = true;
        animators.AnimaSetBool("DoAnim", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.Instance.isShaking = false;
        animators.AnimaSetBool("DoAnim", false);
    }

}
