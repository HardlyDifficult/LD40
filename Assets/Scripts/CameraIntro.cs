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
    animator = GetComponent<Animator>();
  }

  protected void Start()
  {
    networkController = GameObject.FindObjectOfType<NetworkController>();
    networkController.onGameBegin += OnGameBegin;
  }

  protected void OnDestroy()
  {
    networkController.onGameBegin -= OnGameBegin;
  }

  void OnGameBegin()
  {
    animator.Play(PhotonNetwork.isMasterClient ? player1Animation : player2Animation);
  }
}
