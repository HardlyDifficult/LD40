using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private const float dissolveLength = 2;
    private MeshRenderer[] renderers;

    private HitDetector detector;

	private void Start ()
    {
        detector = GetComponentInChildren<HitDetector>();
        renderers = GetComponentsInChildren<MeshRenderer>();

        HitDetector.onHit += OnDetectorHit;
	}

    private void OnDestroy()
    {
        HitDetector.onHit -= OnDetectorHit;
    }

    public void OnDetectorHit(HitDetector hitDetectorWhichWasHit, Player scoringPlayer)
    {
        if (hitDetectorWhichWasHit == detector)
        {
            DissolvePot();
        }
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

            step = Time.time / (startTime + dissolveLength);

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
