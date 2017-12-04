using System;
using System.Collections;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
  public delegate void OnHit(HitDetector hitDetectorWhichWasHit, Player scoringPlayer);
  public static OnHit onHit;

  Coroutine winCheckRoutine;

  [SerializeField]
  ParticleSystem[] particlesPlayOnHit;

  [SerializeField]
  ParticleSystem[] particlesStopOnHit;

  [SerializeField]
  MeshRenderer[] objectsScaleDownOnHit;

  [SerializeField]
  public int spellOnHitPlayerId = -1; // -1 = local player
  [SerializeField]
  public int spellOnHitId = -1; // -1 = no spell linked to this detector

#if UNITY_EDITOR
  public bool editorHit;
#endif

  bool hasHit;

  TurnController turnController;

  protected void Awake()
  {
    turnController = GameObject.FindObjectOfType<TurnController>();
  }

  //void Start()
  //{
  //  // enable this for quick tests
  //  //winCheckRoutine = StartCoroutine(WinCheck());
  //}

#if UNITY_EDITOR
  void Update()
  {
    if (editorHit && winCheckRoutine == null)
    {
      winCheckRoutine = StartCoroutine(WinCheck());
    }
  }
#endif

  protected void OnTriggerEnter(
  Collider collider)
  {
    if (winCheckRoutine != null)
    {
      StopCoroutine(winCheckRoutine);
    }
    winCheckRoutine = StartCoroutine(WinCheck());
  }

  IEnumerator WinCheck()
  {
    if (hasHit)
    {
      yield break;
    }
    yield return new WaitForSeconds(1);


    hasHit = true;
    print("Nailed it!");
    turnController.Score(spellOnHitPlayerId == 0 ? 1 : 0);
    winCheckRoutine = null;
    if (spellOnHitId != -1)
    {
      PlayerSpellsManager.instance.SetPlayerSpellStatus(spellOnHitPlayerId, spellOnHitId, true);
    }
    for (int i = 0; i < particlesPlayOnHit.Length; i++)
    {
      particlesPlayOnHit[i]?.Play();
    }
    for (int i = 0; i < particlesStopOnHit.Length; i++)
    {
      particlesStopOnHit[i]?.Stop();
    }
    for (int t = 0; t < 50; ++t)
    {
      yield return new WaitForSeconds(0.1f);
      for (int i = 0; i < objectsScaleDownOnHit.Length; i++)
      {
        objectsScaleDownOnHit[i].transform.localScale *= 0.99f;
      }
    }

    onHit?.Invoke(this, spellOnHitPlayerId == 0 ? Player.player2 : Player.player1);

    for (int i = 0; i < objectsScaleDownOnHit.Length; i++)
    {
      objectsScaleDownOnHit[i].gameObject.SetActive(false);
    }
  }

  protected void OnTriggerExit(
    Collider collider)
  {
    if (winCheckRoutine != null)
    {
      StopCoroutine(winCheckRoutine);
    }
  }
}
