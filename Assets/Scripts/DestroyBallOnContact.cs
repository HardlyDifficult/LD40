using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBallOnContact : MonoBehaviour
{
  const int playerLayer = 8;

  [SerializeField]
  float howLongTillDestroy = 2;

  protected void OnCollisionEnter(
    Collision collision)
  {
    if(collision.gameObject.layer != playerLayer)
    {
      return;
    }

    StartCoroutine(DestroyBall());
  }

  protected void OnCollisionExit(
    Collision collision)
  {
    StopAllCoroutines();
  }

  IEnumerator DestroyBall()
  {
    yield return new WaitForSeconds(howLongTillDestroy);
    Destroy(gameObject);
  }
}
