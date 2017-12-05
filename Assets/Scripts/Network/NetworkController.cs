using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
  #region Data
  public event Action onGameBegin;

  [SerializeField]
  GameObject showThisOnDisconnectBeforeReloadingTheScene;

  /// <summary>
  /// Player 2 will use this negated
  /// </summary>
  [SerializeField]
  float zPositionPlayer;

  /// <summary>
  /// Must be in Resources
  /// </summary>
  [SerializeField]
  BallThrower playerPrefab;

  /// <summary>
  /// Cached
  /// </summary>
  RoomOptions roomOptions;
  #endregion

  #region Init
  protected void Awake()
  {
    roomOptions = new RoomOptions()
    {
      MaxPlayers = 2
    };
    PhotonNetwork.player.NickName = GameManager.instance.name;
    PhotonNetwork.ConnectUsingSettings("0.13");
  }

  protected void OnDestroy()
  {
    PhotonNetwork.Disconnect();
  }
  #endregion

  #region Events
  /// <summary>
  /// Step 1
  /// </summary>
  protected void OnJoinedLobby()
  {
    PhotonNetwork.JoinRandomRoom(roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);
  }

  /// <summary>
  /// Step 2 success
  /// </summary>
  protected void OnJoinedRoom()
  {
    print($"Welcome!  There are a total of {PhotonNetwork.room.PlayerCount} players in the room");

    if (PhotonNetwork.room.PlayerCount > 1)
    {
      OnGameBegin();
    }
  }

  /// <summary>
  /// Step 2 fail path.
  /// There are no rooms with a spot open
  /// </summary>
  protected void OnPhotonRandomJoinFailed(
    object[] codeAndMsg)
  {
    print($"{nameof(OnPhotonRandomJoinFailed)} code {codeAndMsg[0]} message {codeAndMsg[1]}");

    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
  }

  /// <summary>
  /// Step 3: Someone joined our room.
  /// 
  /// This should always be a second player and trigger the start of a game.
  /// </summary>
  protected void OnPhotonPlayerConnected(
    PhotonPlayer newPlayer)
  {
    Debug.Assert(PhotonNetwork.room.PlayerCount == 2);

    OnGameBegin();
  }

  /// <summary>
  /// Your opponent rage quit.  Start a new game.
  /// </summary>
  /// <param name="otherPlayer"></param>
  void OnPhotonPlayerDisconnected(
    PhotonPlayer otherPlayer)
  {
    print($"{nameof(OnPhotonPlayerDisconnected)}: {otherPlayer}");

    StartCoroutine(ReloadScene());
  }
  #endregion

  #region Private
  /// <summary>
  /// Each player creates their ball and the gameBegin event is fired.
  /// </summary>
  void OnGameBegin()
  {
    Vector3 position = playerPrefab.transform.position;
    position.z = zPositionPlayer;

    GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, position, transform.rotation, 0);
    PhotonView view = player.GetComponent<PhotonView>();
    view.RequestOwnership();
    Debug.Assert(view.isMine);


    StartCoroutine(StartGame());
  }

  private IEnumerator StartGame()
  {
    yield return new WaitForSeconds(1);
    onGameBegin?.Invoke();
  }

  /// <summary>
  /// Show a message for 3 seconds and then reload.
  /// </summary>
  /// <returns></returns>
  IEnumerator ReloadScene()
  {
    print("Reloading scene...");
    showThisOnDisconnectBeforeReloadingTheScene.SetActive(true);
    yield return new WaitForSeconds(3);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  #endregion

  #region For Debugging
  void OnPhotonJoinRoomFailed()
  {
    Debug.LogError("Join failed");
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

  void OnConnectionFail(
    DisconnectCause cause)
  {
    print($"{nameof(OnConnectionFail)} cause: {cause}");
  }

  void OnFailedToConnectToPhoton(
    DisconnectCause cause)
  {
    print($"{nameof(OnFailedToConnectToPhoton)} cause: {cause}");
  }

  void OnPhotonMaxCccuReached()
  {
    print(nameof(OnPhotonMaxCccuReached));
  }

  void OnCustomAuthenticationFailed(
    string debugMessage)
  {
    print(nameof(OnCustomAuthenticationFailed) + " " + debugMessage);
  }
  #endregion
}