using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStyleSwapper : MonoBehaviour
{
  protected void Awake()
  {
    PhotonView photonView = GetComponent<PhotonView>();
    if (photonView.isMine == false)
    {
      return;
    }

    ChangeBall.instance.onBallPreferenceChange += Instance_onBallPreferenceChange;
  }

  protected void OnDestroy()
  {
    if (ChangeBall.instance != null)
    {
      ChangeBall.instance.onBallPreferenceChange -= Instance_onBallPreferenceChange;
    }
  }

  protected void Start()
  {
    GetComponentInParent<BallThrower>().RegisterBall(gameObject);
  }

  void Instance_onBallPreferenceChange()
  {
    GameObject newBall = Instantiate(ChangeBall.instance.currentBallPrefab, transform.position, transform.rotation);
    newBall.transform.SetParent(transform.parent);
    Rigidbody myBody = GetComponent<Rigidbody>();
    Rigidbody newBody = newBall.GetComponent<Rigidbody>();
    newBody.velocity = myBody.velocity;
    newBody.useGravity = myBody.useGravity;
    Destroy(gameObject);
  }
}
