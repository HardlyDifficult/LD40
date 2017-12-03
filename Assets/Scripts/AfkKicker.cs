using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfkKicker : MonoBehaviour
{
  TurnController turnController;

  Coroutine routine;

  Player player;

  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onTurnChange += TurnController_onTurnChange;
    turnController.onActionPointsChange += TurnController_onActionPointsChange;

    player = GetComponentInParent<Player>();
  }

  protected void OnDestroy()
  {
    turnController.onTurnChange -= TurnController_onTurnChange;
  }

  void TurnController_onActionPointsChange()
  {
    Reset();
  }

  void TurnController_onTurnChange()
  {
    Reset();
  }

  void Reset()
  {
    if (routine != null)
    {
      AfkWarningMessage.instance.SetActive(false);
      StopCoroutine(routine);
    }

    if (player.isMyTurn)
    {
      routine = StartCoroutine(TimeoutThenKick());
    }
  }

  IEnumerator TimeoutThenKick()
  {
    yield return new WaitForSeconds(10);
    AfkWarningMessage.instance.SetActive(true);
    yield return new WaitForSeconds(3);

    SceneManager.LoadScene("Menu");
  }
}
