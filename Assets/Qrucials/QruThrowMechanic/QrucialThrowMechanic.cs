using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrucialThrowMechanic : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float throwStrength;

    // Could probably be automatic somehow
    [Header("Screen positions")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private bool holdingBall;
    private Vector3 originalBallPosition;

    private LinkedList<Vector3> positionList = new LinkedList<Vector3>();

    private LineRenderer line;
    private Coroutine indicatorRoutine;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        originalBallPosition = ball.transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!holdingBall)
            {
                holdingBall = true;

                // Probably want to cache this
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();
                ballRb.velocity = Vector3.zero;
                ballRb.useGravity = false;
            }
            else
            {
                SetBallPosition(Input.mousePosition);
                positionList.AddFirst(ball.transform.position);

                // Only store the last 10 positions - play with this value for results
                if (positionList.Count > 10)
                {
                    positionList.RemoveLast();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (holdingBall)
            {
                Vector3 direction;
                if (!GetThrowMagnitude(out direction))
                {
                    ball.transform.position = originalBallPosition;
                    // NOT THROWN STUFF
                }
                else
                {
                    ThrowBall(direction);
                }

                holdingBall = false;
                positionList.Clear();
            }
        }
    }

    private void ThrowBall(Vector3 direction)
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        Debug.Assert(rb != null);

        direction += transform.forward * direction.magnitude;

        Debug.DrawRay(ball.transform.position, direction);

        rb.useGravity = true;
        rb.AddForce((direction + transform.forward * direction.magnitude) * throwStrength, ForceMode.Impulse);

        // Show throw indicator
        if (indicatorRoutine != null) StopCoroutine(indicatorRoutine);
        indicatorRoutine = StartCoroutine(ShowThrowIndicator());
    }

    private IEnumerator ShowThrowIndicator()
    {
        line.enabled = true;

        line.SetPosition(0, positionList.Last.Value);
        line.SetPosition(1, positionList.First.Value);

        yield return new WaitForSeconds(2);

        line.enabled = false;
    }

    private bool GetThrowMagnitude(out Vector3 direction)
    {
        direction = Vector3.zero;

        // Need to hold for at least two frames
        if (positionList.Count < 2)
            return false;

        direction = positionList.First.Value - positionList.Last.Value;

        // Minimum strength of throw - Play with this value for results
        if (direction.magnitude < 1)
            return false;

        Debug.Log(direction.magnitude);

        return true;
    }

    private void SetBallPosition(Vector2 mousePosition)
    {
        float x = mousePosition.x / Screen.width;
        float y = mousePosition.y / Screen.height;

        Vector3 ballPosition = new Vector3(minX + x * (maxX - minX), minY + y * (maxY - minY), originalBallPosition.z);
        ball.transform.position = ballPosition;
    }
}
