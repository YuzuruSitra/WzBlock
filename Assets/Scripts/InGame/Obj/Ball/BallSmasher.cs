using System;
using UnityEngine;

namespace InGame.Obj.Ball
{
    public class BallSmasher : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        public event Action SmashEvent;
        public float ExplosionAddForce = 2.0f;
        public const int MaxSmashCount = 5;
        public readonly Vector3[] ExplosionDirection = new[]
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
            SmashEvent?.Invoke();
            OnChangeCountEvent(0);
        }
        
        private void OnChangeCountEvent(int count)
        {
            _smashCount = count;
            ChangeCountEvent?.Invoke(_smashCount);
        }
    }
}
