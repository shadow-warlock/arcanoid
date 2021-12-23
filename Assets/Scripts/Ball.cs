using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float Speed = 9.0f;
    private float _stateChangeDeltaTime = 0;
    private Vector2 _minimalVertical = new Vector2(0.9f, 0.1f);
    private State state = State.Wait;
    private const float AccelerationTime = 2;
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
            State.Run => Speed,
            _ => 0
        };
        Vector2 normalized = _rigidbody.velocity.normalized;
        if (Math.Abs(normalized.y) < Math.Abs(_minimalVertical.y))
        {
            float x = _minimalVertical.x * Mathf.Sign(normalized.x);
            float y = _minimalVertical.y * Mathf.Sign(normalized.y);
            normalized = new Vector2(x, y);
        }

        if (state == State.Run && _stateChangeDeltaTime == 0)
        {
            normalized = new Vector2(0, 1);
        }
        _stateChangeDeltaTime += Time.deltaTime;
        _rigidbody.velocity = normalized * idealSpeed * (Math.Min(_stateChangeDeltaTime / AccelerationTime, 1));
    }

    public void Pulse()
    {
        transform.SetParent(GameObject.FindWithTag("BallContainer").transform);
        state = State.Run;
        _stateChangeDeltaTime = 0;
        _rigidbody.velocity = new Vector2(0, 1);
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