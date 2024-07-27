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
        [SerializeField] private float _addSpeed = 1f;
        [SerializeField] private float _reduceSpeed = 0.25f;
        [SerializeField] private float _shakePower = 1.0f;
        private Vector2 _launchDirection = new Vector2(-1, -0.5f);
        private Rigidbody _rigidBody;
        private GameStateHandler _gameStateHandler;
        private Vector3 _currentDirection;
        private float _currentSpeed;
        public float CurrentSpeedRatio => Mathf.Clamp(_currentSpeed / _maxSpeed, 0, _maxSpeed);
        public event Action HitPaddleEvent;

        [SerializeField] private ShakeByDOTween _shakeByDoTween;
        private bool _isFirstPush;
        private ComboCounter _comboCounter;
        [SerializeField] private BallSmasher _ballSmasher;
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _launchPos = transform.position;
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateBall;
            _ballSmasher.SmashEvent += DoSmash;
            _comboCounter = ComboCounter.Instance;
        }

        public void FixedUpdate()
        {
            // Care stack.
            if (_rigidBody.velocity.magnitude <= 0)  _currentDirection = -_currentDirection;
            
            _rigidBody.velocity = _currentDirection * _currentSpeed;
            if (_rigidBody.velocity.normalized != Vector3.zero) _currentDirection = _rigidBody.velocity.normalized;
            if (_rigidBody.velocity.magnitude <= _minSpeed) _currentSpeed = _minSpeed;
        }

        private void DoSmash()
        {
            _currentDirection = _ballSmasher.SmashDirection();
            _currentSpeed += BallSmasher.ExplosionAddForce;
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
                        _currentDirection = _launchDirection.normalized;
                        _currentSpeed = _launchSpeed;
                        _isFirstPush = true;
                    }
                    break;
                case GameStateHandler.GameState.Launch:
                    _isFirstPush = false;
                    transform.position = _launchPos;
                    break;
                case GameStateHandler.GameState.FinGame:
                    _rigidBody.velocity = Vector3.zero;
                    break;
            }
        }
        
        private void ReflectDirection(Collision collision)
        {
            // 衝突した法線ベクトルを取得
            var normal = collision.contacts[0].normal;
            var reflectDirection = Vector3.Reflect(_currentDirection, normal);

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
                var newSpeed= _currentSpeed + _addSpeed;
                if (newSpeed >= _maxSpeed) newSpeed = _maxSpeed;
                _currentSpeed = newSpeed;
            }
            _currentDirection = reflectDirection;
            _rigidBody.velocity = _currentDirection * _currentSpeed;
        }
        
        private void ReduceForce(GameObject hitObj)
        {
            if (!hitObj.CompareTag("Block") && !hitObj.CompareTag("Bullet")) return;
            
            var newSpeed= _currentSpeed - _reduceSpeed;
            if (newSpeed <= _minSpeed) newSpeed = _minSpeed;
            _currentSpeed = newSpeed;
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
            _ballSmasher.AvoidFrameStack(collision.gameObject);
            ReduceForce(collision.gameObject);
            CalcHitCount(collision.gameObject);
            ShakeCamera(collision.gameObject);
        }
    }
}
