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
    [Header("不同身份的玩家控制面板")]
    public Canvas ShiperParent;
    public Image PowerBarSteady;
    public Image PowerBarReady;
    public Transform PowerPoint;

    public Canvas HelmsmanParent;

    public Canvas DummerParent;

    [Header("鼓点面板相关")]
    public Text Buff_Text;
    public Canvas GetBuffList;
    public List<int> buffList;
    public int nowBuffPoint;

    public Canvas LoseParent;

    public Animator animator;

    private void Start()
    {
        nowBuffPoint = 0;
        buffList = new List<int>();
        Buff_Text.gameObject.SetActive(false);
    }

    private void Update()
    {
        PowerBarReady.fillAmount = GameManager.Instance.boatMovement.CurrentSpeed / GameManager.Instance.boatMovement.MaxSpeed;
    }

    /// <summary>
    /// 初始化控制窗口面板
    /// </summary>
    public void InitControllerPanel()
    {
        ShiperParent.gameObject.SetActive(GameManager.Instance.playerType == PlayerType.Boatman);
        HelmsmanParent.gameObject.SetActive(GameManager.Instance.playerType == PlayerType.Helmsman);
        DummerParent.gameObject.SetActive(GameManager.Instance.playerType == PlayerType.Dummer);
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

        GetBuffList.transform.GetChild(nowBuffPoint).GetComponent<Image>().color = isMiddle ? Color.red : Color.yellow;
        GetBuffList.transform.GetChild(nowBuffPoint).gameObject.SetActive(true);
        buffList.Add(isMiddle ? 0 : 1);
        nowBuffPoint += 1;
    }

    /// <summary>
    /// Buff效果UI显示
    /// </summary>
    public void ShowBuff(string buffName)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            GetBuffList.transform.GetChild(i).gameObject.SetActive(false);
        }

        Buff_Text.text = buffName;
        Buff_Text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Buff效果UI隐藏
    /// </summary>
    public void HideBuff()
    {
        buffList = new List<int>();
        nowBuffPoint = 0;

        Buff_Text.text = "";
        Buff_Text.gameObject.SetActive(false);
    }

    #endregion
}
