using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
  [SerializeField]
  float raycastDistance = 1;

  [SerializeField]
  float strength = 100;

  [SerializeField]
  float maxY = 50;

  [SerializeField]
  float wandPowerTranslation = 0.1f;

  [SerializeField]
  float wandPowerRotation = 50;

  [SerializeField]
  float wandArcTranslation = 1;

  [SerializeField]
  float wandArcRotation = 10;

  [SerializeField]
  GameObject wandRotationTarget;

  Rigidbody ballBody;

  [SerializeField]
  GameObject wand;

  float? whenBallWasReleased;

  [SerializeField]
  float timeTillBallReset = 6;

  [SerializeField]
  float resetWhenBelowY = -10;

  Vector3 ballInitialPosition;

  PhotonView photonView;

  ParticleSystem ballParticleSystem;

  protected void Awake()
  {
    photonView = GetComponent<PhotonView>();

    if (photonView.isMine)
    {
      GameObject ball = PhotonNetwork.Instantiate(GameManager.instance.currentBallPrefab.name, transform.position, transform.rotation, 0);
      PhotonView ballsView = ball.GetComponent<PhotonView>();
      ballsView.RequestOwnership();
      ball.GetComponent<Rigidbody>().useGravity = false;
      ballBody = ball.GetComponent<Rigidbody>();
      ballParticleSystem = ball.GetComponent<ParticleSystem>();
    }


    if (wand != null)
    {
      //ballBody.gameObject.SetActive(false);
    }
  }

  protected void Start()
  {
    ballInitialPosition = ballBody?.transform.position ?? Vector3.zero;
  }

  protected void Update()
  {
    if (photonView.isMine == false)
    {
      return;
    }

    if (whenBallWasReleased != null)
    {
      if (Time.timeSinceLevelLoad - whenBallWasReleased > timeTillBallReset
        || Input.GetMouseButtonDown(1)
        || ballBody.transform.position.y <= resetWhenBelowY)
      {
        whenBallWasReleased = null;
        ballBody.transform.position = ballInitialPosition;
        ballBody.useGravity = false;
        ballBody.velocity = Vector3.zero;
        if (wand != null)
        {
          //ballBody.gameObject.SetActive(false);
          if (ballParticleSystem)
          {
            ballParticleSystem.Simulate(1, true, true);
            ballParticleSystem.Play();
          }
        }
        UIController.instance.EnableThrowUI();
      }
    }

    if (whenBallWasReleased == null)
    {
      UIController.instance.arcMeter.UpdateArc();
    }

    MoveBallLeftAndRight();

    if (whenBallWasReleased == null)
    {
      ConsiderThrowing();
    }
  }

  void ConsiderThrowing()
  {
    if (Input.GetMouseButtonUp(0))
    { // Throw
      float power = UIController.instance.powerMeter.currentValue;
      float arc = UIController.instance.arcMeter.currentValue;

      // TODO launch

      ballBody?.gameObject.SetActive(true);
      Vector3 direction = ballBody.transform.position;
      direction.y = maxY * arc;
      direction.z = 10;
      ballBody.AddForce(direction * power * strength);
      ballBody.useGravity = true;

      whenBallWasReleased = Time.timeSinceLevelLoad;
      UIController.instance.powerMeter.Reset();
      UIController.instance.DisableThrowUI();
    }
    else if (Input.GetMouseButton(0))
    { // Power
      UIController.instance.powerMeter.Charge();
    }
  }

  void MoveBallLeftAndRight()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    Plane plane = new Plane(Camera.main.transform.forward, raycastDistance);
    float enter;
    plane.Raycast(ray, out enter);
    Vector3 position = ray.GetPoint(enter);

    float power = UIController.instance.powerMeter.currentValue;
    float arc = UIController.instance.arcMeter.currentValue;

    Vector3 myPosition = wand ? wand.transform.position : ballBody.transform.position;
    myPosition.x = position.x;
    myPosition.z = ballInitialPosition.z - wandPowerTranslation * power - wandArcTranslation * arc;
    if (whenBallWasReleased == null
      && ballBody != null)
    {
      ballBody.transform.position = myPosition;
    }
    if (wand != null)
    {
      wand.transform.position = myPosition;
      Vector3 target = wandRotationTarget ? wandRotationTarget.transform.position : myPosition + new Vector3(0, -1, 0);
      target = Quaternion.AngleAxis(power * wandPowerRotation + arc * wandArcRotation, Vector3.left) * target;
      wand.transform.LookAt(target);
    }
  }
}
