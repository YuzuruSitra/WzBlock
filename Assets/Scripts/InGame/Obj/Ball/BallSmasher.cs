using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Obj.Ball
{
    public class BallSmasher : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        public event Action ExplosionEvent;
        private int _hitFrameCount;
        private Rigidbody _rigidBody;
        [SerializeField] private float _explosionForce = 6.0f;
        private const int MaxStackCount = 5;
        private readonly List<float> _baseAngles = new() { 0f, 90f, 180f, 270f, 360f };
        private readonly Vector3[] _explosionDirection = new[]
        {
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0)
        };
        private const float TiltThreshold = 10;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateBall;
        }
        
        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStateBall;
        }
        
        private void ChangeStateBall(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            _hitFrameCount = 0;
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
    
        private void OnCollisionEnter(Collision collision)
        {
            AvoidFrameStack(collision.gameObject);
        }
    
    }
}
