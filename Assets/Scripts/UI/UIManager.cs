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
    public GameObject shiperView;
    public GameObject helmanView;
    public GameObject drummerView;

    public Canvas HelmsmanParent;

    public Canvas DummerParent;
    public Button DummerCenter_Btn;
    public Button DummerEdge_Btn;
    public Text Buff_Text;
    public Canvas GetBuffList;

    public Canvas LoseParent;

    [Header("相关数据")]
    public float PowerBarUpSpeed;
    public float PowerBarLowSpeed;
    public float PowerBarToPoint;

    public bool canBuff = true;
    public int nowBuffPoint;
    public int[] buffList;

    public Animator animator;

    private void Start()
    {
        nowBuffPoint = 0;
        buffList = new int[15];
        PowerBarUpSpeed = GameManager.Instance.addSpeed / GameManager.Instance.maxSpeed;
        PowerBarLowSpeed = GameManager.Instance.slowSpeed / GameManager.Instance.maxSpeed;

        Buff_Text.gameObject.SetActive(false);

        DummerCenter_Btn.onClick.AddListener(() => {
            animator.SetTrigger("DoRight");
            if (canBuff)
            {
                canBuff = false;
                GetBuffList.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                GetBuffList.transform.GetChild(0).gameObject.SetActive(true);
                buffList[0] = 1;
                nowBuffPoint += 1;
                StartCoroutine(GetBuffLastTime());
            }
            else
            {
                GetBuffList.transform.GetChild(nowBuffPoint).GetComponent<Image>().color = Color.red;
                GetBuffList.transform.GetChild(nowBuffPoint).gameObject.SetActive(true);
                buffList[nowBuffPoint] = 1;
                nowBuffPoint += 1;
            }
        });

        DummerEdge_Btn.onClick.AddListener(() =>
        {
            animator.SetTrigger("DoLeft");
            if (!canBuff && buffList[nowBuffPoint] == 0)
            {
                Debug.Log("Yellow");
                GetBuffList.transform.GetChild(nowBuffPoint).GetComponent<Image>().color = Color.yellow;
                GetBuffList.transform.GetChild(nowBuffPoint).gameObject.SetActive(true);
                buffList[nowBuffPoint] = 2;
                nowBuffPoint += 1;
            }
        });
    }

    private void Update()
    {
        switch (GameManager.Instance.playerType)
        {
            case PlayerType.Shiper:
                ShiperParent.gameObject.SetActive(true);
                HelmsmanParent.gameObject.SetActive(false);
                DummerParent.gameObject.SetActive(false);

                PowerBarReady.fillAmount = GameManager.Instance.currentSpeed / GameManager.Instance.maxSpeed;

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

    public void PowerBarLow()
    {

    }

    public void Lose()
    {
        LoseParent.gameObject.SetActive(true);
        //PhotonNetwork.LeaveRoom();
    }

    IEnumerator GetBuffLastTime()
    {
        yield return new WaitForSeconds(2);
        DummerCenter_Btn.interactable = false;
        DummerEdge_Btn.interactable = false;
        CheckBuff();
        buffList = new int[15];
        nowBuffPoint = 0;
        yield return new WaitForSeconds(2);
        DummerCenter_Btn.interactable = true;
        DummerEdge_Btn.interactable = true;
        Buff_Text.gameObject.SetActive(false);
        yield break;
    }

    public void CheckBuff()
    {
        for(int i = 0; i < buffList.Length; i++)
        {
            GetBuffList.transform.GetChild(i).gameObject.SetActive(false);
        }
        Buff_Text.gameObject.SetActive(true);

        if (buffList[1] == 1 && buffList[2] == 1 && buffList[3] == 1 && nowBuffPoint == 4)
            Buff_Text.text = "获得加速效果";
        else if (buffList[1] == 2 && buffList[2] == 2 && nowBuffPoint == 3 && nowBuffPoint == 3)
            Buff_Text.text = "获得转弯加速效果";
        else if (buffList[1] == 2 && buffList[2] == 1 && nowBuffPoint == 3 && nowBuffPoint == 3)
            Buff_Text.text = "获得无敌效果";
        else
            Buff_Text.text = "无效";

        canBuff = true;
    }

}
