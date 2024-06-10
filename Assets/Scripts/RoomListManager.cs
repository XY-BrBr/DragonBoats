using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public GameObject NoRoom_Text;
    public GameObject RoomMessage_pre;
    public Transform gridLayout;

    public GameObject CreateRoomPanel;
    public InputField masterName;
    public InputField roomName;

    private void Update()
    {
        NoRoom_Text.SetActive(gridLayout.childCount <= 0);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < gridLayout.childCount; i++) 
        {
            if(gridLayout.GetChild(i).transform.Find("RoomName_Text").GetComponent<Text>().text == roomList[i].Name)
            {
                Destroy(gridLayout.GetChild(i).gameObject);

                if(roomList[i].PlayerCount == 0)
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
        if (masterName.text.Length <= 2)
        {
            Debug.Log("Error");
            return;
        }

        if (roomName.text.Length <= 2)
        {
            Debug.Log("Error");
            return;
        }

        PhotonNetwork.NickName = masterName.text;
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, default);
    }

    public void DoBack()
    {
        CreateRoomPanel.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
