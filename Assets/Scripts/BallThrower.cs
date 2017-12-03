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

  [SerializeField]
  LineRenderer[] trajectoryLines;
  [SerializeField]
  MeshFilter trajectoryPlane;
  [SerializeField]
  Vector2 trajectoryPowerRange = new Vector2(0.0f, 1.0f);

  float? whenBallWasReleased;

  [SerializeField]
  float timeTillBallReset = 6;

  [SerializeField]
  float resetWhenBelowY = -10;

  Vector3 ballInitialPosition;

  PhotonView photonView;

  GameObject ball;

  GameObject ballVisuals;

  ParticleSystem ballParticleSystem;

  TurnController turnController;

  Player player;
  
  protected void Awake()
  {
    player = GetComponentInParent<Player>();
    turnController = GameObject.FindObjectOfType<TurnController>();
    photonView = GetComponent<PhotonView>();

    if (photonView.isMine)
    {
      ball = PhotonNetwork.Instantiate(GameManager.instance.currentBallPrefab.name, transform.position, transform.rotation, 0);
      PhotonView ballsView = ball.GetComponent<PhotonView>();
      ballsView.RequestOwnership();
      ball.GetComponent<Rigidbody>().useGravity = false;
      ballBody = ball.GetComponent<Rigidbody>();
      ballParticleSystem = ball.GetComponent<ParticleSystem>();
      ballVisuals = ball.transform.GetChild(0).gameObject;
      ballVisuals.SetActive(false);
    }
    onPlayerSpawn?.Invoke(photonView);

    if (wand != null)
    {
      //ballBody.gameObject.SetActive(false);
    }
  }

  protected void Start()
  {
    if (photonView.isMine)
    {
      if (wand != null)
      {
        ballBody.transform.position = wand.transform.position;
      }
    }

    ballInitialPosition = wand ? wand.transform.position : ballBody.transform.position;
  }

  protected void Update()
  {
    if (photonView.isMine == false
      || player.isMyTurn == false)
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
      target = Quaternion.AngleAxis(power * wandPowerRotation + arc * wandArcRotation, Vector3.left) * target;
      wand.transform.LookAt(target);
    }

    UpdateTrajectoryLines();
  }

  const int TrajectoryLinePointsCount = 100;
  Vector3[] TrajectoryLinePoints = new Vector3[TrajectoryLinePointsCount];
  Vector3[] TrajectoryPlanePoints;
  private void UpdateTrajectoryLines()
  {
    if (whenBallWasReleased != null)
    {
      for (int i = 0; i < trajectoryLines.Length; i++)
      {
        trajectoryLines[i]?.gameObject.SetActive(false);
      }
      trajectoryPlane?.gameObject.SetActive(false);
      return;
    }
    // init plane mesh topology
    Mesh plane;
    bool updatePlaneTriangles = false;
    if (TrajectoryPlanePoints == null || TrajectoryPlanePoints.Length != trajectoryLines.Length * TrajectoryLinePointsCount)
    {
      TrajectoryPlanePoints = new Vector3[trajectoryLines.Length * TrajectoryLinePointsCount];
      updatePlaneTriangles = true;
      plane = trajectoryPlane.mesh = new Mesh();
    }
    else
    {
      plane = trajectoryPlane.mesh;
    }
    // parameters for the simple physics sim
    float mass = ballBody.mass;
    float dt = Time.fixedDeltaTime;
    Vector3 gravity = Physics.gravity;
    const float lineTime = 3.0f;
    Vector3[] linePoints = TrajectoryLinePoints;
    const float lineDt = lineTime / (TrajectoryLinePointsCount - 1);
    float radius = ballBody.GetComponent<SphereCollider>()?.radius ?? 1.0f;
    const int layerMask = ~(1 << 8);
    for (int li = 0; li < trajectoryLines.Length; li++)
    {
      float power = UIController.instance.powerMeter.currentValue;
      float arc = UIController.instance.arcMeter.currentValue;
      float powerLine = trajectoryPowerRange.x + (trajectoryPowerRange.y - trajectoryPowerRange.x) * li / Math.Max(1, trajectoryLines.Length - 1);
      power = Math.Max(power, powerLine);
      Vector3 position = ballBody.transform.position;
      position.z = ballInitialPosition.z - wandPowerTranslation * power - wandArcTranslation * arc;
      Vector3 velocity = Vector3.zero;
      Vector3 direction = position;
      direction.y = maxY * arc;
      direction.z = 10;
      Vector3 dv = direction * (power * strength * dt / mass);
      velocity += dv;
      dv = gravity * (lineDt / mass);
      int i = 0;
      while (i < TrajectoryLinePointsCount - 1)
      {
        linePoints[i++] = position;
        velocity += dv;
        RaycastHit hitInfo;
        if (velocity.y < 0.0f && Physics.SphereCast(position, radius, velocity, out hitInfo, velocity.magnitude * lineDt, layerMask))
        {
          position += velocity * ((radius + hitInfo.distance) / velocity.magnitude);
          break;
        }
        else
        {
          position += velocity * lineDt;
        }
      }
      while (i < TrajectoryLinePointsCount)
      {
        linePoints[i++] = position;
      }
      LineRenderer line = trajectoryLines[li];
      if (line)
      {
        line.gameObject.SetActive(true);
        line.useWorldSpace = true;
        if (line.positionCount != TrajectoryLinePointsCount)
          line.positionCount = TrajectoryLinePointsCount;
        line.SetPositions(linePoints);
      }
      for (i = 0; i < TrajectoryLinePointsCount; i++)
      {
        TrajectoryPlanePoints[li * TrajectoryLinePointsCount + i] = linePoints[i];
      }
    }
    plane.vertices = TrajectoryPlanePoints;
    if (updatePlaneTriangles)
    {
      int[] planeFaces = new int[(trajectoryLines.Length - 1) * (TrajectoryLinePointsCount - 1) * 6];
      int ind = 0;
      for (int li = 0; li < trajectoryLines.Length - 1; li++)
      {
        for (int i = 0; i < TrajectoryLinePointsCount - 1; i++)
        {
          int v0 = li * TrajectoryLinePointsCount + i;
          int v1 = v0 + 1;
          int v2 = v0 + TrajectoryLinePointsCount + 1;
          int v3 = v0 + TrajectoryLinePointsCount;
          planeFaces[ind++] = v0;
          planeFaces[ind++] = v1;
          planeFaces[ind++] = v2;
          planeFaces[ind++] = v0;
          planeFaces[ind++] = v2;
          planeFaces[ind++] = v3;
        }
      }
      plane.triangles = planeFaces;
    }
    // make sure the vertices positions are in global space
    trajectoryPlane.transform.position = Vector3.zero;
    trajectoryPlane.transform.rotation = Quaternion.identity;
    trajectoryPlane.transform.localScale = Vector3.one;
    trajectoryPlane.gameObject.SetActive(true);
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

      Vector3 direction = ballBody.transform.position;
      direction.y = maxY * arc;
      direction.z = -direction.z;
      ballBody.AddForce(direction * power * strength);
      ballBody.useGravity = true;

      whenBallWasReleased = Time.timeSinceLevelLoad;
      UIController.instance.powerMeter.Reset();
      UIController.instance.DisableThrowUI();
      turnController.numberOfActionsRemaining--;
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
