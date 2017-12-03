using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
  public delegate void OnPlayerSpawn(PhotonView photonView);
  public static event OnPlayerSpawn onPlayerSpawn;

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

  int playerId;

  ParticleSystem ballParticleSystem;

  TurnController turnController;

  public bool isMyTurn
  {
    get
    {
      bool isPlayer0 = playerId == 0;
      bool myTurn = turnController.isCurrentlyPlayer0sTurn == isPlayer0;
      return myTurn;
    }
  }

  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
    photonView = GetComponent<PhotonView>();
    playerId = (int)PhotonNetwork.player.CustomProperties["PlayerId"];

    if (photonView.isMine)
    {
      GameObject ball = PhotonNetwork.Instantiate(GameManager.instance.currentBallPrefab.name, transform.position, transform.rotation, 0);
      PhotonView ballsView = ball.GetComponent<PhotonView>();
      ballsView.RequestOwnership();
      ball.GetComponent<Rigidbody>().useGravity = false;
      ballBody = ball.GetComponent<Rigidbody>();
      ballParticleSystem = ball.GetComponent<ParticleSystem>();
    }
    onPlayerSpawn?.Invoke(photonView);

    if (wand != null)
    {
      //ballBody.gameObject.SetActive(false);
    }
  }

  protected void Start()
  {
    ballInitialPosition = wand ? wand.transform.position : ballBody.transform.position;
  }

  protected void Update()
  {
    if (photonView.isMine == false
      || isMyTurn == false)
    {
      return;
    }
    ConsiderReloading();

    if (whenBallWasReleased == null)
    {
      UIController.instance.arcMeter.UpdateArc();
    }

    MoveBallLeftAndRight();

    ConsiderThrowing();
  }

  void MoveBallLeftAndRight()
  {
    float power = UIController.instance.powerMeter.currentValue;
    float arc = UIController.instance.arcMeter.currentValue;
    float xPosition;
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Plane plane = new Plane(Camera.main.transform.forward, raycastDistance);
      float enter;
      plane.Raycast(ray, out enter);
      xPosition = ray.GetPoint(enter).x;
    }

    Vector3 myPosition = wand ? wand.transform.position : ballBody.transform.position;
    myPosition.x = xPosition;
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
      target = Quaternion.AngleAxis(power * wandPowerRotation + arc * wandArcRotation, Vector3.right) * target;
      wand.transform.LookAt(target);
    }
  }

  void ConsiderThrowing()
  {
    if (whenBallWasReleased != null)
    {
      return;
    }

    if (Input.GetMouseButtonUp(0))
    { // Throw
      float power = UIController.instance.powerMeter.currentValue;
      float arc = UIController.instance.arcMeter.currentValue;

      ballBody?.gameObject.SetActive(true);
      Vector3 direction = ballBody.transform.position;
      direction.y = maxY * arc;
      direction.z = -direction.z;
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

  void ConsiderReloading()
  {
    if (whenBallWasReleased == null)
    {
      return;
    }

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
}
