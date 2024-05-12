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
    [Description("扒手")]
    Boatman,

    [Description("舵手")]
    Helmsman,

    [Description("鼓手")]
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
    #region 龙船数据
    public float addSpeed = 2f; //加速度
    public float resistanceSpeed = 1f; //模拟阻碍力
    public float slowSpeed = 1f;
    public float maxSpeed = 40f;
    public float currentSpeed = 0f;

    public bool isRotating = false;
    public float rotateSpeed = 0.2f;//正常旋转速度
    public float rotateAdd = 0.1f;//加速旋转

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

    [Header("移动控制组件")]
    [SerializeField]
    [Tooltip("扒手控制组件")]
    BoatManController boatmanController;
    [Tooltip("扒手控制组件")]
    HelmsmanController helmsmanController;
    [Tooltip("扒手控制组件")]
    DrummerController drummerController;

    [Header("鼓手移动控制相关")]
    public bool canBuff = true;
    public bool getBuff = false;
    public int nowBuffPoint;
    public int[] buffList;

    [Header("测试用面板")]
    public Button SwitchPlayerType_Btn;
    public Text PlayerType_Text;

    public float ShipAcceleration;
    public PlayerType playerType;

    float ReTime = 7f; //失败界面显示倒计时

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

            PlayerType_Text.text = $"当前为 {playerType.GetDscription()} 模式";
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
                Debug.Log("返回菜单");
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
    /// 初始化角色
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

    ///旋转逻辑：
    ///如果在档，挡的方向与转的方向  一致  ==》 加速旋转且急速减速 ，船身会向转弯方向急速倾斜 (漂移效果)
    ///          档的方向与转的方向不一致  ==》 旋转速度微微减慢且有减速 ，船身会向转弯方向微微倾斜 
    ///          特殊情况：如果没速度(当前设置为5) 档只能有减速效果
    ///          
    ///如果没档：船身只会向转弯方向慢慢倾斜（比档的方向与转的方向不一致的时候要多）
    public void RotateControl(bool isRight)
    {
        //旋转方向
        float dir = isRight ? 1 : -1;
        float shakedir;

        if (isShaking)
        {
            isSameDir = false;
            //档的方向
            shakedir = isShakeRight ? 1 : -1;

            //挡的方向相同
            if (dir == shakedir)
            {
                isSameDir = true;
                //向旋转方向加速旋转
                currentRotateSpeed = (rotateSpeed + rotateAdd) * dir;

                //船身向加速方向倾斜加大
                currentShakeSpeed = (shakeSpeed + shakeAdd) * dir;
            }
            else
            {
                //转弯变慢
                currentRotateSpeed = rotateSpeed * dir * 0.75f;

                //船身缓慢倾斜
                currentShakeSpeed = shakeSpeed * dir * 0.5f;
            }
        }
        else
        {
            //船身只会向转弯方向慢慢倾斜
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
    /// 获取Buff效果
    /// </summary>
    /// <param name="buffList">鼓点数组</param>
    /// <param name="nowBuffPoint">数组指针</param>
    /// <returns></returns>
    public string CheckBuff(int[] buffList, int nowBuffPoint)
    {
        string buffName;
        if (buffList[1] == 1 && buffList[2] == 1 && buffList[3] == 1 && nowBuffPoint == 4)
            buffName = "获得加速效果";
        else if (buffList[1] == 2 && buffList[2] == 2 && nowBuffPoint == 3 && nowBuffPoint == 3)
            buffName = "获得转弯加速效果";
        else if (buffList[1] == 2 && buffList[2] == 1 && nowBuffPoint == 3 && nowBuffPoint == 3)
            buffName = "获得无敌效果";
        else
            buffName = "无效";

        return buffName;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}
