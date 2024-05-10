using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

enum Buff
{
    [Description("最大速度提升")]
    MaxUp = 2,

    [Description("转向速度提升")]
    RotateFast = 4,

    [Description("无敌时间")]
    Invincible = 8,
}

public class ShiperMovement : MonoBehaviour, IPunObservable
{
    //组件
    Rigidbody rigid;
    PhotonView photonView;

    //用于模拟船向前移动的逻辑
    //测试版1：通过点击鼠标左键加速

    //当前速度

    //最大速度
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
        Debug.Log("旋转");
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
