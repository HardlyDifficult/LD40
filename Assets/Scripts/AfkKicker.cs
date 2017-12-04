using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfkKicker : MonoBehaviour
{
  [SerializeField]
  float timeBeforeWarning = 30;

  [SerializeField]
  float timeAfterWarning = 15;

  TurnController turnController;

  Coroutine routine;

  Player player;
  PhotonView photonView;

  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onTurnChange += TurnController_onTurnChange;
    turnController.onActionPointsChange += TurnController_onActionPointsChange;

    player = GetComponentInParent<Player>();
    photonView = player.GetComponent<PhotonView>();
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

    if (player.isMyTurn && photonView.isMine)
    {
      routine = StartCoroutine(TimeoutThenKick());
    }
  }

  IEnumerator TimeoutThenKick()
  {
    yield return new WaitForSeconds(timeBeforeWarning);
    print("You have been warned");
    AfkWarningMessage.instance.SetActive(true);
    yield return new WaitForSeconds(timeAfterWarning);

    SceneManager.LoadScene("Menu");
  }
}
