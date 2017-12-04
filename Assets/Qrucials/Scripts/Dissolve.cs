using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
  private const float dissolveLength = 2;
  private MeshRenderer[] renderers;

  private HitDetector detector;

  private void Start()
  {
    detector = GetComponentInChildren<HitDetector>();
    renderers = GetComponentsInChildren<MeshRenderer>();

    detector.onHit += OnDetectorHit;
  }

  private void OnDestroy()
  {
    detector.onHit -= OnDetectorHit;
  }

  public void OnDetectorHit(
    Player scoringPlayer)
  {
    DissolvePot();
  }

  private void DissolvePot()
  {
    StartCoroutine(DissolveRoutine());
  }

  IEnumerator DissolveRoutine()
  {
    float startTime = Time.time;
    float step = 0;

    while (step < 1)
    {
      Debug.Log(step);

      float time = Time.time - startTime;
      step = time / dissolveLength;

      foreach (var render in renderers)
      {
        foreach (var mat in render.materials)
        {
          mat.SetFloat("_DisVal", step);
        }
      }
      yield return null;
    }
  }
}
