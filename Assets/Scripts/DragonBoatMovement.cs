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

    public bool isRotating = false;
    public bool isShaking = false;

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

        CurrentSpeed = 0f;

        CurrentRotateSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentSpeed > 0)
        {
            CurrentSpeed -= GameManager.Instance.resistanceSpeed * Time.deltaTime;
        }

        if (CurrentSpeed >= 5f && CurrentRotateSpeed != 0)
        {
            Ship.transform.Rotate(0, CurrentRotateSpeed, 0);
            Foam.transform.Rotate(0, -CurrentRotateSpeed, 0);
        }

        ShipBody.transform.Rotate(CurrentShakeSpeed, 0, 0);

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
    public void RotateControl(bool isTurnRight, bool isSameDir)
    {
        //��ת����
        float dir = isTurnRight ? 1 : -1;

        if (!isRotating)
        {
            CurrentRotateSpeed = 0;
            return;
        }

        if (isShaking)
        {
            //���ķ�����ͬ
            if (isSameDir)
            {
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
    }

    public void ChangeRotate(bool isRight)
    {
        ShakeAdd = isRight ? ShakeAdd : -ShakeAdd;
    }

    [PunRPC]
    public void NetChangeRotate(bool isRotating, bool isShaking, bool isShakeRight, bool isRight)
    {

        //RotateControl(isRight);
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
