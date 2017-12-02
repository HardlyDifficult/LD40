using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
  [SerializeField]
  GameObject ballPrefab;

  [SerializeField]
  float zPositionPlayer1;

  [SerializeField]
  float zPositionPlayer2;

  RoomOptions roomOptions;

  protected void Awake()
  {
    roomOptions = new RoomOptions()
    {
      MaxPlayers = 2
    };
    PhotonNetwork.ConnectUsingSettings("0.1");
  }

  protected void OnJoinedLobby()
  {
    PhotonNetwork.JoinRandomRoom(roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);
  }

  protected void OnPhotonRandomJoinFailed()
  {
    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
  }

  protected void OnJoinedRoom()
  {
    PhotonPlayer[] playerList = PhotonNetwork.playerList;

    print($"Welcome!  There are a total of {playerList.Length} players in the room");

    Vector3 position = ballPrefab.transform.position;
    if (playerList.Length == 1)
    {
      position.z = zPositionPlayer1;
    }
    else
    {
      position.z = zPositionPlayer2;
    }

    PhotonNetwork.Instantiate(ballPrefab.name, position, transform.rotation, 0);
  }
}
