using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public float RushBoost;
    public Transform[] PatrolPoints;
    public float WaitTime;
    public float RushTime;

    private Transform _playerTransform;
    private Transform _targetPoint;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody2D;
    private int _patrolIndex;
    private float _startWait;

    private float _startRush;

    public void Rush()
    {
        Debug.Log("Get over here!");
        _startRush = Time.time;
        _startWait = -WaitTime;
    }

    private void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _targetPoint = PatrolPoints[_patrolIndex];
        _startWait = -WaitTime;
        _startRush = -RushTime;
        _playerTransform = GameObject.Find("Hackerman").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _movement * ((moveSpeed + (IsRushing() ? RushBoost : 0)) * Utils.Utils.TimeMultiplier);
        _rigidbody2D.rotation = Mathf.LerpAngle(_rigidbody2D.rotation,
            Utils.Utils.AngleBetween(transform.position,
                IsRushing() ? _playerTransform.position : _targetPoint.position),
            IsWaiting() ? .05f : 0.3f);
    }

    private void Update()
    {
        Vector2 toPoint = (IsRushing() ? _playerTransform.position : _targetPoint.position) - transform.position;
        _movement = IsWaiting() ? Vector2.zero : toPoint.normalized;

        switch (toPoint.magnitude)
        {
            case < 0.2f:
                _patrolIndex = (_patrolIndex + 1) % PatrolPoints.Length;
                _targetPoint = PatrolPoints[_patrolIndex];
                _startWait = Time.time;
                break;
            case < 0.75f:
                if (IsRushing())
                {
                    GameManager.GameOver();
                }
                break;
        }
    }

    private bool IsRushing() => Time.time - _startRush < RushTime;
    private bool IsWaiting() => Time.time - _startWait < WaitTime;
}