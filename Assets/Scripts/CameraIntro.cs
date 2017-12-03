using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntro : MonoBehaviour
{
  [SerializeField]
  string player1Animation;

  [SerializeField]
  string player2Animation;

  Animator animator;

  NetworkController networkController;

  protected void Awake()
  {
    networkController = GameObject.FindObjectOfType<NetworkController>();
    networkController.onGameBegin += OnGameBegin;
    animator = GetComponent<Animator>();
  }

  protected void OnDestroy()
  {
    networkController.onGameBegin -= OnGameBegin;
  }

  void OnGameBegin()
  {
    int playerId = (int)PhotonNetwork.player.CustomProperties["PlayerId"];
    animator.Play(playerId == 0 ? player1Animation : player2Animation);
  }
}
