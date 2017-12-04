using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTornado : MonoBehaviour
{
    private Rigidbody _body;
    private readonly Vector3 _posMin = new Vector3(2.45f, -2.248f, 0);
    private readonly Vector3 _posMax = new Vector3(-2.45f, -2.248f, 0);
    public float Speed = 0.2f;
    public float _absMaxX = 2.45f;
    private float direction = -1;
    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        direction = -1;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>()?.AddForce(50 * direction, 0, 0, ForceMode.Force);
    }
    // Update is called once per frame
    void Update()
    {
        var npos = gameObject.transform.position;
        //if (Mathf.Abs(gameObject.transform.position.x) >= _absMaxX)
        //{
        //    direction *= -1;// == _posMax) ? _posMin : _posMax;
        //}

        if (_body.transform.position.x >= _absMaxX)
        {
            direction = 1;// == _posMax) ? _posMin : _posMax;
        }
        if (_body.transform.position.x <= -1 * _absMaxX)
        {
            direction = -1;// == _posMax) ? _posMin : _posMax;
        }
        npos.x -= (direction * Speed * Time.deltaTime);
        //gameObject.transform.position = npos;
        _body.MovePosition(npos);
    }
}
