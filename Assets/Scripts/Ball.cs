using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
  #region Data
  // TODO
  /// <summary>
  /// Must be set when this is spawned
  /// </summary>
  public Player player;

  /// <summary>
  /// Differs for each team.
  /// </summary>
  GameObject ballModel;

  TurnController turnController;

  Rigidbody body;

  ParticleSystem[] particles;

  Vector3 originalBallPosition;
  #endregion

  #region Properties
  public float homeZPosition
  {
    get
    {
      return originalBallPosition.z;
    }
  }
  #endregion

  #region Init
  protected void Awake()
  {
    print(transform.position);
    turnController = GameObject.FindObjectOfType<TurnController>();
    body = GetComponent<Rigidbody>();
  }

  protected void Start()
  {
    if (player == null)
    {
      player = Player.remotePlayer;
    }
    if (player.isFirstPlayer == false)
    {
      Vector3 position = transform.position;
      position.z = -position.z;
      transform.position = position;
    }

    ballModel = transform.GetChild(player.isFirstPlayer ? 1 : 0).gameObject;
    particles = ballModel.GetComponentsInChildren<ParticleSystem>();
    originalBallPosition = transform.position;
  }
  #endregion

  #region Write
  public void ShowBall()
  {
    transform.position = originalBallPosition;
    ballModel.SetActive(true);
    body.useGravity = false;
    body.velocity = Vector3.zero;
    for (int i = 0; i < particles.Length; i++)
    {
      particles[i].Simulate(player.isFirstPlayer ? 0 : 1.5f, true, true);
      particles[i].Play();
    }
  }

  public void HideBall()
  {
    ballModel.SetActive(false);
  }

  public void Throw(
    Vector3 direction)
  {
    body.useGravity = true;
    body.AddForce(direction, ForceMode.Impulse);
  }
  #endregion
}
