using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CauldronHitSound : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    private AudioSource source;
    HitDetector detector;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        detector = GetComponentInChildren<HitDetector>();
        detector.onHit += OnDetectorHit;
    }

    private void OnDestroy()
    {
        detector.onHit -= OnDetectorHit;
    }

    private void OnDetectorHit(Player scoringPlayer)
    {
        source.PlayOneShot(sounds[UnityEngine.Random.Range(0, sounds.Length)]);
    }
}
