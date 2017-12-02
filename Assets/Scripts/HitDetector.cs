using System;
using System.Collections;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
  Coroutine winCheckRoutine;

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
