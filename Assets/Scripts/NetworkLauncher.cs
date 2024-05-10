using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    public GameObject TeamListPanel;
    public GameObject StartButtons;
    public GameObject LoadingPanel;

    public void ConnectToNetwork()
    {
        StartButtons.SetActive(false);
        LoadingPanel.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        LoadingPanel.SetActive(false);
        TeamListPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
