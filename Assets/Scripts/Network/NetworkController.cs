using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
  public event Action onGameBegin;

  [SerializeField]
  GameObject showThisOnDisconnectBeforeReloadingTheScene;

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
    PhotonNetwork.player.NickName = GameManager.instance.name;
    PhotonNetwork.ConnectUsingSettings("0.7");
  }

  protected void OnDestroy()
  {
    PhotonNetwork.Disconnect();
  }

  protected void OnJoinedLobby()
  {
    PhotonNetwork.JoinRandomRoom(roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);
  }

  protected void OnPhotonRandomJoinFailed(object[] codeAndMsg)
  {
    print($"{nameof(OnPhotonRandomJoinFailed)} code {codeAndMsg[0]} message {codeAndMsg[1]}");

    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
  }

  void OnPhotonJoinRoomFailed()
  {
    Debug.LogError("Join failed");
  }

  protected void OnPhotonPlayerConnected(
    PhotonPlayer newPlayer)
  {
    Debug.Assert(PhotonNetwork.room.PlayerCount == 2);

    OnGameBegin();
  }

  protected void OnJoinedRoom()
  {
    print($"Welcome!  There are a total of {PhotonNetwork.room.PlayerCount} players in the room");

    if (PhotonNetwork.room.PlayerCount > 1)
    {
      OnGameBegin();
    }
  }

  void OnGameBegin()
  {
    Vector3 position = playerPrefab.transform.position;
    position.z = zPositionPlayer1;

    GameObject ball = PhotonNetwork.Instantiate(playerPrefab.name, position, transform.rotation, 0);
    PhotonView view = ball.GetComponent<PhotonView>();
    view.RequestOwnership();
    Debug.Assert(view.isMine);

    onGameBegin?.Invoke();
  }

  void OnLeftRoom()
  {
    print(nameof(OnLeftRoom));
  }

  void OnPhotonCreateRoomFailed(object[] codeAndMsg)
  { // codeAndMsg[0] is short ErrorCode. codeAndMsg[1] is string debug msg. }
    print(nameof(OnPhotonCreateRoomFailed) + " " + codeAndMsg[1]);
  }

  void OnPhotonJoinRoomFailed(object[] codeAndMsg)
  { // codeAndMsg[0] is short ErrorCode. codeAndMsg[1] is string debug msg. }
    print(nameof(OnPhotonJoinRoomFailed) + " " + codeAndMsg[1]);
  }

  void OnCreatedRoom()
  {
    print(nameof(OnCreatedRoom));
  }

  void OnLeftLobby()
  {
    print(nameof(OnLeftLobby));
  }

  void OnDisconnectedFromPhoton()
  {
    print(nameof(OnDisconnectedFromPhoton));
  }

  void OnConnectionFail(DisconnectCause cause)
  {
    print($"{nameof(OnConnectionFail)} cause: {cause}");
  }

  void OnFailedToConnectToPhoton(DisconnectCause cause)
  {
    print($"{nameof(OnFailedToConnectToPhoton)} cause: {cause}");
  }

  void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
  {
    print($"{nameof(OnPhotonPlayerDisconnected)}: {otherPlayer}");

    StartCoroutine(ReloadScene());
  }

  IEnumerator ReloadScene()
  {
    print("Reloading scene...");
    showThisOnDisconnectBeforeReloadingTheScene.SetActive(true);
    yield return new WaitForSeconds(3);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  void OnPhotonMaxCccuReached()
  {
    print(nameof(OnPhotonMaxCccuReached));
  }

  void OnCustomAuthenticationFailed(string debugMessage)
  {
    print(nameof(OnCustomAuthenticationFailed) + " " + debugMessage);
  }
}