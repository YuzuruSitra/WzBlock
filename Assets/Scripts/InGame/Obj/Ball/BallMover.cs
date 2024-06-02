using UnityEngine;
using System.Collections.Generic;
using System;
using InGame.InGameSystem;

namespace InGame.Obj.Ball
{
    public class BallMover : MonoBehaviour
    {
        public int HitCount { get; private set; }

        public event Action<int> ChangeHitCount;
        private Vector3 _launchPos;
        [Range(1, 10)] [SerializeField] private float _launchSpeed = 3f;
        [SerializeField] private float _minSpeed = 1f;
        [SerializeField] private float _maxSpeed = 7f;
        [SerializeField] private float _shakeSpeedLimit = 4f;
        [SerializeField] private float _paddleAddForce = 1f;
        [SerializeField] private float _hitReduceFactor = 0.9f;
        [SerializeField] private float _explosionForce = 6.0f;
        [SerializeField] private float _shakePower = 1.0f;
        private Vector2 _launchDirection = new Vector2(1, 0.5f);
        private const float MinThreshold = 0.001f;
        private Rigidbody _rigidBody;
        private GameStateHandler _gameStateHandler;
        private Vector3 _currentVelocity;
        private Vector3 _currentAngular;
        private float _currentSpeed;
        public float CurrentSpeedRatio => Mathf.Clamp(_currentSpeed / _maxSpeed, 0, _maxSpeed);
        public event Action ExplosionEvent;
        public event Action HitPaddleEvent;
        private int _hitFrameCount;

        [SerializeField]
        private ShakeByDOTween _shakeByDoTween;

        private readonly List<float> _baseAngles = new List<float> { 0f, 90f, 180f, 270f, 360f };

        private readonly Vector3[] _explosionDirection = new Vector3[]
        {
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0)
        };

        private const int MaxStackCount = 5;
        private const float TiltThreshold = 10;
        private bool _isFirstPush;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _launchPos = transform.position;
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateBall;
        }

        public void FixedUpdate()
        {
            if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;

            var velocity = _rigidBody.velocity;
            var speed = velocity.magnitude;
            if (Mathf.Approximately(speed, 0f))
                velocity = _currentVelocity.normalized * _minSpeed;
            else if (speed < MinThreshold)
                velocity = transform.forward * _minSpeed;

            var setSpeed = Mathf.Clamp(speed, _minSpeed, _maxSpeed);
            _currentSpeed = setSpeed;
            velocity = velocity.normalized * setSpeed;
            _rigidBody.velocity = velocity;
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
                    HitCount = 0;
                    ChangeHitCount?.Invoke(HitCount);
                    _hitFrameCount = 0;
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

        private void AvoidFrameStack(GameObject hitObj)
        {
            if (!hitObj.CompareTag("Frame") && !hitObj.CompareTag("Paddle"))
            {
                _hitFrameCount = 0;
                return;
            }

            var velocity = _rigidBody.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            var isWithinThreshold = IsAngleWithinThreshold(angle);
            if (!isWithinThreshold) return;

            if (hitObj.CompareTag("Frame")) _hitFrameCount++;

            if (_hitFrameCount < MaxStackCount) return;

            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            var rnd = UnityEngine.Random.Range(0, _explosionDirection.Length);
            _rigidBody.AddForce(_explosionDirection[rnd] * _explosionForce, ForceMode.Impulse);
            _hitFrameCount = 0;
            ExplosionEvent?.Invoke();
        }

        private bool IsAngleWithinThreshold(float angle)
        {
            var index = 0;
            for (; index < _baseAngles.Count; index++)
            {
                var baseAngle = _baseAngles[index];
                if (Mathf.Abs(Mathf.Abs(angle) - baseAngle) <= TiltThreshold)
                    return true;
            }

            return false;
        }

        private void ChangeReflection(GameObject hitObj)
        {
            if (!hitObj.CompareTag("Paddle")) return;
            HitPaddleEvent?.Invoke();
            var playerPos = hitObj.transform.position;
            var ballPos = transform.position;
            var direction = (ballPos - playerPos).normalized;
            _rigidBody.AddForce(direction * _paddleAddForce, ForceMode.Impulse);
        }

        private void ReduceForce(GameObject hitObj)
        {
            if (hitObj.CompareTag("Block") || hitObj.CompareTag("Bullet"))
                _rigidBody.velocity *= _hitReduceFactor;
        }

        private void CalcHitCount(GameObject hitObj)
        {
            if (hitObj.CompareTag("Paddle")) HitCount = 0;
            if (hitObj.CompareTag("Block")) HitCount++;
            if (hitObj.CompareTag("Enemy")) HitCount++;
            ChangeHitCount?.Invoke(HitCount);
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
            AvoidFrameStack(collision.gameObject);
            ChangeReflection(collision.gameObject);
            ReduceForce(collision.gameObject);
            CalcHitCount(collision.gameObject);
            ShakeCamera(collision.gameObject);
        }
    }
}
