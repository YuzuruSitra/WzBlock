using UnityEngine;
using System.Collections.Generic;
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
            // �ŏ����x�����̏ꍇ�A���x���C������
            if (speed < _minSpeed)
            {
                velocity = _currentVelocity.normalized * _minSpeed;
            }
            // ���x�̍Čv�Z
            var setSpeed = Mathf.Clamp(speed, _minSpeed, _maxSpeed);
            _currentSpeed = setSpeed;
            velocity = velocity.normalized * setSpeed;
            _rigidBody.velocity = velocity;

            // ���݂̑��x���X�V
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

        private void ReflectDirection(Collision collision)
        {
            // �Փ˂����@���x�N�g�����擾
            var normal = collision.contacts[0].normal;
            var incomingVelocity = _currentVelocity; // �ێ����Ă��錻�݂̑��x���g�p
            var speed = incomingVelocity.magnitude;
            var reflectDirection = Vector3.Reflect(incomingVelocity.normalized, normal);

            // �p�h���Ƃ̏Փˏ���
            if (collision.gameObject.CompareTag("Paddle"))
            {
                HitPaddleEvent?.Invoke();
                var paddle = collision.gameObject;
                var playerPos = paddle.transform.position;
                var ballPos = transform.position;

                // �p�h���̃��[�J����Ԃł̃{�[���̈ʒu���v�Z
                var paddleLocalPos = paddle.transform.InverseTransformPoint(ballPos);
                var paddleWidth = paddle.GetComponent<Renderer>().bounds.size.x;

                // �p�h���̍��[����E�[�܂ł̈ʒu�� -1 ���� 1 �͈̔͂ɐ��K��
                var offset = Mathf.Clamp(paddleLocalPos.x / (paddleWidth / 2), -1, 1);

                // �I�t�Z�b�g�ɉ����Ĕ��˕������v�Z
                reflectDirection.x = offset;
                reflectDirection.y = 1.0f;
                reflectDirection.z = 0;
                reflectDirection = reflectDirection.normalized;
                speed += _paddleAddForce;
            }

            // ���x��ێ������˕����̂ݕύX
            _rigidBody.velocity = reflectDirection * speed;
            _currentVelocity = _rigidBody.velocity; // ���݂̑��x���X�V
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
            AvoidFrameStack(collision.gameObject);
            ReflectDirection(collision); // ���ˏ������Ăяo��
            ReduceForce(collision.gameObject);
            CalcHitCount(collision.gameObject);
            ShakeCamera(collision.gameObject);
        }
    }
}
