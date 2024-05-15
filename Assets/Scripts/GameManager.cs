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
    public DragonBoatData_SO boatData;
    public DragonBoatMovement boatMovement;

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

    #region Unity Base Method

    // Start is called before the first frame update
    void Start()
    {
        nowBuffPoint = 0;
        buffList = new int[15];

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    #endregion

    public DragonBoatData_SO InitDragonBoat()
    {
        return boatData;
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


    public void ShakeControl(bool isRight)
    {
        
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
