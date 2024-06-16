using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public RaceScreen raceScreen;

    public Transform MainScreen;
    public Button Setting_Btn;
    public Button Racing_Btn;
    public Button Training_Btn;
    public Button Ranking_Btn;
    public Button Message_Btn;
    public Button PreparationArea_Btn;

    public Button Quit_Btn;

    public GameObject RaceScreenGo;
    public GameObject TeamListPanel;
    public GameObject PreparationArea;
    public GameObject TrainningScreen;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        InitScreen();
        InitEvent();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScreen()
    {
        Setting_Btn = MainScreen.Find("Setting_Img").Find("Setting_Btn").GetComponent<Button>();
        Racing_Btn = MainScreen.Find("Racing_Btn").GetComponent<Button>();
        Training_Btn = MainScreen.Find("Training_Btn").GetComponent<Button>();
        Ranking_Btn = MainScreen.Find("Ranking_Btn").GetComponent<Button>();
        Message_Btn = MainScreen.Find("Message_Btn").GetComponent<Button>();
        PreparationArea_Btn = MainScreen.Find("PreparationArea_Btn").GetComponent<Button>();
    }

    public void InitEvent()
    {
        Setting_Btn.onClick.AddListener(() => { ; });
        Racing_Btn.onClick.AddListener(() => { TeamListPanel.SetActive(true); });
        Training_Btn.onClick.AddListener(() => { TrainningButtonEven(); });
        Ranking_Btn.onClick.AddListener(() => { SceneManager.LoadScene("Story"); });
        Message_Btn.onClick.AddListener(() => { ; });
        PreparationArea_Btn.onClick.AddListener(() => { PreparationArea.SetActive(true); });
    }

    public void SettingButtonEven()
    {

    }

    public void RacingButtonEven(bool isRace)
    {
        raceScreen.isRace = false;
        RaceScreenGo.SetActive(true);
    }

    public void TrainningButtonEven()
    {
        TrainningScreen.SetActive(true);
    }
}
