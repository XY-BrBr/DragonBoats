using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("��ͬ��ɫ���ӽ�")]
    public GameObject shiperView;
    public GameObject helmsmanView;
    public GameObject drummerView;

    public void InitCamere()
    {
        shiperView.SetActive(GameManager.Instance.playerType == PlayerType.Boatman);
        helmsmanView.SetActive(GameManager.Instance.playerType == PlayerType.Helmsman);
        drummerView.SetActive(GameManager.Instance.playerType == PlayerType.Dummer);
    }
}
