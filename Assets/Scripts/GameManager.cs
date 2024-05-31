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

public class GameManager : Singleton<GameManager>, IPunObservable
{
    public DragonBoatData_SO boatData;
    public DragonBoatMovement boatMovement;

    public float resistanceSpeed = 0.1f;

    public GameObject Ship;
    public Camera cam;

    [Header("移动控制组件")]
    [SerializeField]
    [Tooltip("扒手控制组件")]
    BoatManController boatmanController;
    [Tooltip("扒手控制组件")]
    HelmsmanController helmsmanController;
    [Tooltip("扒手控制组件")]
    DrummerController drummerController;

    public BuffManager buffManager;

    [Header("测试用面板")]
    public Button SwitchPlayerType_Btn;
    public Text PlayerType_Text;

    public PlayerType playerType;

    #region Unity Base Method

    // Start is called before the first frame update
    void Start()
    {
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

        UIManager.Instance.InitControllerPanel();
        cam.GetComponent<CameraController>().InitCamere();
    }


    public void ShakeControl(bool isRight)
    {
        
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}
