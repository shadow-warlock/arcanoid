using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float _minSpeed  = 8.0f;
    private float _maxSpeed  = 10.0f;
    private float _verticalMinSpeed  = 0.5f;
    private float _accel = 3.0f;
    private bool _start = false;
    private Rigidbody2D _rigidbody;
    private float _horizontalTime = 0;
    private float _horizontalTimeout = 3;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void DamageAnimation()
    {
        GetComponent<Animator>().SetTrigger("Damage");
    }

    private void Update()
    {
        if (_start)
        {
            if (Math.Abs(_rigidbody.velocity.y) < _verticalMinSpeed)
            {
                _horizontalTime += Time.deltaTime;
            }
            else
            {
                _horizontalTime = 0;
            }

            if (_horizontalTime >= _horizontalTimeout)
            {
                _rigidbody.AddRelativeForce(new Vector2(0, 1) * 2);
            }
        }
    }

    private void FixedUpdate ()
    {
        if (_start)
        {
            if(_rigidbody.velocity.magnitude < _minSpeed)
                _rigidbody.AddRelativeForce(_rigidbody.velocity * _accel);
            if(_rigidbody.velocity.magnitude > _maxSpeed)
                _rigidbody.AddRelativeForce(_rigidbody.velocity * -_accel);
        }
    }

    public void Pulse()
    {
        _horizontalTime = 0;
        _start = true;
        transform.SetParent(GameObject.FindWithTag("BallContainer").transform);
        _rigidbody.AddRelativeForce(new Vector2(0, 1) * _accel);
    }
    
    public void Magnet(Transform magnet)
    {
        _start = false;
        transform.SetParent(magnet);
        _rigidbody.velocity = new Vector2(0, 0);
    }

}
