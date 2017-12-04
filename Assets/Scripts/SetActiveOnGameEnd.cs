using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnGameEnd: MonoBehaviour
{
  public enum When
  {
    Always, IWin, ILose
  }

  [SerializeField]
  bool setActiveTo = false;

  [SerializeField]
  When when;

  TurnController turnController;

  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onWin += TurnController_onWin;

    if(setActiveTo)
    {
      gameObject.SetActive(false);
    }
  }

  void TurnController_onWin(
    Player winningPlayer)
  {
    switch (when)
    {
      case When.Always:
        break;
      case When.IWin:
        if(Player.localPlayer != winningPlayer)
        {
          return;
        }
        break;
      case When.ILose:
        if (Player.localPlayer == winningPlayer)
        {
          return;
        }
        break;
      default:
        break;
    }

    gameObject.SetActive(setActiveTo);
  }

  protected void OnDestroy()
  {
    turnController.onWin -= TurnController_onWin;
  }
}
