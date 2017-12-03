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

  Player player;

  protected void Awake()
  {
    animator = GetComponent<Animator>();
    player = GetComponentInParent<Player>();
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
    animator.Play(player.isPlayer0 ? player1Animation : player2Animation);
  }
}
