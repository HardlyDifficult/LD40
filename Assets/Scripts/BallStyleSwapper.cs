using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStyleSwapper : MonoBehaviour
{
  protected void Awake()
  {
    PhotonView photonView = GetComponent<PhotonView>();
    if(photonView.isMine == false)
    {
      return;
    }

    ChangeBall.instance.onBallPreferenceChange += Instance_onBallPreferenceChange;
  }

  void Instance_onBallPreferenceChange()
  {
    GameObject newBall = Instantiate(ChangeBall.instance.currentBallPrefab, transform.position, transform.rotation);
    Rigidbody myBody = GetComponent<Rigidbody>();
    Rigidbody newBody = newBall.GetComponent<Rigidbody>();
    newBody.velocity = myBody.velocity;
    Destroy(gameObject);
  }
}
