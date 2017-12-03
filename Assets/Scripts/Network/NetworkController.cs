using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
  public event Action onGameBegin;

  [SerializeField]
  float zPositionPlayer1;

  [SerializeField]
  float zPositionPlayer2;

  RoomOptions roomOptions;

  /// <summary>
  /// Must be in Resources
  /// </summary>
  [SerializeField]
  BallThrower playerPrefab;

  protected void Awake()
  {
    roomOptions = new RoomOptions()
    {
      MaxPlayers = 2
    };
    PhotonNetwork.ConnectUsingSettings("0.2");
  }

  protected void OnJoinedLobby()
  {
    PhotonNetwork.JoinRandomRoom(roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);
  }

  protected void OnPhotonRandomJoinFailed()
  {
    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
  }

  void OnPhotonJoinRoomFailed()
  {
    Debug.LogError("Join failed");
  }

  protected void OnPhotonPlayerConnected(
    PhotonPlayer newPlayer)
  {
    Debug.Assert(PhotonNetwork.playerList.Length == 2);

    onGameBegin?.Invoke();
  }

  protected void OnJoinedRoom()
  {
    PhotonPlayer[] playerList = PhotonNetwork.playerList;
    print($"Welcome!  There are a total of {playerList.Length} players in the room");

    Vector3 position = playerPrefab.transform.position;
    if (playerList.Length == 1)
    {
      //PhotonNetwork.player.SetCustomProperties(
      PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
      {
        {"PlayerId", 0}
      });
    }
    else
    {
      int otherPlayerId = (int)playerList[0].CustomProperties["PlayerId"];
      if (otherPlayerId == 0)
      {
        PhotonNetwork.player.CustomProperties.Add("PlayerId", 1);
      }
      else
      {
        PhotonNetwork.player.CustomProperties.Add("PlayerId", 0);
      }
    }

    if ((int)PhotonNetwork.player.CustomProperties["PlayerId"] == 0)
    {
      position.z = zPositionPlayer1;
      print("You are Player 0");
    }
    else
    {
      position.z = zPositionPlayer2;
      print("You are Player 1");
    }

    GameObject ball = PhotonNetwork.Instantiate(playerPrefab.name, position, transform.rotation, 0);
    PhotonView view = ball.GetComponent<PhotonView>();
    view.RequestOwnership();
    Debug.Assert(view.isMine);

    if (playerList.Length > 1)
    {
      onGameBegin?.Invoke();
    }
  }
}
