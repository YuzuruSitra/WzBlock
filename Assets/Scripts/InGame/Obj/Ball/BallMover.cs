using UnityEngine;
using System;
using InGame.InGameSystem;

namespace InGame.Obj.Ball
{
    public class BallMover : MonoBehaviour
    {
        private Vector3 _launchPos;
        [Range(1, 10)] [SerializeField] private float _launchSpeed = 3f;
        [SerializeField] private float _minSpeed = 1f;
        [SerializeField] private float _maxSpeed = 7f;
        [SerializeField] private float _shakeSpeedLimit = 4f;
        [SerializeField] private float _paddleAddForce = 1f;
        [SerializeField] private float _hitReduceFactor = 0.9f;
        [SerializeField] private float _shakePower = 1.0f;
        private Vector2 _launchDirection = new Vector2(1, 0.5f);
        private Rigidbody _rigidBody;
        private GameStateHandler _gameStateHandler;
        private Vector3 _currentVelocity;
        private Vector3 _currentAngular;
        private float _currentSpeed;
        public float CurrentSpeedRatio => Mathf.Clamp(_currentSpeed / _maxSpeed, 0, _maxSpeed);
        public event Action HitPaddleEvent;

        [SerializeField]
        private ShakeByDOTween _shakeByDoTween;
        private bool _isFirstPush;
        private ComboCounter _comboCounter;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _launchPos = transform.position;
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateBall;
            _comboCounter = ComboCounter.Instance;
        }

        public void FixedUpdate()
        {
            if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;

            var velocity = _rigidBody.velocity;
            var speed = velocity.magnitude;
            // 最小速度未満の場合、速度を修正する
            if (speed < _minSpeed)
            {
                velocity = _currentVelocity.normalized * _minSpeed;
            }
            // 速度の再計算
            var setSpeed = Mathf.Clamp(speed, _minSpeed, _maxSpeed);
            _currentSpeed = setSpeed;
            velocity = velocity.normalized * setSpeed;
            _rigidBody.velocity = velocity;

            // 現在の速度を更新
            if (velocity != Vector3.zero)
                _currentVelocity = velocity;
            _currentAngular = _rigidBody.angularVelocity;
        }


        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStateBall;
        }

        private void ChangeStateBall(GameStateHandler.GameState newState)
        {
            switch (newState)
            {
                case GameStateHandler.GameState.InGame:
                    if (!_isFirstPush)
                    {
                        _rigidBody.AddForce(_launchDirection.normalized * _launchSpeed, ForceMode.Impulse);
                        _isFirstPush = true;
                        return;
                    }

                    _rigidBody.velocity = _currentVelocity;
                    _rigidBody.angularVelocity = _currentAngular;
                    break;
                case GameStateHandler.GameState.Launch:
                    _isFirstPush = false;
                    transform.position = _launchPos;
                    break;
                case GameStateHandler.GameState.FinGame:
                    _rigidBody.velocity = Vector3.zero;
                    _rigidBody.angularVelocity = Vector3.zero;
                    _currentVelocity = Vector3.zero;
                    _currentSpeed = 0;
                    break;
                case GameStateHandler.GameState.Settings:
                    _rigidBody.velocity = Vector3.zero;
                    _rigidBody.angularVelocity = Vector3.zero;
                    break;
            }
        }
        
        private void ReflectDirection(Collision collision)
        {
            // 衝突した法線ベクトルを取得
            var normal = collision.contacts[0].normal;
            var incomingVelocity = _currentVelocity; // 保持している現在の速度を使用
            var speed = incomingVelocity.magnitude;
            var reflectDirection = Vector3.Reflect(incomingVelocity.normalized, normal);

            // パドルとの衝突処理
            if (collision.gameObject.CompareTag("Paddle"))
            {
                HitPaddleEvent?.Invoke();
                var paddle = collision.gameObject;
                var ballPos = transform.position;

                // パドルのローカル空間でのボールの位置を計算
                var paddleLocalPos = paddle.transform.InverseTransformPoint(ballPos);
                var paddleWidth = paddle.GetComponent<Renderer>().bounds.size.x;

                // パドルの左端から右端までの位置を -1 から 1 の範囲に正規化
                var offset = Mathf.Clamp(paddleLocalPos.x / (paddleWidth / 2), -1, 1);

                // オフセットに応じて反射方向を計算
                reflectDirection.x = offset;
                reflectDirection.y = 1.0f;
                reflectDirection.z = 0;
                reflectDirection = reflectDirection.normalized;
                speed += _paddleAddForce;
            }

            // 速度を保持しつつ反射方向のみ変更
            _rigidBody.velocity = reflectDirection * speed;
            _currentVelocity = _rigidBody.velocity; // 現在の速度を更新
        }


        private void ReduceForce(GameObject hitObj)
        {
            if (hitObj.CompareTag("Block") || hitObj.CompareTag("Bullet"))
            {
                var speed = _currentVelocity.magnitude * _hitReduceFactor;
                _rigidBody.velocity = _rigidBody.velocity.normalized * speed;
            }
        }

        private void CalcHitCount(GameObject hitObj)
        {
            if (hitObj.CompareTag("Paddle")) _comboCounter.ChangeCount(0);
        }

        private void ShakeCamera(GameObject hitObj)
        {
            if (!hitObj.CompareTag("Block")) return;
            var velocity = _rigidBody.velocity;
            var speed = velocity.magnitude;
            if (speed < _shakeSpeedLimit) return;
            _shakeByDoTween.StartShake(_shakePower);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ReflectDirection(collision); // 反射処理を呼び出す
            ReduceForce(collision.gameObject);
            CalcHitCount(collision.gameObject);
            ShakeCamera(collision.gameObject);
        }
    }
}
