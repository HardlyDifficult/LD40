using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  public bool isPlayer0;

  TurnController turnController;

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
    NetworkController network = GameObject.FindObjectOfType<NetworkController>();
    network.onGameBegin += Network_onGameBegin;
  }

  protected void OnDestroy()
  {
    NetworkController network = GameObject.FindObjectOfType<NetworkController>();
    network.onGameBegin -= Network_onGameBegin;
  }

  void Network_onGameBegin()
  {
    if (PhotonNetwork.isMasterClient)
    {
      isPlayer0 = true;
    }
    else
    { // Player 2 goes to the other side of the table
      Vector3 position = transform.position;
      position.z = -position.z;
      transform.position = position;
    }
  }
}
