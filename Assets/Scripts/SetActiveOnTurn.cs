using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnTurn : MonoBehaviour
{
  [SerializeField]
  bool onMyTurnSetActiveTo;
  
  TurnController turnController;

  protected void Awake()
  {
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
    bool active = (PhotonNetwork.isMasterClient == turnController.isCurrentlyFirstPlayersTurn) == onMyTurnSetActiveTo;
    gameObject.SetActive(active);
  }
}
