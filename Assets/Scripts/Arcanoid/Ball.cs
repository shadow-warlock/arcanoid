using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float Speed = 9.0f;
    private float _stateChangeDeltaTime = 0;
    private Vector2 _minimalVertical = new Vector2(0.9f, 0.1f);
    private bool _start = false;
    private State state = State.Wait;
    private const float AccelerationTime = 2;
    private Vector2 _slowStartSpeed;
    private Rigidbody2D _rigidbody;
    private ParticleSystem _particleTail;

    private enum State
    {
        Wait,
        Run,
        Slow
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _particleTail = GetComponent<ParticleSystem>();
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

        if (state == State.Run && !_start)
        {
            _start = true;
            normalized = new Vector2(0, 1);
        }

        ParticleSystem.ShapeModule particleTailShape = _particleTail.shape;
        Vector3 rotation = particleTailShape.rotation;
        float angle = Mathf.Rad2Deg * Mathf.Asin(-normalized.y);
        if (normalized.x > 0)
        {
            angle = 180 - angle;
        }

        rotation.z = angle;
        particleTailShape.rotation = rotation;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor))
        {
            if (transform.parent.childCount == 1)
            {
                Platform platformScript = GameController.GetInstance().Platform;
                platformScript.TakeDamage(1);
            }
            transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
