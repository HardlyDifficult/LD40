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
    Vector3 position = ballPrefab.transform.position;
    if (playerList.Length == 1)
    {
      position.z = zPositionPlayer1;
    }
    else
    {
      position.z = zPositionPlayer2;
    }

    PhotonView ball = Instantiate(ballPrefab, position, transform.rotation);
    ball.RequestOwnership();
  }
}
