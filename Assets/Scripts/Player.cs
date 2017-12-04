using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  #region Data
  public static Player player1, player2;

  TurnController turnController;

  PhotonView photonView;

  /// <summary>
  /// Cached.
  /// </summary>
  NetworkController network;
  #endregion

  #region Property
  public static bool isPlayer1
  {
    get
    {
      return PhotonNetwork.isMasterClient;
    }
  }

  public static Player localPlayer
  {
    get
    {
      if (PhotonNetwork.isMasterClient)
      {
        return player1;
      }
      else
      {
        return player2;
      }
    }
  }

  public static Player remotePlayer
  {
    get
    {
      if (PhotonNetwork.isMasterClient)
      {
        return player2;
      }
      else
      {
        return player1;
      }
    }
  }

  public bool isFirstPlayer
  {
    get
    {
      if (PhotonNetwork.isMasterClient)
      {
        return photonView.isMine;
      }
      else
      {
        return photonView.isMine == false;
      }
    }
  }

  public bool isMyTurn
  {
    get
    {
      return turnController.isCurrentlyFirstPlayersTurn == isFirstPlayer;
    }
  }
  #endregion

  #region Init
  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    photonView = GetComponent<PhotonView>();

    if (isFirstPlayer)
    {
      player1 = this;
    }
    else
    {
      player2 = this;
    }

    network = GameObject.FindObjectOfType<NetworkController>();
    network.onGameBegin += Network_onGameBegin;
  }

  protected void OnDestroy()
  {
    network = GameObject.FindObjectOfType<NetworkController>();
    if (network != null)
    {
      network.onGameBegin -= Network_onGameBegin;
    }
  }
  #endregion

  #region Events
  void Network_onGameBegin()
  {
    if (isFirstPlayer == false && photonView.isMine)
    { // Player 2 goes to the other side of the table
      Vector3 position = transform.position;
      position.z = -position.z;
      transform.position = position;
    }

    Wizards wizards = GameObject.FindObjectOfType<Wizards>();
    transform.SetParent((isFirstPlayer ? wizards.player1WandBone : wizards.player2WandBone).transform);
    transform.localPosition = Vector3.zero;
  }
  #endregion
}
