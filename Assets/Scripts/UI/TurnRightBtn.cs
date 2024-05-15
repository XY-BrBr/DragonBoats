using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnRightBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPress;
    public List<Animator> animators;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (isPress)
        {
            //GameManager.Instance.ChangeRotate(true);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        //GameManager.Instance.isRotating = true;
        //animators.AnimaSetBool("DoAnim", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
        //GameManager.Instance.isRotating = false;
        //GameManager.Instance.ChangeRotate(false);
        //animators.AnimaSetBool("DoAnim", false);
    }
}
