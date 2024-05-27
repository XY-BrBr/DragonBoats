using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragonBoatMovement : MonoBehaviour, IPunObservable
{
    public DragonBoatData_SO currentBoatData;

    //组件
    public GameObject Ship;
    public GameObject ShipBody;
    public GameObject Foam;

    Rigidbody rigid;
    PhotonView photonView;

    public bool isRotating = false;
    public bool isShaking = false;

    float ReTime = 7f; //失败界面显示倒计时

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
        //扒手移动
        if (CurrentSpeed > 0)
        {
            CurrentSpeed -= GameManager.Instance.resistanceSpeed * Time.deltaTime;
        }

        //舵手旋转
        if (CurrentSpeed >= 5f && isRotating)
        {
            Ship.transform.Rotate(0, CurrentRotateSpeed * Time.deltaTime, 0);
            Foam.transform.Rotate(0, -CurrentRotateSpeed * Time.deltaTime, 0);
        }

        ShipBody.transform.Rotate(CurrentShakeSpeed * Time.deltaTime, 0, 0);

        float angle = ShipBody.transform.EulerAngles2InspectorRotation_Ex().x;

        if (angle > 39f || angle < -39f)
        {
            Time.timeScale = 0;
            UIManager.Instance.Lose();
            ReTime -= Time.fixedUnscaledDeltaTime;
            if (ReTime < 0)
            {
                Debug.Log("返回菜单");
                SceneManager.LoadSceneAsync("Menu");
            }
        }

        rigid.velocity = GameManager.Instance.Ship.transform.forward * CurrentSpeed;
    }

    private void FixedUpdate()
    {
        float angle = ShipBody.transform.EulerAngles2InspectorRotation_Ex().x;

        if (!isRotating)
        {
            Debug.Log(angle);
            if (angle > 0.1f)
                ShipBody.transform.Rotate(ReturnShakeSpeed * -1 * Time.deltaTime, 0, 0);
            else if (angle < -0.1f)
                ShipBody.transform.Rotate(ReturnShakeSpeed * 1 * Time.deltaTime, 0, 0);
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
    //TODO:后续根据需求更改获取的数据对象
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

        photonView.RPC("DoMove", RpcTarget.Others, CurrentSpeed);
    }

    [PunRPC]
    private void DoMove(float currentSpeed)
    {
        CurrentSpeed = currentSpeed;
    }
    #endregion

    #region Helmsman Logic

    ///旋转逻辑：
    ///如果在档，挡的方向与转的方向  一致  ==》 加速旋转且急速减速 ，船身会向转弯方向急速倾斜 (漂移效果)
    ///          档的方向与转的方向不一致  ==》 旋转速度微微减慢且有减速 ，船身会向转弯方向微微倾斜 
    ///          特殊情况：如果没速度(当前设置为5) 档只能有减速效果
    ///          
    ///如果没档：船身只会向转弯方向慢慢倾斜（比档的方向与转的方向不一致的时候要多）
    public void RotateControl(bool isTurnRight, bool isSameDir)
    {
        //旋转方向
        float dir = isTurnRight ? 1 : -1;

        if (!isRotating)
        {
            CurrentShakeSpeed = 0;
            CurrentRotateSpeed = 0;
            photonView.RPC("NetChangeRotate", RpcTarget.Others, isRotating, CurrentRotateSpeed, CurrentShakeSpeed);
            return;
        }

        if (isShaking)
        {
            //挡的方向相同
            if (isSameDir)
            {
                //向旋转方向加速旋转
                CurrentRotateSpeed = (RotateSpeed + RotateAdd) * dir;

                //船身向加速方向倾斜加大
                CurrentShakeSpeed = (ShakeSpeed + ShakeAdd) * dir;
            }
            else
            {
                //转弯变慢
                CurrentRotateSpeed = RotateSpeed * dir * 0.75f;

                //船身缓慢倾斜
                CurrentShakeSpeed = ShakeSpeed * dir * 0.5f;
            }
        }
        else
        {
            //船身只会向转弯方向慢慢倾斜
            CurrentRotateSpeed = RotateSpeed * dir;
            CurrentShakeSpeed = ShakeSpeed * dir;
        }

        photonView.RPC("NetChangeRotate", RpcTarget.Others, isRotating, isShaking, CurrentRotateSpeed, CurrentShakeSpeed);
    }

    [PunRPC]
    public void NetChangeRotate(bool isRotating, bool isShaking, float currentRotateSpeed, float currentShakeSpeed)
    {
        this.isRotating = isRotating;
        this.isShaking = isShaking;
        CurrentRotateSpeed = currentRotateSpeed;
        CurrentShakeSpeed = currentShakeSpeed;
        //RotateControl(isRight);
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
