using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragonBoatMovement : MonoBehaviour, IPunObservable
{
    public DragonBoatData_SO currentBoatData;

    //���
    public GameObject Ship;
    public GameObject ShipBody;
    public GameObject Foam;

    Rigidbody rigid;
    PhotonView photonView;

    float ReTime = 7f; //ʧ�ܽ�����ʾ����ʱ

    #region Unity Base Method
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ReTime = 5f;

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

        float angle = ShipBody.transform.EulerAngles2InspectorRotation_Ex().x;

        if (angle > 39f || angle < -39f)
        {
            Time.timeScale = 0;
            UIManager.Instance.Lose();
            ReTime -= Time.fixedUnscaledDeltaTime;
            if (ReTime < 0)
            {
                Debug.Log("���ز˵�");
                SceneManager.LoadSceneAsync("Menu");
            }
        }

        rigid.velocity = GameManager.Instance.Ship.transform.forward * currentBoatData.currentSpeed;
    }

    private void FixedUpdate()
    {
        float angle = ShipBody.transform.EulerAngles2InspectorRotation_Ex().x;

        if (!isRotating)
        {
            if (angle > 0.1f)
                ShipBody.transform.Rotate((ShakeSpeed + ShakeAdd + ReturnShakeSpeed) * -1, 0, 0);
            else if (angle < -0.1f)
                ShipBody.transform.Rotate((ShakeSpeed + ShakeAdd + ReturnShakeSpeed) * 1, 0, 0);
            else
                return;
        }
    }
    #endregion

    #region Read from Data_SO

    #region Base Property
    public float MaxSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.maxSpeed; else return 0; }
    }

    public float MinSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.minSpeed; else return 0; }
    }

    public float RotateSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.rotateSpeed; else return 0; }
    }

    public float ShakeSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.shakeSpeed; else return 0; }
    }
    #endregion

    #region Active Property
    public float CurrentSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.currentSpeed; else return 0; }
        set { currentBoatData.currentSpeed = Mathf.Clamp(value, 0, currentBoatData.maxSpeed); }
    }

    public float CurrentRotateSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.currentRotateSpeed; else return 0; }
        set { currentBoatData.currentRotateSpeed = value; }
    }

    public float CurrentShakeSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.currentShakeSpeed; else return 0; }
        set { currentBoatData.currentShakeSpeed = value; }
    }
    #endregion

    #region Others Property
    //TODO:��������������Ļ�ȡ�����ݶ���
    public float AddSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.addSpeed; else return 0; }
        set { currentBoatData.addSpeed = value; }
    }

    public float SlowSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.slowSpeed; else return 0; }
        set { currentBoatData.addSpeed = value; }
    }

    public float RotateAdd
    {
        get { if (currentBoatData != null) return currentBoatData.rotateAdd; else return 0; }
        set { currentBoatData.addSpeed = value; }
    }

    public float ReturnShakeSpeed
    {
        get { if (currentBoatData != null) return currentBoatData.returnShakeSpeed; else return 0; }
        set { currentBoatData.addSpeed = value; }
    }

    public float ShakeAdd
    {
        get { if (currentBoatData != null) return currentBoatData.shakeAdd; else return 0; }
        set { currentBoatData.addSpeed = value; }
    }
    #endregion

    #endregion

    public bool isRotating = false;
    public bool isShaking = false;
    public bool isShakeRight = false;
    public bool isSameDir = false;

    #region BoatMan Logic
    public void GetAcceleration()
    {
        CurrentSpeed += AddSpeed;

        photonView.RPC("DoMove", RpcTarget.All, CurrentSpeed);
    }

    [PunRPC]
    private void DoMove(float currentSpeed)
    {
        CurrentSpeed = currentSpeed;
    }
    #endregion

    #region Helmsman Logic

    ///��ת�߼���
    ///����ڵ������ķ�����ת�ķ���  һ��  ==�� ������ת�Ҽ��ټ��� ���������ת�䷽������б (Ư��Ч��)
    ///          ���ķ�����ת�ķ���һ��  ==�� ��ת�ٶ�΢΢�������м��� ���������ת�䷽��΢΢��б 
    ///          ������������û�ٶ�(��ǰ����Ϊ5) ��ֻ���м���Ч��
    ///          
    ///���û��������ֻ����ת�䷽��������б���ȵ��ķ�����ת�ķ���һ�µ�ʱ��Ҫ�ࣩ
    public void RotateControl(bool isRight)
    {
        //��ת����
        float dir = isRight ? 1 : -1;
        float shakedir;

        if (isShaking)
        {
            isSameDir = false;
            //���ķ���
            shakedir = isShakeRight ? 1 : -1;

            //���ķ�����ͬ
            if (dir == shakedir)
            {
                isSameDir = true;
                //����ת���������ת
                CurrentRotateSpeed = (RotateSpeed + RotateAdd) * dir;

                //��������ٷ�����б�Ӵ�
                CurrentShakeSpeed = (ShakeSpeed + ShakeAdd) * dir;
            }
            else
            {
                //ת�����
                CurrentRotateSpeed = RotateSpeed * dir * 0.75f;

                //��������б
                CurrentShakeSpeed = ShakeSpeed * dir * 0.5f;
            }
        }
        else
        {
            //����ֻ����ת�䷽��������б
            CurrentRotateSpeed = RotateSpeed * dir;
            CurrentShakeSpeed = ShakeSpeed * dir;
        }

        if (CurrentSpeed >= 5f)
        {
            Ship.transform.Rotate(0, CurrentRotateSpeed, 0);
            Foam.transform.Rotate(0, -CurrentRotateSpeed, 0);
        }

        ShipBody.transform.Rotate(CurrentShakeSpeed, 0, 0);
    }

    public void ChangeRotate(bool isRight)
    {
        ShakeAdd = isRight ? ShakeAdd : -ShakeAdd;
        photonView.RPC("NetChangeRotate", RpcTarget.All, isRotating, isShaking, isShakeRight, isRight);
    }

    [PunRPC]
    public void NetChangeRotate(bool isRotating, bool isShaking, bool isShakeRight, bool isRight)
    {
        this.isRotating = isRotating;
        this.isShaking = isShaking;
        this.isShakeRight = isShakeRight;
        RotateControl(isRight);
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
