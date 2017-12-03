using UnityEngine;
using System;
using UnityEngine.UI;

public class ActionPointsRemaining : MonoBehaviour
{
  Text text;

  TurnController turnController;

  protected void Awake()
  {
    text = GetComponent<Text>();
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onActionPointsChange += TurnController_onActionPointsChange;
  }

  protected void Start()
  {
    NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
    networkController.onGameBegin += NetworkController_onGameBegin;
  }

  protected void OnDestroy()
  {
    NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
    networkController.onGameBegin -= NetworkController_onGameBegin;
  }

  void NetworkController_onGameBegin()
  {
    Refresh();
  }

  void TurnController_onActionPointsChange()
  {
    Refresh();
  }

  void Refresh()
  {
    text.text = turnController.numberOfActionsRemaining.ToString();
  }
}
