using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
  TurnController turnController;
  int playerId;

  public bool isMyTurn
  {
    get
    {
      bool isPlayer0 = playerId == 0;
      bool myTurn = turnController.isCurrentlyPlayer0sTurn == isPlayer0;
      return myTurn;
    }
  }

  protected void Awake()
  {
    playerId = (int)PhotonNetwork.player.CustomProperties["PlayerId"];
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onTurnChange += TurnController_onTurnChange;
  }

  protected void OnDestroy()
  {
    turnController.onTurnChange -= TurnController_onTurnChange;
  }

  void TurnController_onTurnChange()
  {
    gameObject.SetActive(isMyTurn);
  }
}
