using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float _speed = 10.0f;
    private float _minimalNormalizedY = 0.1f;
    private float _stateChangeDeltaTime = 0;
    private State state = State.Wait;
    private float _accelerationTime = 2;
    private float _slowMotionSpeed = 0.5f;
    private Vector2 _slowStartSpeed;
    private Rigidbody2D _rigidbody;

    private enum State
    {
        Wait, Run, Slow
    }

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
        float idealSpeed = state switch
        {
            State.Slow => _slowMotionSpeed,
            State.Run => _speed,
            _ => 0
        };
        _stateChangeDeltaTime += Time.deltaTime;
        _rigidbody.velocity = _rigidbody.velocity.normalized * idealSpeed *
                              (Math.Min(_stateChangeDeltaTime / _accelerationTime, 1));
    }

    public void Pulse()
    {
        state = State.Run;
        _stateChangeDeltaTime = 0;
        transform.SetParent(GameObject.FindWithTag("BallContainer").transform);
    }

    public void Slow()
    {
        state = State.Slow;
        _stateChangeDeltaTime = 0;
        _slowStartSpeed = _rigidbody.velocity;
    }

    public void Haste()
    {
        state = State.Run;
        _stateChangeDeltaTime = 0;
        _rigidbody.velocity = _slowStartSpeed.normalized;
    }
}