using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public enum PlayerType
{
    [Description("����")]
    Boatman,

    [Description("����")]
    Helmsman,

    [Description("����")]
    Dummer,
}

public static class Enum_Ex
{
    public static string GetDscription(this Enum val)
    {
        var field = val.GetType().GetField(val.ToString());
        var customAttribute = Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
        return customAttribute == null ? val.ToString() : ((DescriptionAttribute)customAttribute).Description;
    }
}

public class GameManager : Singleton<GameManager>, IPunObservable
{
    #region ��������
    public float addSpeed = 2f; //���ٶ�
    public float resistanceSpeed = 1f; //ģ���谭��
    public float slowSpeed = 1f;
    public float maxSpeed = 40f;
    public float currentSpeed = 0f;

    public bool isRotating = false;
    public float rotateSpeed = 0.2f;//������ת�ٶ�
    public float rotateAdd = 0.1f;//������ת

    public bool isShaking = false;
    public bool isShakeRight = false;
    public bool isSameDir = false;
    public float returnShakeSpeed = 0.2f;
    public float shakeSpeed = 0.1f;
    public float shakeAdd = 0.5f;

    [SerializeField] float currentRotateSpeed = 0;
    [SerializeField] float currentShakeSpeed = 0;
    #endregion

    public GameObject Ship;
    public GameObject ShipBody;
    public GameObject Foam;
    public PhotonView photonView;

    [Header("�ƶ��������")]
    [SerializeField]
    [Tooltip("���ֿ������")]
    BoatManController boatmanController;
    [Tooltip("���ֿ������")]
    HelmsmanController helmsmanController;
    [Tooltip("���ֿ������")]
    DrummerController drummerController;

    [Header("�����ƶ��������")]
    public bool canBuff = true;
    public bool getBuff = false;
    public int nowBuffPoint;
    public int[] buffList;

    [Header("���������")]
    public Button SwitchPlayerType_Btn;
    public Text PlayerType_Text;

    public float ShipAcceleration;
    public PlayerType playerType;

    float ReTime = 7f; //ʧ�ܽ�����ʾ����ʱ

    // Start is called before the first frame update
    void Start()
    {
        nowBuffPoint = 0;
        buffList = new int[15];

        ReTime = 5f;

        playerType = PlayerType.Boatman;

        boatmanController = Ship.GetComponent<BoatManController>();
        helmsmanController = Ship.GetComponent<HelmsmanController>();
        drummerController = Ship.GetComponent<DrummerController>();

        InitRole();

        SwitchPlayerType_Btn.onClick.AddListener(() =>
        {
            if (playerType == PlayerType.Dummer)
                playerType = PlayerType.Boatman;
            else
                playerType = playerType + 1;

            PlayerType_Text.text = $"��ǰΪ {playerType.GetDscription()} ģʽ";
            InitRole();
        });
    }

