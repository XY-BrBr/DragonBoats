using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShakeRightBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //GameManager.Instance.isShakeRight = true;
        //GameManager.Instance.isShaking = true;
        //GameManager.Instance.ChangeRotate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //GameManager.Instance.isShaking = false;
        //GameManager.Instance.ChangeRotate(false);
    }
}
