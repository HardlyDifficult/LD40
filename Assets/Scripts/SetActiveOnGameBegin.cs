using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnGameBegin : MonoBehaviour
{
  [SerializeField]
  bool setActiveTo = false;

  NetworkController networkController;

  protected void Awake()
  {
    networkController = GameObject.FindObjectOfType<NetworkController>();
    networkController.onGameBegin += NetworkController_onGameBegin;

    if(setActiveTo)
    {
      gameObject.SetActive(false);
    }
  }

  void NetworkController_onGameBegin()
  {
    gameObject.SetActive(setActiveTo);
  }

  protected void OnDestroy()
  {
    networkController.onGameBegin -= NetworkController_onGameBegin;
  }
}
