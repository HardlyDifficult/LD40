using System;
using System.Collections;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
  Coroutine winCheckRoutine;

  [SerializeField]
  ParticleSystem[] particlesPlayOnHit;

  void Start()
  {
    // enable this for quick tests
    //winCheckRoutine = StartCoroutine(WinCheck());
  }

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
    yield return new WaitForSeconds(1);
    print("Nailed it!");
    for (int i = 0; i < particlesPlayOnHit.Length; i++)
    {
      particlesPlayOnHit[i]?.Play();
    }
    winCheckRoutine = null;
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
