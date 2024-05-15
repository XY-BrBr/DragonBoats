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

public class DragonBoatMovement : MonoBehaviour, IPunObservable
{
    public DragonBoatData_SO currentBoatData;

    //���
    Rigidbody rigid;
    PhotonView photonView;

    //����ģ�⴬��ǰ�ƶ����߼�
    //���԰�1��ͨ���������������

    //��ǰ�ٶ�

    //����ٶ�
    //public float maxSpeed = 20f; 

    #region Unity Base Method
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentBoatData = Instantiate(GameManager.Instance.InitDragonBoat());

        GameManager.Instance.currentSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBoatData.currentSpeed > 0)
        {
            currentBoatData.currentSpeed -= GameManager.Instance.resistanceSpeed * Time.deltaTime;
        }

        rigid.velocity = GameManager.Instance.Ship.transform.forward * currentBoatData.currentSpeed;
    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region Read from Data_SO
    public float CurrentSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.currentSpeed; else return 0; }
        set { if (value < currentBoatData.maxSpeed) currentBoatData.currentSpeed = value; 
            else currentBoatData.currentSpeed = currentBoatData.maxSpeed; }
    }

    public float MaxSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.maxSpeed; else return 0; }
    }
    #endregion

    public void DoRotate()
    {
        Debug.Log("��ת");
        transform.Rotate(GameManager.Instance.rotateSpeed * Time.deltaTime, 0, 0);
    }

    public void GetAcceleration()
    {
        CurrentSpeed += currentBoatData.addSpeed;

        photonView.RPC("DoMove", RpcTarget.All, CurrentSpeed);
    }

    [PunRPC]
    private void DoMove(float currentSpeed)
    {
        CurrentSpeed = currentSpeed;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
