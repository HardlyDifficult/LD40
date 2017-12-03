using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnTurn : MonoBehaviour
{
  [SerializeField]
  bool onMyTurnSetActiveTo;

  int playerId;
  TurnController turnController;

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

  protected void Start()
  {
    Refresh();
  }

  void TurnController_onTurnChange()
  {
    Refresh();
  }

  void Refresh()
  {
    bool active = isMyTurn == onMyTurnSetActiveTo;
    gameObject.SetActive(active);
  }
}
