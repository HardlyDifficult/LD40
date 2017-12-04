using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
  TurnController turnController;

  [HideInInspector]
  public GameObject visuals;

  /// <summary>
  /// Set on spawn
  /// </summary>
  Player player;

  protected void Start()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    player = PhotonNetwork.isMasterClient ? turnController.player1 : turnController.player2;

    visuals = transform.GetChild(player.isPlayer0 ? 0 : 1).gameObject;
    visuals.SetActive(true);
    visuals.transform.position = new Vector3(1000, 1000, 10009);
    turnController.onTurnChange += TurnController_onTurnChange;
    //TurnController_onTurnChange();
  }

  protected void OnDestroy()
  {
    turnController.onTurnChange -= TurnController_onTurnChange;
  }

  void TurnController_onTurnChange()
  {
    visuals.transform.position = Vector3.zero;
    //visuals.SetActive(player.isMyTurn);
  }
}
