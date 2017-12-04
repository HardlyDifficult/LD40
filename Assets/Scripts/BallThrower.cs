using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
  #region Data
  [SerializeField]
  float throwStrength;

  [Header("Screen positions")]
  [SerializeField]
  float minX;

  [SerializeField]
  float maxX;

  [SerializeField]
  float minY;

  [SerializeField]
  float maxY;

  /// <summary>
  /// Used to reject small (e.g. accidental) movements / launches.
  /// </summary>
  [SerializeField]
  float minThrowMag = .05f;

  [SerializeField]
  float minTimeTillReset = 1;

  Ball ball;

  bool holdingBall;

  // TODO
  [SerializeField]
  GameObject wand;

  float? whenBallWasReleased;

  [SerializeField]
  float timeTillBallReset = 6;

  PhotonView photonView;

  TurnController turnController;

  Player player;

  Coroutine shotRoutine;

  /// <summary>
  /// Min y.
  /// X we had when min y was found.
  /// z is ignored.
  /// </summary>
  Vector3? minPositionOfThrow;
  #endregion

  #region Init
  protected void Awake()
  {
    player = GetComponentInParent<Player>();
    turnController = GameObject.FindObjectOfType<TurnController>();
    turnController.onTurnChange += TurnController_onTurnChange;
    photonView = GetComponent<PhotonView>();

    if (photonView.isMine)
    {
      ball = PhotonNetwork.Instantiate(GameManager.instance.currentBallPrefab.name, transform.position, transform.rotation, 0).GetComponent<Ball>();
      ball.player = player;
      PhotonView ballsView = ball.GetComponent<PhotonView>();
      ballsView.RequestOwnership();
    }
  }

  protected void OnDestroy()
  {
    turnController.onTurnChange -= TurnController_onTurnChange;
  }
  #endregion

  #region Events
  protected void Update()
  {
    if (photonView.isMine == false
      || player.isMyTurn == false
      || shotRoutine != null)
    { // Busy
      return;
    }

    ConsiderReload();

    if (holdingBall)
    {
      Aim();
      Shoot();
    }
  }

  void TurnController_onTurnChange()
  {
    if (player.isMyTurn && photonView.isMine)
    {
      Reload();
    }
  }
  #endregion

  #region Private Write
  void ConsiderReload()
  {
    if (holdingBall
      || Time.timeSinceLevelLoad - whenBallWasReleased < minTimeTillReset)
    {
      return;
    }

    if (Input.GetMouseButton(0)
      || Time.timeSinceLevelLoad - whenBallWasReleased > timeTillBallReset
      || Input.GetMouseButtonDown(1))
    {
      Reload();
    }
  }

  void Reload()
  {
    holdingBall = true;
    whenBallWasReleased = null;
    ball.ShowBall();
    Cursor.visible = false;
  }

  void Aim()
  {
    Debug.Assert(holdingBall);

    SetBallPosition(Input.mousePosition);

    if (minPositionOfThrow == null || minPositionOfThrow.Value.y >= ball.transform.position.y)
    {
      minPositionOfThrow = ball.transform.position;
    }
  }

  void Shoot()
  {
    Debug.Assert(holdingBall);

    if (Input.GetMouseButtonUp(0) == false)
    {
      return;
    }

    Vector3 direction;
    if (GetThrowMagnitude(out direction))
    {
      ThrowBall(direction);
    }

    minPositionOfThrow = null;
  }

  public static float Sigmoid(double value) { return 1.0f / (1.0f + (float)Math.Exp(-value)); }

  void ThrowBall(
    Vector3 direction)
  {
    float mag = direction.magnitude;
    mag = Sigmoid(mag);
    print(mag);
    mag = Mathf.Clamp(mag, .25f, 1);

    direction += transform.forward * mag;
    direction = (direction + transform.forward * direction.magnitude) * throwStrength;

    if (player.isFirstPlayer == false)
    {
      direction.z = -direction.z;
    }

    Debug.DrawRay(ball.transform.position, direction);

    ball.Throw(direction);

    whenBallWasReleased = Time.timeSinceLevelLoad;
    holdingBall = false;
    Cursor.visible = true;
    shotRoutine = StartCoroutine(Shot());
  }

  IEnumerator Shot()
  {
    yield return new WaitForSeconds(3);
    turnController.numberOfActionsRemaining--;
    shotRoutine = null;
  }

  void SetBallPosition(
    Vector2 mousePosition)
  {
    float maxX = this.maxX;
    float minX = this.minX;

    if (player.isFirstPlayer == false)
    {
      maxX = this.minX;
      minX = this.maxX;
    }

    float percentX = mousePosition.x / Screen.width;
    percentX = Mathf.Clamp01(percentX);
    float percentY = mousePosition.y / Screen.height;
    percentY = Mathf.Clamp01(percentY);

    Vector3 ballPosition = new Vector3(minX + percentX * (maxX - minX), minY + percentY * (maxY - minY), ball.homeZPosition);
    ball.transform.position = ballPosition;
  }
  #endregion

  #region Private Read
  bool GetThrowMagnitude(
    out Vector3 direction)
  {
    direction = Vector3.zero;

    if (minPositionOfThrow == null)
    {
      return false;
    }

    direction = ball.transform.position - minPositionOfThrow.Value;

    // Minimum strength of throw - Play with this value for results
    if (direction.magnitude < minThrowMag)
    {
      return false;
    }

    return true;
  }
  #endregion
}
