using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ShiperController : MonoBehaviour, IPunObservable
{
    //组件信息
    DragonBoatMovement shiperMovement;

    //移动端按钮
    public Canvas ShiperParent;
    public Button ShiperTowards_Btn;
    public Button ShiperOrder_Btn;

    public Button TurnRight_Btn;
    public Button TurnLeft_Btn;
    public Button RightSlowDown_Btn;
    public Button LeftSlowDown_Btn;

    public bool isPress = false;
    public List<Animator> animators;

    //主机端数据

    #region 游戏流程控制
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

            Debug.Log("点击事件");
        });
    }

    private void Update()
    {
        
    }

    #endregion

    #region 游戏逻辑

    /// <summary>
    /// 点击按钮加速
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
