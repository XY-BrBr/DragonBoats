using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : Singleton<UIManager>
{
    //UI组件
    public Canvas ShiperParent;
    public Image PowerBarSteady;
    public Image PowerBarReady;
    public Transform PowerPoint;

    [Header(" 不同角色的视角")]
    public GameObject shiperView;
    public GameObject helmanView;
    public GameObject drummerView;

    public Canvas HelmsmanParent;

    public Canvas DummerParent;

    public Text Buff_Text;
    public Canvas GetBuffList;

    public Canvas LoseParent;

    [Header("相关数据")]
    public float PowerBarUpSpeed;
    public float PowerBarLowSpeed;
    public float PowerBarToPoint;

    public Animator animator;

    private void Start()
    {
        PowerBarUpSpeed = GameManager.Instance.addSpeed / GameManager.Instance.maxSpeed;
        PowerBarLowSpeed = GameManager.Instance.slowSpeed / GameManager.Instance.maxSpeed;

        Buff_Text.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (GameManager.Instance.playerType)
        {
            case PlayerType.Boatman:
                ShiperParent.gameObject.SetActive(true);
                HelmsmanParent.gameObject.SetActive(false);
                DummerParent.gameObject.SetActive(false);

                PowerBarReady.fillAmount = GameManager.Instance.boatMovement.CurrentSpeed / GameManager.Instance.boatMovement.MaxSpeed;

                shiperView.SetActive(true);
                helmanView.SetActive(false);
                drummerView.SetActive(false);

                //if (PowerPoint.rotation.z <= 0 || PowerPoint.rotation.z <= -180)
                //{
                //    if (PowerBarToPoint < 0)
                //        PowerPoint.Rotate(0, 0, -PowerBarLowSpeed * PowerBarToPoint * Time.deltaTime);
                //}
                break;
            case PlayerType.Helmsman:
                ShiperParent.gameObject.SetActive(false);
                HelmsmanParent.gameObject.SetActive(true);
                DummerParent.gameObject.SetActive(false);

                shiperView.SetActive(false);
                helmanView.SetActive(true);
                drummerView.SetActive(false);

                PowerBarReady.fillAmount = GameManager.Instance.currentSpeed / GameManager.Instance.maxSpeed;
                break;
            case PlayerType.Dummer:
                ShiperParent.gameObject.SetActive(false);
                HelmsmanParent.gameObject.SetActive(false);
                DummerParent.gameObject.SetActive(true);

                helmanView.SetActive(false);
                shiperView.SetActive(false);
                drummerView.SetActive(true);

                break;
        }

    }

    public void PowerBarUp()
    {
        PowerBarReady.fillAmount = GameManager.Instance.currentSpeed / GameManager.Instance.maxSpeed;
        //float powerRotate = (PowerPoint.localRotation.z + 180f) % 360f - 180f;

        ////TODO: 过界判定
        //if (powerRotate <= 0)
        //{
        //    if(PowerBarToPoint < 0)
        //        if(powerRotate - PowerBarUpSpeed * PowerBarToPoint <= -180)
        //            //TODO : 这里可能有问题
        //            PowerPoint.Rotate(0, 0, -180 - powerRotate);
        //        else
        //            PowerPoint.Rotate(0, 0, PowerBarUpSpeed * PowerBarToPoint);
        //}
    }

    public void Lose()
    {
        LoseParent.gameObject.SetActive(true);
        //PhotonNetwork.LeaveRoom();
    }

    #region 鼓手UI显示逻辑

    /// <summary>
    /// 鼓点列表指示刷新显示
    /// </summary>
    /// <param name="isMiddle">是否敲击鼓中间</param>
    public void ShowBuffList(bool isMiddle)
    {
        //TODO: 把动画触发优化一下
        animator.SetTrigger("DoAnim");

        GetBuffList.transform.GetChild(GameManager.Instance.nowBuffPoint).GetComponent<Image>().color = isMiddle ? Color.red : Color.yellow;
        GetBuffList.transform.GetChild(GameManager.Instance.nowBuffPoint).gameObject.SetActive(true);
        GameManager.Instance.buffList[GameManager.Instance.nowBuffPoint] = isMiddle ? 1 : 2;
        GameManager.Instance.nowBuffPoint += 1;
    }

    /// <summary>
    /// Buff效果UI显示
    /// </summary>
    public void ShowBuff()
    {
        for (int i = 0; i < GameManager.Instance.buffList.Length; i++)
        {
            GetBuffList.transform.GetChild(i).gameObject.SetActive(false);
        }

        Buff_Text.text = GameManager.Instance.CheckBuff(GameManager.Instance.buffList, GameManager.Instance.nowBuffPoint);
        Buff_Text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Buff效果UI隐藏
    /// </summary>
    public void HideBuff()
    {
        Buff_Text.text = "";
        Buff_Text.gameObject.SetActive(false);
    }

    #endregion
}
