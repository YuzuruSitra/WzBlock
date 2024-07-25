using System;
using UnityEngine;

namespace InGame.Obj.Ball
{
    public class BallSmasher : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        public event Action ExplosionEvent;
        private Rigidbody _rigidBody;
        [SerializeField] private float _explosionForce = 6.0f;
        public const int MaxSmashCount = 5;
        private readonly Vector3[] _explosionDirection = new[]
        {
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0)
        };
        private int _smashCount;
        public event Action<int> ChangeCountEvent;
        [SerializeField] private BallMover _ballMover;
        
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
            OnChangeCountEvent(0);
        }

        public void AvoidFrameStack(GameObject hitObj)
        {
            if (!hitObj.CompareTag("Frame") && !hitObj.CompareTag("Paddle"))
            {
                OnChangeCountEvent(0);
                return;
            }
            
            if (hitObj.CompareTag("Frame")) OnChangeCountEvent(_smashCount + 1);
            if (_smashCount < MaxSmashCount) return;
            DoSmash();
        }

        private void DoSmash()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            var rnd = UnityEngine.Random.Range(0, _explosionDirection.Length);
            _rigidBody.AddForce(_explosionDirection[rnd] * _explosionForce, ForceMode.Impulse);
            OnChangeCountEvent(0);
            ExplosionEvent?.Invoke();
        }

        private void OnChangeCountEvent(int count)
        {
            _smashCount = count;
            ChangeCountEvent?.Invoke(_smashCount);
        }
    }
}