    private void Update()
    {
        float angle = EulerAngles2InspectorRotation(ShipBody.transform.up, ShipBody.transform.eulerAngles).x;

        if (angle > 39f || angle < -39f)
        {
            Time.timeScale = 0;
            UIManager.Instance.Lose();
            ReTime -= Time.fixedUnscaledDeltaTime;
            if (ReTime < 0)
            {
                Debug.Log("���ز˵�");
                SceneManager.LoadSceneAsync("Menu");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = EulerAngles2InspectorRotation(ShipBody.transform.up, ShipBody.transform.eulerAngles).x;

        if (!isRotating)
        {
            if (angle > 0.1f)
                ShipBody.transform.Rotate((shakeSpeed + shakeAdd + returnShakeSpeed) * -1, 0, 0);
            else if (angle < -0.1f)
                ShipBody.transform.Rotate((shakeSpeed + shakeAdd + returnShakeSpeed) * 1, 0, 0);
            else
                return;
        }

        //if (isShaking) { currentSpeed}
    }

    /// <summary>
    /// ��ʼ����ɫ
    /// </summary>
    public void InitRole()
    {
        boatmanController.enabled = false;
        helmsmanController.enabled = false;
        drummerController.enabled = false;

        switch (playerType)
        {
            case PlayerType.Boatman:
                boatmanController.enabled = true;
                break;
            case PlayerType.Helmsman:
                helmsmanController.enabled = true;
                break;
            case PlayerType.Dummer:
                drummerController.enabled = true;
                break;
            default:
                break;
        }
    }

    ///��ת�߼���
    ///����ڵ������ķ�����ת�ķ���  һ��  ==�� ������ת�Ҽ��ټ��� ���������ת�䷽������б (Ư��Ч��)
    ///          ���ķ�����ת�ķ���һ��  ==�� ��ת�ٶ�΢΢�������м��� ���������ת�䷽��΢΢��б 
    ///          ������������û�ٶ�(��ǰ����Ϊ5) ��ֻ���м���Ч��
    ///          
    ///���û��������ֻ����ת�䷽��������б���ȵ��ķ�����ת�ķ���һ�µ�ʱ��Ҫ�ࣩ
    public void RotateControl(bool isRight)
    {
        //��ת����
        float dir = isRight ? 1 : -1;
        float shakedir;

        if (isShaking)
        {
            isSameDir = false;
            //���ķ���
            shakedir = isShakeRight ? 1 : -1;

            //���ķ�����ͬ
            if (dir == shakedir)
            {
                isSameDir = true;
                //����ת���������ת
                currentRotateSpeed = (rotateSpeed + rotateAdd) * dir;

                //��������ٷ�����б�Ӵ�
                currentShakeSpeed = (shakeSpeed + shakeAdd) * dir;
            }
            else
            {
                //ת�����
                currentRotateSpeed = rotateSpeed * dir * 0.75f;

                //��������б
                currentShakeSpeed = shakeSpeed * dir * 0.5f;
            }
        }
        else
        {
            //����ֻ����ת�䷽��������б
            currentRotateSpeed = rotateSpeed * dir;
            currentShakeSpeed = shakeSpeed * dir;
        }

        if (currentSpeed >= 5f)
        {
            Ship.transform.Rotate(0, currentRotateSpeed, 0);
            Foam.transform.Rotate(0, -currentRotateSpeed, 0);
        }

        ShipBody.transform.Rotate(currentShakeSpeed, 0, 0);
        //zAngle += (shakeSpeed + shakeAdd * shake) * -dir;
        //transform.rotation = Quaternion.Euler(0,0, zAngle);
    }

    public void ShakeControl(bool isRight)
    {
        
    }

    public void ChangeRotate(bool isRight)
    {
        shakeAdd = isRight ? shakeAdd : -shakeAdd;
        photonView.RPC("NetChangeRotate", RpcTarget.All, isRotating, isShaking, isShakeRight, isRight);
    }

    [PunRPC]
    public void NetChangeRotate(bool isRotating, bool isShaking, bool isShakeRight, bool isRight)
    {
        this.isRotating = isRotating;
        this.isShaking = isShaking;
        this.isShakeRight = isShakeRight;
        RotateControl(isRight);
    }

    private Vector3 EulerAngles2InspectorRotation(Vector3 up, Vector3 eulerAngle)
    {
        Vector3 resVector = eulerAngle;

        if(Vector3.Dot(up, Vector3.up) >= 0f)
        {
            if(eulerAngle.x >= 0f&& eulerAngle.x <= 90f) { resVector.x = eulerAngle.x; }
            if(eulerAngle.x >= 270f&& eulerAngle.x <= 360f) { resVector.x = eulerAngle.x - 360f; }
        }

        if (Vector3.Dot(up, Vector3.up) < 0f)
        {
            if (eulerAngle.x >= 0f && eulerAngle.x <= 90f) { resVector.x = 180 - eulerAngle.x; }
            if (eulerAngle.x >= 270f && eulerAngle.x <= 360f) { resVector.x = 180 - eulerAngle.x; }
        }

        if (eulerAngle.y > 180)
            resVector.y = eulerAngle.y - 360f;

        if (eulerAngle.z > 180)
            resVector.z = eulerAngle.z - 360f;

        return resVector;
    }

    /// <summary>
    /// ��ȡBuffЧ��
    /// </summary>
    /// <param name="buffList">�ĵ�����</param>
    /// <param name="nowBuffPoint">����ָ��</param>
    /// <returns></returns>
    public string CheckBuff(int[] buffList, int nowBuffPoint)
    {
        string buffName;
        if (buffList[1] == 1 && buffList[2] == 1 && buffList[3] == 1 && nowBuffPoint == 4)
            buffName = "��ü���Ч��";
        else if (buffList[1] == 2 && buffList[2] == 2 && nowBuffPoint == 3 && nowBuffPoint == 3)
            buffName = "���ת�����Ч��";
        else if (buffList[1] == 2 && buffList[2] == 1 && nowBuffPoint == 3 && nowBuffPoint == 3)
            buffName = "����޵�Ч��";
        else
            buffName = "��Ч";

        return buffName;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}
