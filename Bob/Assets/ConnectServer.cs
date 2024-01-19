using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectServer : MonoBehaviour
{
    public Text connectText;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("p 1");
    }

    void Update()
    {
        connectText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }
    void OnConnectedToMaster(){
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed(){
        RoomOptions roomop = new RoomOptions() { MaxPlayers =  3};
        PhotonNetwork.JoinOrCreateRoom("t" , roomop , TypedLobby.Default);
    }
    void OnJoinedRoom(){
      SceneManager.LoadScene("Bob");
    }
}
