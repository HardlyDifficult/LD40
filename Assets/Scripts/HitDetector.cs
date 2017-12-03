using System;
using System.Collections;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
  Coroutine winCheckRoutine;

  [SerializeField]
  ParticleSystem[] particlesPlayOnHit;
    
  [SerializeField]
  ParticleSystem[] particlesStopOnHit;
    
  [SerializeField]
  MeshRenderer[] objectsScaleDownOnHit;

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
    winCheckRoutine = null;
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
