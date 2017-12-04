using System;
using UnityEngine;

public class MirrorPositionForPlayer2 : MonoBehaviour
{
  NetworkController networkController;

  protected void Start()
  {
    if(Player.localPlayer.isFirstPlayer)
    {
      return;
    }

    Vector3 position = transform.position;
    position.x = -position.x;
    position.z = -position.z;
    transform.position = position;
    transform.rotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
  }
}
