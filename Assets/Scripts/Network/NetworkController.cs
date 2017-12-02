using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
  [SerializeField]
  PhotonView ballPrefab;

  [SerializeField]
  float zPositionPlayer1;

  [SerializeField]
  float zPositionPlayer2;

  protected void Awake()
  {
    PhotonNetwork.ConnectUsingSettings("0.1");
  }

  protected void OnJoinedLobby()
  {
    PhotonNetwork.JoinRandomRoom();
  }

  protected void OnPhotonRandomJoinFailed()
  {
    RoomOptions roomOptions = new RoomOptions();
    roomOptions.MaxPlayers = 2;
    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
  }

  protected void OnJoinedRoom()
  {
    PhotonPlayer[] playerList = PhotonNetwork.playerList;
    // TODO
    if (playerList.Length == 1)
    {
      //ballPrefab.RequestOwnership();
    }
    else
    {
      //ball2.RequestOwnership();
    }
  }
}
