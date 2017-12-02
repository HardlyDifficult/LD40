using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
  [SerializeField]
  float raycastDistance = 1;

  [SerializeField]
  PowerMeter powerMeter;

  [SerializeField]
  ArcMeter arcMeter;

  [SerializeField]
  float strength = 100;

  [SerializeField]
  float maxY = 50;

  Rigidbody body;

  float? whenBallWasReleased;

  [SerializeField]
  float timeTillBallReset = 3;

  Vector3 ballInitialPosition;

  protected void Awake()
  {
    ballInitialPosition = transform.position;
    body = GetComponent<Rigidbody>();
    body.useGravity = false;
  }

  protected void Update()
  {
    if (whenBallWasReleased != null)
    {
      if (Time.timeSinceLevelLoad - whenBallWasReleased > timeTillBallReset)
      {
        whenBallWasReleased = null;
        transform.position = ballInitialPosition;
        body.useGravity = false;
        body.velocity = Vector3.zero;
      }
      else
      {
        return;
      }
    }

    arcMeter.UpdateArc();

    MoveBallLeftAndRight();

    ConsiderThrowing();
  }

  void ConsiderThrowing()
  {
    if (Input.GetMouseButtonUp(0))
    { // Throw
      float power = powerMeter.currentValue;
      // TODO launch

      Vector3 direction = transform.position;
      direction.y = maxY * arcMeter.currentValue;
      direction.z = 10;
      body.AddForce(direction * power * strength);
      body.useGravity = true;

      whenBallWasReleased = Time.timeSinceLevelLoad;
      powerMeter.Reset();
    }
    else if (Input.GetMouseButton(0))
    { // Power
      powerMeter.Charge();
    }
  }

  void MoveBallLeftAndRight()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    Plane plane = new Plane(Camera.main.transform.forward, raycastDistance);
    float enter;
    plane.Raycast(ray, out enter);
    Vector3 position = ray.GetPoint(enter);

    Vector3 myPosition = transform.position;
    myPosition.x = position.x;
    transform.position = myPosition;
  }
}
