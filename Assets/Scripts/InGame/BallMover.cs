using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    [Range(3, 10)]
    [SerializeField]
    private float _launchSpeed = 3f;
    [SerializeField]
    private float _minSpeed = 3f;
    [SerializeField]
    private float _maxSpeed = 10f;
    private Vector2 _launchDirection = new Vector2(1, 0.5f);
    private Rigidbody _rigidBody;
    private GameStateHandler _gameStateHandler;
    private Vector3 _currentVelocity;
    private Vector3 _currentangular;

    private int _hitFrameCount;
    private const int MAX_STACK_COUNT = 5;
    [SerializeField]
    private Transform _BottomCenterPos;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateBall;
    }

    private void ChangeStateBall(GameStateHandler.GameState newState)
    {
        switch (newState)
        {
            case GameStateHandler.GameState.InGame:
                _rigidBody.AddForce(_launchDirection.normalized * _launchSpeed * Time.deltaTime, ForceMode.Impulse);
                break;
            case GameStateHandler.GameState.Launch:
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
                _hitFrameCount = 0;
                break;
            default:
                _rigidBody.velocity = _currentVelocity;
                _rigidBody.angularVelocity = _currentangular;
                break;
        }
    }

    private void Update()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        Vector3 velocity = _rigidBody.velocity;
        float clampedSpeed = Mathf.Clamp(velocity.magnitude, _minSpeed, _maxSpeed);
        _rigidBody.velocity = velocity.normalized * clampedSpeed;
        _currentVelocity = _rigidBody.velocity;
        _currentangular = _rigidBody.angularVelocity;
    }

    // 壁反射でのスタック回避
    private void AvoidFrameStack(GameObject hitObj)
    {
        if (hitObj.CompareTag("Frame")) 
            _hitFrameCount ++;
        else 
            _hitFrameCount = 0;
        
        if (_hitFrameCount < MAX_STACK_COUNT) return;
        Vector3 direction = (_BottomCenterPos.position - transform.position).normalized;
        float speed = _rigidBody.velocity.magnitude;
        _rigidBody.velocity = direction * speed * Time.deltaTime;
        _hitFrameCount = 0;
    }

    // プレイヤーに当たったときに、跳ね返る方向を変える
    private void ChangeRefrection(GameObject hitObj)
    {
        if (hitObj.CompareTag("Paddle"))
        {
            Vector3 playerPos = hitObj.transform.position;
            Vector3 ballPos = transform.position;
            Vector3 direction = (ballPos - playerPos).normalized;
            float speed = _rigidBody.velocity.magnitude;
            _rigidBody.velocity = direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AvoidFrameStack(collision.gameObject);
        ChangeRefrection(collision.gameObject);
    }
}
