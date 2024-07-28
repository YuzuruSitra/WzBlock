using System;
using UnityEngine;

namespace InGame.Obj.Ball
{
    public class BallSmasher : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        public event Action SmashEvent;
        public const float ExplosionAddForce = 3.0f;
        public const int MaxSmashCount = 4;
        private int _smashCount;
        public event Action<int> ChangeCountEvent;
        [SerializeField] private BallMover _ballMover;
        [SerializeField] private Transform _cubeLim;
        [SerializeField] private Transform _cubeUpper;
        
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
            if (hitObj.CompareTag("Paddle") || hitObj.CompareTag("BlockBomb"))
            {
                OnChangeCountEvent(0);
                return;
            }
            if (hitObj.CompareTag("BlockDefault") || hitObj.CompareTag("EnemyBullet")) OnChangeCountEvent(_smashCount + 1);
            if (_smashCount < MaxSmashCount) return;
            SmashEvent?.Invoke();
            OnChangeCountEvent(0);
        }

        public Vector3 SmashDirection()
        {
            var direction = Vector3.zero;
            direction.x = UnityEngine.Random.Range(-1.0f, 1.0f);
            direction.y = -1.0f;
            // Random Bottom
            return direction;
        }
        
        private void OnChangeCountEvent(int count)
        {
            _smashCount = count;
            ChangeCountEvent?.Invoke(_smashCount);
        }
    }
}
