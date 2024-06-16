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

    //���
    public GameObject Ship;
    public GameObject ShipBody;
    public GameObject Foam;

    public BuffManager buffManager;

    Rigidbody rigid;
    PhotonView photonView;

    [Header("�������")]
    public bool needTurn = false;
    public bool canTurn = false;

    [Header("��ת���")]
    public bool isRotating = false;
    public bool isShaking = false;
    public bool isSameDir = false;

    [Header("�����ƶ��������")]
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
        //����������
        if (CurrentSpeed > 0)
        {
            CurrentSpeed -= GameManager.Instance.resistanceSpeed * Time.deltaTime;
        }

        //������ת
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

        //ת��
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
    /// ����ת
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
            //���ķ�����ͬ
            if (this.isSameDir)
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
    /// �ĵ�ˢ�¼�ʱ��
    /// </summary>
    /// <returns></returns>
    IEnumerator GetBuffLastTime()
    {
        //���ļ�ʱ
        yield return new WaitForSeconds(2);

        //���Ľ�������ʼ����Buff
        getBuff = true;
        photonView.RPC("NetSetDrummer", RpcTarget.Others, getBuff, currentBuff);
        yield return new WaitForSeconds(2);

        //Buff����������ָ�״̬
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
                //����
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
