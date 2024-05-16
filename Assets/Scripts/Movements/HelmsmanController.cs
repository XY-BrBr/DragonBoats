using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HelmsmanController : MonoBehaviour
{
    DragonBoatMovement movement;

    public Button TurnRight_Btn;
    public Button TurnLeft_Btn;
    public Button ShakeRight_Btn;
    public Button ShakeLeft_Btn;

    public bool pressTurnR;
    public bool pressTurnL;
    public bool pressShakeR;
    public bool pressShakeL;

    // Start is called before the first frame update
    void Start()
    {
        pressTurnR = false;
        pressTurnL = false;
        pressShakeR = false;
        pressShakeL = false;

        EventTrigger triggerTR = TurnRight_Btn.gameObject.GetComponent<EventTrigger>();
        EventTrigger triggerTL = TurnLeft_Btn.gameObject.GetComponent<EventTrigger>();
        EventTrigger triggerSR = ShakeRight_Btn.gameObject.GetComponent<EventTrigger>();
        EventTrigger triggerSL = ShakeLeft_Btn.gameObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        EventTrigger.Entry entryUp = new EventTrigger.Entry();

        entryDown.eventID = EventTriggerType.PointerDown;
        entryUp.eventID = EventTriggerType.PointerUp;

        entryDown.callback = new EventTrigger.TriggerEvent();
        entryDown.callback.AddListener(OnPointerDown);
        entryUp.callback.AddListener(OnPointerUp);

        triggerTR.triggers.Add(entryDown);
        triggerTR.triggers.Add(entryUp);
        triggerTL.triggers.Add(entryDown);
        triggerTL.triggers.Add(entryUp);
        triggerSR.triggers.Add(entryDown);
        triggerSR.triggers.Add(entryUp);
        triggerSL.triggers.Add(entryDown);
        triggerSL.triggers.Add(entryUp);

        movement = GetComponent<DragonBoatMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlTurn();
    }

    ///旋转逻辑：
    ///如果在档，挡的方向与转的方向  一致  ==》 加速旋转且急速减速 ，船身会向转弯方向急速倾斜 (漂移效果)
    ///          档的方向与转的方向不一致  ==》 旋转速度微微减慢且有减速 ，船身会向转弯方向微微倾斜 
    ///          特殊情况：如果没速度(当前设置为5) 档只能有减速效果
    ///          
    ///如果没档：船身只会向转弯方向慢慢倾斜（比档的方向与转的方向不一致的时候要多）
    public void ControlTurn()
    {
        bool isTrunRight, isSameDir;

        movement.isRotating = pressTurnL || pressTurnR;

        movement.isShaking = pressShakeL || pressShakeR;

        isTrunRight = pressTurnR;

        isSameDir = (pressTurnR && pressShakeR) || (pressTurnL && pressShakeL);

        movement.RotateControl(isTrunRight, isSameDir);
    }

    public void OnPointerDown(BaseEventData pointData)
    {
        if(pointData.selectedObject == TurnRight_Btn.gameObject) { pressTurnR = true; }
        if(pointData.selectedObject == TurnLeft_Btn.gameObject) { pressTurnL = true; }
        if(pointData.selectedObject == ShakeRight_Btn.gameObject) { pressShakeR = true; }
        if(pointData.selectedObject == ShakeLeft_Btn.gameObject) { pressShakeL = true; }
    }

    public void OnPointerUp(BaseEventData pointData)
    {
        if (pointData.selectedObject == TurnRight_Btn.gameObject) { pressTurnR = false; }
        if (pointData.selectedObject == TurnLeft_Btn.gameObject) { pressTurnL = false; }
        if (pointData.selectedObject == ShakeRight_Btn.gameObject) { pressShakeR = false; }
        if (pointData.selectedObject == ShakeLeft_Btn.gameObject) { pressShakeL = false; }
    }
}
