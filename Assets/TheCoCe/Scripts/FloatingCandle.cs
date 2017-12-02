using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCandle : MonoBehaviour {
    private Vector3 position;
    private float randomSpeed;
    [SerializeField]
    private float magnitude;
    private bool isSine;
    private int rotationDirection;
    private float randomOffset;
	[SerializeField]
	private Light light;

    // Use this for initialization
    void Awake()
    {
        position = transform.position;
        randomSpeed = Random.Range(0.4f, 0.6f);
        magnitude = Random.Range(0.3f, 0.5f);
        rotationDirection = (Random.value > 0.5f) ? -1 : 1;
        randomOffset = Random.Range(0f, 20.0f);
		
        light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float Offset = rotationDirection * Mathf.Sin(Time.time + randomOffset);
        transform.position = position + new Vector3(Mathf.Sin(Time.time + randomOffset + randomSpeed) * randomSpeed * magnitude, Offset * randomSpeed * magnitude, 0f);
        light.intensity = 1 + (Mathf.Sin(Time.time * 4f * randomSpeed) * 0.5f);
    }
}