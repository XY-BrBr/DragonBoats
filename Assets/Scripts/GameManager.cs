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

    public BuffManager buffManager;

    [Header("���������")]
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


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}
