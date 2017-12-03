using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  public bool isPlayer0;

  TurnController turnController;

  PhotonView photonView;

  public bool isMyTurn
  {
    get
    {
      return turnController.isCurrentlyPlayer0sTurn == isPlayer0;
    }
  }

  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    photonView = GetComponent<PhotonView>();
    NetworkController network = GameObject.FindObjectOfType<NetworkController>();
    network.onGameBegin += Network_onGameBegin;
  }

  protected void OnDestroy()
  {
    NetworkController network = GameObject.FindObjectOfType<NetworkController>();
    if (network != null)
    {
      network.onGameBegin -= Network_onGameBegin;
    }
  }

  void Network_onGameBegin()
  {
    if (PhotonNetwork.isMasterClient)
    {
      isPlayer0 = photonView.isMine;
    }
    else
    {
      isPlayer0 = !photonView.isMine;
    }

    if (isPlayer0 == false && photonView.isMine)
    { // Player 2 goes to the other side of the table
      Vector3 position = transform.position;
      position.z = -position.z;
      transform.position = position;
    }
  }
}
