using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFloating : MonoBehaviour
{
    [SerializeField]
    public int spellPlayerId = -1; // -1 = local player
    [SerializeField]
    public int spellId = 0;
    [SerializeField]
    public bool spellEnabled;
    [SerializeField]
    public float spellTransitionTime = 5;
    [SerializeField]
    public float spellRandomizeTime = 10;
    [SerializeField]
    private Vector3 displacementBias = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField]
    private Vector3 magnitudeMin = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField]
    private Vector3 magnitudeMax = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField]
    private Vector3 speedMin     = new Vector3(0.4f, 0.4f, 0.4f);
    [SerializeField]
    private Vector3 speedMax     = new Vector3(0.6f, 0.6f, 0.6f);

    [SerializeField]
    private Vector3 magnitude2Min = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField]
    private Vector3 magnitude2Max = new Vector3(0.4f, 0.4f, 0.4f);
    [SerializeField]
    private Vector3 speed2Min = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField]
    private Vector3 speed2Max = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 lastDisplacement = Vector3.zero;
    private Vector3 randomSpeed = Vector3.zero;
    private Vector3 randomMagnitude = Vector3.zero;
    private Vector3 randomSpeed2 = Vector3.zero;
    private Vector3 randomMagnitude2 = Vector3.zero;
    private Vector3 randomOffset = Vector3.zero;
    private Vector3 randomOffset2 = Vector3.zero;
    private float floatingTime = 0.0f;
    private float floatingStartingTime = 0.0f;

    private Vector3 previousRandomSpeed = Vector3.zero;
    private Vector3 previousRandomMagnitude = Vector3.zero;
    private Vector3 previousRandomSpeed2 = Vector3.zero;
    private Vector3 previousRandomMagnitude2 = Vector3.zero;
    private Vector3 previousRandomOffset = Vector3.zero;
    private Vector3 previousRandomOffset2 = Vector3.zero;

    // for main camera, we need to stop the animator
    [SerializeField]
    public Animator animator;


    // Use this for initialization
    void Awake()
    {
        RandomizeDisplacement();
    }

    void RandomizeDisplacement()
    {
        previousRandomSpeed = randomSpeed;
        previousRandomMagnitude = randomMagnitude;
        previousRandomOffset = randomOffset;
        previousRandomSpeed2 = randomSpeed2;
        previousRandomMagnitude2 = randomMagnitude2;
        previousRandomOffset2 = randomOffset2;
        floatingStartingTime = floatingTime;
        randomSpeed = new Vector3(
            UnityEngine.Random.Range(speedMin.x, speedMax.x),
            UnityEngine.Random.Range(speedMin.y, speedMax.y),
            UnityEngine.Random.Range(speedMin.z, speedMax.z));
        randomMagnitude = new Vector3(
            UnityEngine.Random.Range(magnitudeMin.x, magnitudeMax.x),
            UnityEngine.Random.Range(magnitudeMin.y, magnitudeMax.y),
            UnityEngine.Random.Range(magnitudeMin.z, magnitudeMax.z));
        randomOffset = new Vector3(
            UnityEngine.Random.Range(0f, 20.0f),
            UnityEngine.Random.Range(0f, 20.0f),
            UnityEngine.Random.Range(0f, 20.0f));
        randomSpeed2 = new Vector3(
            UnityEngine.Random.Range(speed2Min.x, speed2Max.x),
            UnityEngine.Random.Range(speed2Min.y, speed2Max.y),
            UnityEngine.Random.Range(speed2Min.z, speed2Max.z));
        randomMagnitude2 = new Vector3(
            UnityEngine.Random.Range(magnitude2Min.x, magnitude2Max.x),
            UnityEngine.Random.Range(magnitude2Min.y, magnitude2Max.y),
            UnityEngine.Random.Range(magnitude2Min.z, magnitude2Max.z));
        randomOffset2 = new Vector3(
            UnityEngine.Random.Range(0f, 20.0f),
            UnityEngine.Random.Range(0f, 20.0f),
            UnityEngine.Random.Range(0f, 20.0f));
    }

    private void Start()
    {
        PlayerSpellsManager.instance.onSpellChange += OnSpellChange;
    }

    protected void OnDestroy()
    {
        if (PlayerSpellsManager.instance)
            PlayerSpellsManager.instance.onSpellChange -= OnSpellChange;
    }

    private void OnSpellChange(int pi, int si, bool status, bool isLocalPlayer)
    {
        if (si == spellId && (pi == spellPlayerId || (isLocalPlayer && spellPlayerId == -1)))
        {
            spellEnabled = status;
            if (status && animator) animator.enabled = false;
        }
    }

    Vector3 currentDisplacement()
    {
        return new Vector3(
            (Mathf.Sin(floatingTime * randomSpeed.z + randomOffset.x) + displacementBias.x) * randomMagnitude.x,
            (Mathf.Sin(floatingTime * randomSpeed.y + randomOffset.y) + displacementBias.y) * randomMagnitude.y,
            (Mathf.Sin(floatingTime * randomSpeed.z + randomOffset.z) + displacementBias.z) * randomMagnitude.z
            ) + new Vector3(
            (Mathf.Sin(floatingTime * randomSpeed2.z + randomOffset2.x) + displacementBias.x) * randomMagnitude2.x,
            (Mathf.Sin(floatingTime * randomSpeed2.y + randomOffset2.y) + displacementBias.y) * randomMagnitude2.y,
            (Mathf.Sin(floatingTime * randomSpeed2.z + randomOffset2.z) + displacementBias.z) * randomMagnitude2.z);
    }

    Vector3 previousDisplacement()
    {
        return new Vector3(
            (Mathf.Sin(floatingTime * previousRandomSpeed.z + previousRandomOffset.x) + displacementBias.x) * previousRandomMagnitude.x,
            (Mathf.Sin(floatingTime * previousRandomSpeed.y + previousRandomOffset.y) + displacementBias.y) * previousRandomMagnitude.y,
            (Mathf.Sin(floatingTime * previousRandomSpeed.z + previousRandomOffset.z) + displacementBias.z) * previousRandomMagnitude.z
            ) + new Vector3(
            (Mathf.Sin(floatingTime * previousRandomSpeed2.z + previousRandomOffset2.x) + displacementBias.x) * previousRandomMagnitude2.x,
            (Mathf.Sin(floatingTime * previousRandomSpeed2.y + previousRandomOffset2.y) + displacementBias.y) * previousRandomMagnitude2.y,
            (Mathf.Sin(floatingTime * previousRandomSpeed2.z + previousRandomOffset2.z) + displacementBias.z) * previousRandomMagnitude2.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!spellEnabled) return;
        Vector3 current = currentDisplacement();
        if (floatingTime < floatingStartingTime + spellTransitionTime)
        {
            float factor = (floatingTime - floatingStartingTime) / spellTransitionTime;
            current *= factor;
            Vector3 previous = previousDisplacement();
            previous *= (1.0f - factor);
            current += previous;
        }
        transform.position += current - lastDisplacement;
        lastDisplacement = current;
        floatingTime += Time.deltaTime;
        if (floatingTime >= floatingStartingTime + spellRandomizeTime)
        {
            RandomizeDisplacement();
        }
    }
}