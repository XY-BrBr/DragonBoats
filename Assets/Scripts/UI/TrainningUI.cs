using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainningUI : MonoBehaviour
{
    public GameObject Menu;
    public Button Power_Btn;
    public Button Intimacy_Btn;

    public Button Return_Btn;

    // Start is called before the first frame update
    void Start()
    {
        Power_Btn.onClick.AddListener(()=> 
        {
            Debug.Log("Power!!");
            Menu.SetActive(false);
            SceneManager.LoadScene("Power");
        });

        Intimacy_Btn.onClick.AddListener(() =>
        {
            MenuUI.Instance.SwitchScene("TraninningScreen", "RaceScreen");
        });

        Return_Btn.onClick.AddListener(() => 
        {
            MenuUI.Instance.SwitchScene("TraninningScreen", "MainScreen");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneLoad(string sceneName)
    {
        Menu.SetActive(false);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
