using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : Singleton<UIManager>
{
    //UI���
    public Canvas ShiperParent;
    public Image PowerBarSteady;
    public Image PowerBarReady;
    public Transform PowerPoint;

    public Canvas HelmsmanParent;

    public Canvas DummerParent;

    public Text Buff_Text;
    public Canvas GetBuffList;

    public Canvas LoseParent;

    public Animator animator;

    private void Start()
    {
        Buff_Text.gameObject.SetActive(false);
    }

    private void Update()
    {
        PowerBarReady.fillAmount = GameManager.Instance.boatMovement.CurrentSpeed / GameManager.Instance.boatMovement.MaxSpeed;
    }

    /// <summary>
    /// ��ʼ�����ƴ������
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

    #region ����UI��ʾ�߼�

    /// <summary>
    /// �ĵ��б�ָʾˢ����ʾ
    /// </summary>
    /// <param name="isMiddle">�Ƿ��û����м�</param>
    public void ShowBuffList(bool isMiddle)
    {
        //TODO: �Ѷ��������Ż�һ��
        animator.SetTrigger("DoAnim");

        GetBuffList.transform.GetChild(GameManager.Instance.nowBuffPoint).GetComponent<Image>().color = isMiddle ? Color.red : Color.yellow;
        GetBuffList.transform.GetChild(GameManager.Instance.nowBuffPoint).gameObject.SetActive(true);
        GameManager.Instance.buffList[GameManager.Instance.nowBuffPoint] = isMiddle ? 1 : 2;
        GameManager.Instance.nowBuffPoint += 1;
    }

    /// <summary>
    /// BuffЧ��UI��ʾ
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
    /// BuffЧ��UI����
    /// </summary>
    public void HideBuff()
    {
        Buff_Text.text = "";
        Buff_Text.gameObject.SetActive(false);
    }

    #endregion
}
