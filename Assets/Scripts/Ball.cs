using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float _minSpeed  = 8.0f;
    private float _maxSpeed  = 10.0f;
    private float _accel = 3.0f;
    private bool _start = false;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void DamageAnimation()
    {
        GetComponent<Animator>().SetTrigger("Damage");
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
        _start = true;
        transform.SetParent(GameObject.FindWithTag("BallContainer").transform);
        _rigidbody.AddRelativeForce(new Vector2(0, -1) * _accel);
    }

}
