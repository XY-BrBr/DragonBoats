using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceScreen : MonoBehaviour
{
    bool isRace;

    public Button ChaJi_Btn;
    public Button DongSheng_Btn;
    public Button TanTou_Btn;
    public Button ShengTang_Btn;

    public RaceScreen(bool isRace)
    {
        this.isRace = isRace;
    }

    void Start()
    {
        ChaJi_Btn.onClick.AddListener(() => { });
        DongSheng_Btn.onClick.AddListener(() => { });
        TanTou_Btn.onClick.AddListener(() => { SceneManager.LoadSceneAsync("TanTou"); });
        ShengTang_Btn.onClick.AddListener(() => { });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
