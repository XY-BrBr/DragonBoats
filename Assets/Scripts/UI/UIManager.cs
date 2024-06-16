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

    float alpha;
    float second;

    public Image BlackImage;
    public Canvas LoseParent;
    public Button RestartBtn;

    public Button EndScreen;
    public GameObject RewardScreen;

    public Text RoundCount_Text;
    public Text Time_Text;

    public Text FirstTime_Text;
    public Text Gold_Text;

    public Animator animator;

    private void Start()
    {
        nowBuffPoint = 0;
        buffList = new List<int>();
        Buff_Text.gameObject.SetActive(false);

        RestartBtn.onClick.AddListener(() => 
        {
            MenuUI.Instance.SetGameObjectActive();
            SceneManager.LoadScene("Menu");
        });

        EndScreen.onClick.AddListener(() =>
        {
            if(Managers.Instance.isStory == true)
            {
                SceneManager.LoadScene("Story");
                Managers.Instance.isEnd = true;
                return;
            }

            string str1;
            int minute = (int)GameManager.Instance.timer / 60;
            int second = (int)GameManager.Instance.timer - minute * 60;
            int millisecond = (int)((GameManager.Instance.timer - (int)GameManager.Instance.timer) * 100);

            str1 = $"{minute:00}:{second:00}:{millisecond:00}";
            ShowRewardScreen(str1);
        });
    }

    private void Update()
    {
        PowerBarReady.fillAmount = GameManager.Instance.boatMovement.CurrentSpeed / GameManager.Instance.boatMovement.MaxSpeed;
        PowerBarSteady.fillAmount = (GameManager.Instance.boatMovement.MaxSpeed - GameManager.Instance.boatData.maxSpeed) / GameManager.Instance.boatMovement.MaxSpeed;
        
        //Ping_Text.text = "Ping = " +  + "ms";
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

    /// <summary>
    /// 隐藏控制窗口面板
    /// </summary>
    public void HideControllerPanel()
    {
        ShiperParent.gameObject.SetActive(false);
        HelmsmanParent.gameObject.SetActive(false);
        DummerParent.gameObject.SetActive(false);
    }

    public void Lose(int type)
    {
        LoseParent.gameObject.SetActive(true);
        for(int i = 0; i < 1; i++)
        {
            LoseParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        LoseParent.transform.GetChild(type).gameObject.SetActive(true);
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

    public void FillTimeText(string str)
    {
        Time_Text.text = str;
    }

    public void ShowRewardScreen(string str1)
    {
        RewardScreen.SetActive(true);
        GameManager.Instance.rewardData.GoldCount += 1;
        FirstTime_Text.text = str1;
        Gold_Text.text = "" + GameManager.Instance.rewardData.GoldCount;
    }
}
