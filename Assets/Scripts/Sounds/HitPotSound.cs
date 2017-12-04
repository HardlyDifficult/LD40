using System;
using UnityEngine;

public class HitPotSound : MonoBehaviour
{
  AudioSource audioSource;

  protected void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    HitDetector.onHitSound += HitDetector_onHitAnything;
  }

  void HitDetector_onHitAnything(
    Player scoringPlayer)
  {
    audioSource.Play();
  }
}
