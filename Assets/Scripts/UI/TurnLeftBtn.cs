using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnLeftBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPress = false;
    public List<Animator> animators;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (isPress)
        {
            //ameManager.Instance.ChangeRotate(false);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        //GameManager.Instance.isRotating = true;
        animators.AnimaSetBool("DoAnim", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
        //GameManager.Instance.isRotating = false;
        //GameManager.Instance.ChangeRotate(false);
        animators.AnimaSetBool("DoAnim", false);
    }
}
