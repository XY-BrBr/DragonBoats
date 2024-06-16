using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragonBoatMovement : MonoBehaviour, IPunObservable
{
    public DragonBoatData_SO currentBoatData;

    //组件
    public GameObject Ship;
    public GameObject ShipBody;
    public GameObject Foam;

    public BuffManager buffManager;

    Rigidbody rigid;
    PhotonView photonView;

    [Header("扒手相关")]
    public bool needTurn = false;
    public bool canTurn = false;

    [Header("旋转相关")]
    public bool isRotating = false;
    public bool isShaking = false;
    public bool isSameDir = false;

    [Header("鼓手移动控制相关")]
    public bool canBuff = true;
    public bool getBuff = false;
    public int currentBuff;

    float second;
    

    #region Unity Base Method
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        currentBuff = 1;

        currentBoatData = Instantiate(GameManager.Instance.InitDragonBoat());
        buffManager = GetComponent<BuffManager>();

        CurrentSpeed = 0f;

        CurrentRotateSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //场景的阻力
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

        if(isRotating) ShipBody.transform.Rotate(CurrentShakeSpeed * Time.deltaTime, 0, 0);

        float angle = ShipBody.transform.EulerAngles2InspectorRotation_Ex().x;

        if (angle > 39f || angle < -39f)
        {
            Time.timeScale = 0;
            UIManager.Instance.Lose(0);
        }

        if (getBuff)
        {
            getBuff = false;
            buffManager.CheckBuff(currentBuff);
        }

        if (needTurn && CurrentSpeed < 2f)
        {
            Debug.Log(CurrentSpeed);
            canTurn = true;
            needTurn = false;
        }

        rigid.velocity = GameManager.Instance.Ship.transform.forward * CurrentSpeed;
    }

    private void FixedUpdate()
    {
        float angle = ShipBody.transform.EulerAngles2InspectorRotation_Ex().x;

        if (!isRotating)
        {
            //Debug.Log(angle);
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
        set { currentBoatData.maxSpeed = value; }
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

    public void SlowDown()
    {
        CurrentSpeed -= SlowSpeed;

        photonView.RPC("DoMove", RpcTarget.Others, CurrentSpeed);
    }

    public void TurnAround()
    {
        canTurn = false;
        StartCoroutine(GradientBlack());

        photonView.RPC("NetDoTurn", RpcTarget.Others);
    }

    public IEnumerator GradientBlack()
    {
        UIManager.Instance.BlackImage.gameObject.SetActive(true);
        UIManager.Instance.HideControllerPanel();
        GameManager.Instance.isContinueToClock = false;
        float time = 0.7f;
        second = 0;

        Color color = UIManager.Instance.BlackImage.color;
        color.a = 0;
        UIManager.Instance.BlackImage.color = color;

        while (second < time)
        {
            second += Time.deltaTime;
            color.a = Mathf.Clamp01(second / time);
            UIManager.Instance.BlackImage.color = color;
            yield return null;
        }

        color.a = 1f;
        UIManager.Instance.BlackImage.color = color;

        //转身
        DoTurn();
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.BoatToTheEnd();

        second = 0;
        while (second < time)
        {
            second += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (second / time));
            UIManager.Instance.BlackImage.color = color;
            yield return null;
        }

        UIManager.Instance.BlackImage.gameObject.SetActive(false);
        UIManager.Instance.InitControllerPanel();

        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.isContinueToClock = true;
        yield break;
    }

    /// <summary>
    /// 船旋转
    /// </summary>
    public void DoTurn()
    {
        Ship.transform.Rotate(Vector3.up, 180);
        Foam.transform.Rotate(Vector3.up, 180);
    }

    [PunRPC]
    private void DoMove(float currentSpeed)
    {
        CurrentSpeed = currentSpeed;
    }

    [PunRPC]
    private void NetDoTurn()
    {
        StartCoroutine(GradientBlack());
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
        this.isSameDir = isSameDir;

        if (!isRotating)
        {
            CurrentShakeSpeed = 0;
            CurrentRotateSpeed = 0;
            photonView.RPC("NetChangeRotate", RpcTarget.Others, isRotating, isShaking, CurrentRotateSpeed, CurrentShakeSpeed, isSameDir);
            return;
        }

        if (isShaking)
        {
            //挡的方向相同
            if (this.isSameDir)
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

        photonView.RPC("NetChangeRotate", RpcTarget.Others, isRotating, isShaking, CurrentRotateSpeed, CurrentShakeSpeed, isSameDir);
    }

    [PunRPC]
    public void NetChangeRotate(bool isRotating, bool isShaking, float currentRotateSpeed, float currentShakeSpeed, bool isSameDir)
    {
        this.isRotating = isRotating;
        this.isShaking = isShaking;
        CurrentRotateSpeed = currentRotateSpeed;
        CurrentShakeSpeed = currentShakeSpeed;
        this.isSameDir = isSameDir;
        //RotateControl(isRight);
    }
    #endregion

    #region Drummer Logic
    public void DrummerTest(bool isMiddle)
    {
        if (canBuff)
        {
            StartCoroutine(GetBuffLastTime());
            canBuff = false;
        }

        currentBuff = currentBuff << 1;
        currentBuff += isMiddle ? 0 : 1;
    }

    /// <summary>
    /// 鼓点刷新计时器
    /// </summary>
    /// <returns></returns>
    IEnumerator GetBuffLastTime()
    {
        //击鼓计时
        yield return new WaitForSeconds(2);

        //击鼓结束，开始计算Buff
        getBuff = true;
        photonView.RPC("NetSetDrummer", RpcTarget.Others, getBuff, currentBuff);
        yield return new WaitForSeconds(2);

        //Buff计算结束，恢复状态
        getBuff = false;
        canBuff = true;
        currentBuff = 1;
        UIManager.Instance.HideBuff();
        yield break;
    }

    [PunRPC]
    public void NetSetDrummer(bool getBuff,int currentBuff)
    {
        this.getBuff = getBuff;
        this.currentBuff = currentBuff;
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Time.timeScale = 0;
            UIManager.Instance.Lose(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EndPoint"))
        {
            if (Managers.Instance.isStory)
            {
                StartCoroutine(ShowEndGame());
                return;
            }

            if(GameManager.Instance.currentRound + 1 > GameManager.Instance.roundCount
                && GameManager.Instance.CheckPoint[GameManager.Instance.CheckPoint.Count - 1].activeSelf)
            {
                //结束
                StartCoroutine(ShowEndGame());
            }

            needTurn = true;
        }
    }

    public IEnumerator ShowEndGame()
    {
        UIManager.Instance.EndScreen.gameObject.SetActive(true);

        float time = 0.7f;
        float second = 0;

        Color color = UIManager.Instance.EndScreen.gameObject.GetComponent<Image>().color;
        color.a = 0;
        UIManager.Instance.EndScreen.gameObject.GetComponent<Image>().color = color;

        while (second < time)
        {
            second += Time.deltaTime;
            color.a = Mathf.Clamp01(second / time - 0.2f);
            UIManager.Instance.EndScreen.gameObject.GetComponent<Image>().color = color;
            yield return null;
        }

        UIManager.Instance.EndScreen.transform.GetChild(0).gameObject.SetActive(true);
        UIManager.Instance.EndScreen.interactable = true;

        yield return null;
    }
}
