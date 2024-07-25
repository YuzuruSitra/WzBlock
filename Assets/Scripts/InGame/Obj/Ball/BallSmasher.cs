using System;
using UnityEngine;

namespace InGame.Obj.Ball
{
    public class BallSmasher : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        public event Action SmashEvent;
        public readonly float ExplosionAddForce = 2.0f;
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
        [SerializeField] private Transform _cubeLim;
        [SerializeField] private Transform _cubeUpper;
        private Vector2 _centerPos;
        
        private void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateBall;
            _centerPos.x = 0;
            _centerPos.y = _cubeUpper.transform.position.y - _cubeLim.transform.position.y;
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
            SmashEvent?.Invoke();
            OnChangeCountEvent(0);
        }

        public Vector3 SmashPos()
        {
            var pos = transform.position;
            // Left Upper
            if (pos.x < _centerPos.x && pos.y > _centerPos.y) return _explosionDirection[0];
            // Left Bottom
            if (pos.x < _centerPos.x && pos.y < _centerPos.y) return _explosionDirection[1];
            // Right Upper
            if (pos.x > _centerPos.x && pos.y > _centerPos.y) return _explosionDirection[2];
            // Right Bottom
            return _explosionDirection[3];
        }
        
        private void OnChangeCountEvent(int count)
        {
            _smashCount = count;
            ChangeCountEvent?.Invoke(_smashCount);
        }
    }
}
