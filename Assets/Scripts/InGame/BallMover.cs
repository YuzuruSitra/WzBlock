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
    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーに当たったときに、跳ね返る方向を変える
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // プレイヤーの位置を取得
            Vector3 playerPos = collision.transform.position;
            // ボールの位置を取得
            Vector3 ballPos = transform.position;
            // プレイヤーから見たボールの方向を計算
            Vector3 direction = (ballPos - playerPos).normalized;
            // 現在の速さを取得
            float speed = _rigidBody.velocity.magnitude;
            // 速度を変更
            _rigidBody.velocity = direction * speed * Time.deltaTime;
        }
    }
}
