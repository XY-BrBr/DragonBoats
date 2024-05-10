using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

enum Buff
{
    [Description("����ٶ�����")]
    MaxUp = 2,

    [Description("ת���ٶ�����")]
    RotateFast = 4,

    [Description("�޵�ʱ��")]
    Invincible = 8,
}

public class ShiperMovement : MonoBehaviour, IPunObservable
{
    //���
    Rigidbody rigid;
    PhotonView photonView;

    //����ģ�⴬��ǰ�ƶ����߼�
    //���԰�1��ͨ���������������

    //��ǰ�ٶ�

    //����ٶ�
    //public float maxSpeed = 20f; 

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.currentSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentSpeed > 0)
        {
            GameManager.Instance.currentSpeed -= GameManager.Instance.resistanceSpeed * Time.deltaTime;
        }

        rigid.velocity = GameManager.Instance.Ship.transform.forward * GameManager.Instance.currentSpeed;
    }

    private void FixedUpdate()
    {

    }

    [PunRPC]
    private void DoMove(float currentSpeed)
    {
        GameManager.Instance.currentSpeed = currentSpeed;
    }

    public void DoRotate()
    {
        Debug.Log("��ת");
        transform.Rotate(GameManager.Instance.rotateSpeed * Time.deltaTime, 0, 0);
    }

    public void GetAcceleration()
    {
        if (GameManager.Instance.currentSpeed + GameManager.Instance.addSpeed > GameManager.Instance.maxSpeed)
        {
            GameManager.Instance.currentSpeed = GameManager.Instance.maxSpeed;
        }
        else
        {
            GameManager.Instance.currentSpeed += GameManager.Instance.addSpeed;
        }

        photonView.RPC("DoMove", RpcTarget.All, GameManager.Instance.currentSpeed);
    }

    private Vector3 networkVelocity;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
