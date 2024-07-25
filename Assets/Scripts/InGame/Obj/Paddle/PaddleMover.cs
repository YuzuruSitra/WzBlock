using System;
using InGame.Gimmick;
using UnityEngine;

namespace InGame.Obj.Paddle
{
    public class PaddleMover : MonoBehaviour
    {
        private Vector3 _launchPos;
        private GameStateHandler _gameStateHandler;
        [SerializeField]
        private GameObject _leftObj;
        [SerializeField]
        private GameObject _rightObj;
        private float _leftMaxPos;
        private float _rightMaxPos;
        private MoveRangeCalculator _moveRangeCalculator;
        private AbilityReceiver _abilityReceiver;

        private void Start()
        {
            _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj, 0);
            _leftMaxPos = _moveRangeCalculator.LeftMaxPos;
            _rightMaxPos  = _moveRangeCalculator.RightMaxPos;

            _launchPos = transform.position;
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStatePaddle;
            _abilityReceiver = AbilityReceiver.Instance;
        }

        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStatePaddle;
        }

        public void MoveReceive(Vector3 movement)
        {
            if (_abilityReceiver.CurrentCondition == AbilityReceiver.Condition.Stan) return;
            var newPosition = transform.position + movement;
            
            if (newPosition.x <= _leftMaxPos)
                newPosition.x = _leftMaxPos;
            if (newPosition.x >= _rightMaxPos)
                newPosition.x = _rightMaxPos;
            
            transform.position = newPosition;
        }

        private void ChangeStatePaddle(GameStateHandler.GameState newState)
        {
            if (newState == GameStateHandler.GameState.Launch)
                transform.position = _launchPos;
        }

    }
}
