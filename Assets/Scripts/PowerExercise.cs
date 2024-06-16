using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PowerExercise : MonoBehaviour
{
    public Rigidbody rigidbody;

    public Button Play_Btn;
    public Image EndScreen;

    public float currentSpeed;

    public float PCaddSpeed;
    public float PlayerSpeed;

    public float time = 1f;
    public float second;

    public bool isTrainning;

    public GameObject WinPoint;
    public GameObject EndPoint;
    public Text FinishGame_Topic;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0;
        isTrainning = true;

        Play_Btn.onClick.AddListener(() =>
        {
            currentSpeed += PlayerSpeed;
        });

        EndScreen.GetComponent<Button>().onClick.AddListener(() =>
        {
            MenuUI.Instance.SetGameObjectActive();
            SceneManager.LoadScene("Menu");
        });
    }

    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        if(second >= time && isTrainning)
        {
            currentSpeed = -PCaddSpeed;
            second = 0;
        }

        rigidbody.velocity = Vector3.left * currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == WinPoint)
        {
            Debug.Log("Win!!");
            StartCoroutine(ShowEndGame(true));
        }
        else if (other.gameObject == EndPoint)
        {
            Debug.Log("Lose!!");
            StartCoroutine(ShowEndGame(false));
        }
    }

    public IEnumerator ShowEndGame(bool isWin)
    {
        float times = 0.7f;
        float seconds = 0;

        isTrainning = false;
        FinishGame_Topic.text = isWin ? "完成训练！" : "继续努力！";

        EndScreen.gameObject.SetActive(true);
        WinPoint.SetActive(false);
        EndPoint.SetActive(false);
        Color color = EndScreen.color;
        color.a = 0;
        EndScreen.color = color;

        while (seconds < times)
        {
            seconds += Time.deltaTime;
            color.a = Mathf.Clamp01(seconds / times - 0.2f);
            EndScreen.color = color;
            yield return null;
        }

        EndScreen.transform.GetChild(0).gameObject.SetActive(true);
        EndScreen.gameObject.GetComponent<Button>().interactable = true;

        yield return null;
    }
}
