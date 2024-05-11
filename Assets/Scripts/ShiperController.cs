using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ShiperController : MonoBehaviour, IPunObservable
{
    //�����Ϣ
    DragonBoatMovement shiperMovement;

    //�ƶ��˰�ť
    public Canvas ShiperParent;
    public Button ShiperTowards_Btn;
    public Button ShiperOrder_Btn;

    public Button TurnRight_Btn;
    public Button TurnLeft_Btn;
    public Button RightSlowDown_Btn;
    public Button LeftSlowDown_Btn;

    public bool isPress = false;
    public List<Animator> animators;

    //����������

    #region ��Ϸ���̿���
    private void Awake()
    {
        
    }

    private void Start()
    {
        shiperMovement = GetComponent<DragonBoatMovement>();

        ShiperTowards_Btn.onClick.AddListener(() => {
            //UIManager.Instance.PowerBarUp();
            if (PhotonNetwork.IsConnected)
            {
                ChangeSpeed();
            }

            Debug.Log("����¼�");
        });
    }

    private void Update()
    {
        
    }

    #endregion

    #region ��Ϸ�߼�

    /// <summary>
    /// �����ť����
    /// </summary>
    public void ChangeSpeed()
    {
        animators.AnimaSetTrigger("DoAnim");
        shiperMovement.GetAcceleration();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    #endregion

}
