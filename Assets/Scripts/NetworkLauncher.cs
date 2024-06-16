using Photon.Pun;
using UnityEngine;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    public GameObject StartScreen;
    public GameObject MainScreen;
    public GameObject LoadingScreen;

    private void Start()
    {
        //ConnectToNetwork();
        //GameObject.Find("DontDestoryOnLoad");
    }

    public void ConnectToNetwork()
    {
        StartScreen.SetActive(false);
        LoadingScreen.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        LoadingScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
