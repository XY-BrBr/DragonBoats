using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelmsmanController : Singleton<HelmsmanController>
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

        movement = GetComponent<DragonBoatMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlTurn();
    }

    ///��ת�߼���
    ///����ڵ������ķ�����ת�ķ���  һ��  ==�� ������ת�Ҽ��ټ��� ���������ת�䷽������б (Ư��Ч��)
    ///          ���ķ�����ת�ķ���һ��  ==�� ��ת�ٶ�΢΢�������м��� ���������ת�䷽��΢΢��б 
    ///          ������������û�ٶ�(��ǰ����Ϊ5) ��ֻ���м���Ч��
    ///          
    ///���û��������ֻ����ת�䷽��������б���ȵ��ķ�����ת�ķ���һ�µ�ʱ��Ҫ�ࣩ
    public void ControlTurn()
    {
        bool isTrunRight, isSameDir;

        movement.isRotating = pressTurnL || pressTurnR;

        movement.isShaking = pressShakeL || pressShakeR;

        isTrunRight = pressTurnR;

        isSameDir = (pressTurnR && pressShakeR) || (pressTurnL && pressShakeL);

        movement.RotateControl(isTrunRight, isSameDir);
    }
}
