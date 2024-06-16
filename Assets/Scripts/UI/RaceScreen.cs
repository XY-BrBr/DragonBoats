using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RaceScreen : MonoBehaviourPunCallbacks
{
    public bool isRace;
    public string roomName;

    public GameObject Menu;
    public Button ChaJi_Btn;
    public Button DongSheng_Btn;
    public Button TanTou_Btn;
    public Button ShengTang_Btn;
    public Button Return_Btn;

    public RaceScreen(bool isRace, string roomName)
    {
        this.isRace = isRace;
        this.roomName = roomName;
    }

    void Start()
    {
        ChaJi_Btn.onClick.AddListener(() => { RaceSceneLoad("ChaJi"); });
        DongSheng_Btn.onClick.AddListener(() => { RaceSceneLoad("DongSheng"); });
        TanTou_Btn.onClick.AddListener(() => { RaceSceneLoad("TanTou"); });
        ShengTang_Btn.onClick.AddListener(() => { RaceSceneLoad("ShengTang"); });

        Return_Btn.onClick.AddListener(() => 
        {
            MenuUI.Instance.SwitchScene("RaceScreen","MainScreen");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RaceSceneLoad(string sceneName)
    {
        if (isRace)
        {
            Debug.Log("创建房间成功！！");
            Menu.SetActive(false);
            RoomOptions options = new RoomOptions { MaxPlayers = 4 };
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "scene", sceneName } };
            options.CustomRoomPropertiesForLobby = new string[] { "scene" };

            PhotonNetwork.JoinOrCreateRoom(roomName, options, default);
            SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            Menu.SetActive(false);
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
