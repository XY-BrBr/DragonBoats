using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomMessage_pre;
    public Transform gridLayout;
    public RaceScreen raceScreen;

    public GameObject CreateRoomPanel;
    public GameObject CreateRoomMessagePanel;
    public InputField masterName;
    public InputField roomName;

    public string sceneName;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("房间变化");

        for (int i = 0; i < gridLayout.childCount; i++)
        {
            if (gridLayout.GetChild(i).transform.Find("RoomName_Text").GetComponent<Text>().text == roomList[i].Name)
            {
                Destroy(gridLayout.GetChild(i).gameObject);

                if (roomList[i].PlayerCount == 0)
                {
                    roomList.Remove(roomList[i]);
                }
            }
        }

        foreach (var room in roomList)
        {
            GameObject newRoom = Instantiate(RoomMessage_pre, gridLayout.position, Quaternion.identity);

            newRoom.transform.Find("RoomName_Text").GetComponent<Text>().text = room.Name;

            newRoom.transform.Find("JoinRoom_Btn").GetComponent<Button>().onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(room.Name);
            });

            newRoom.transform.SetParent(gridLayout);
        }
    }

    public void ShowCreateRoomMessage()
    {
        CreateRoomPanel.SetActive(true);
    }

    public void DoCreate()
    {
        if (roomName.text.Length <= 2)
        {
            Debug.Log("Error");
            return;
        }

        raceScreen.isRace = true;
        raceScreen.roomName = roomName.text;

        CreateRoomMessagePanel.SetActive(false);
        MenuUI.Instance.SwitchScene("TeamListPanel", "RaceScreen");
    }

    public void DoBack()
    {
        MenuUI.Instance.SwitchScene("TeamListPanel", "MainScreen");
    }

    public override void OnJoinedRoom()
    {
        string sceneName = (string)PhotonNetwork.CurrentRoom.CustomProperties["scene"];

        CreateRoomPanel.SetActive(false);
        CreateRoomMessagePanel.SetActive(false);

        MenuUI.Instance.SetGameObjectActiveF();
        PhotonNetwork.LoadLevel(sceneName);
    }
}
