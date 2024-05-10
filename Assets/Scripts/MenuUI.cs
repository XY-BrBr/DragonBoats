using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public Button Quit_Btn;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        Quit_Btn.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
