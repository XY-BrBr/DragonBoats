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
    public DragonBoatData_SO boatData;
    public DragonBoatMovement boatMovement;

    public float resistanceSpeed = 0.1f;

    public GameObject Ship;
    public Camera cam;

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

    public BuffManager buffManager;

    [Header("���������")]
    public Button SwitchPlayerType_Btn;
    public Text PlayerType_Text;

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

        cam = Camera.main;

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

        UIManager.Instance.InitControllerPanel();
        cam.GetComponent<CameraController>().InitCamere();
    }


    public void ShakeControl(bool isRight)
    {
        
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
        //throw new NotImplementedException();
    }
}
