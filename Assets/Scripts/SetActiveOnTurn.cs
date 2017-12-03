using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnTurn : MonoBehaviour
{
  [SerializeField]
  bool onMyTurnSetActiveTo;
  
  TurnController turnController;

  Player player;
  
  protected void Awake()
  {
    player = GetComponentInParent<Player>();

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
    bool active = player.isMyTurn == onMyTurnSetActiveTo;
    gameObject.SetActive(active);
  }
}
