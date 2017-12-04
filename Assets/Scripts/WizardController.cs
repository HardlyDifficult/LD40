using System;
using UnityEngine;

public class WizardController : MonoBehaviour
{
  const string startAim = "StartAim",
    throwWithMore = "ThrowWithMoreToCome",
    lastThrow = "LastThrow";

  [SerializeField]
  bool isPlayer0;

  Animator animator;

  TurnController turnController;

  NetworkController networkController;

  Player player;

  protected void Awake()
  {
    animator = GetComponent<Animator>();
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onTurnChange += TurnController_onTurnChange;

    networkController = GameObject.FindObjectOfType<NetworkController>();
    networkController.onGameBegin += NetworkController_onGameBegin;

    if(isPlayer0)
    {
      Wizards.wiz1 = this;
    } else
    {
      Wizards.wiz2 = this;
    }
  }

  void NetworkController_onGameBegin()
  {
    player = isPlayer0 ? Player.player1 : Player.player2;
    TurnController_onTurnChange();
  }
  
  protected void OnDestroy()
  {
    turnController.onTurnChange -= TurnController_onTurnChange;
  }

  void TurnController_onTurnChange()
  {
    if (player.isMyTurn)
    {
      animator.Play(startAim);
    }
  }

  public void Shoot()
  {
    animator.Play(turnController.numberOfActionsRemaining > 1 ? throwWithMore : lastThrow);
  }
}
