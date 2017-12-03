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

  protected void Awake()
  {
    BallThrower.onPlayerSpawn += BallThrower_onPlayerSpawn;
    animator = GetComponent<Animator>();
  }

  protected void OnDestroy()
  {
    BallThrower.onPlayerSpawn -= BallThrower_onPlayerSpawn;
  }

  void BallThrower_onPlayerSpawn(
    PhotonView photonView)
  {
    if (photonView.isMine == false)
    {
      return;
    }

    int playerId = (int)PhotonNetwork.player.CustomProperties["PlayerId"];
    animator.Play(playerId == 0 ? player1Animation : player2Animation);
  }
}
