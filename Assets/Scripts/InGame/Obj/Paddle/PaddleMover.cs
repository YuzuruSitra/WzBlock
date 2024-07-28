using System;
using InGame.Gimmick;
using UnityEngine;

namespace InGame.Obj.Paddle
{
    [RequireComponent(typeof(Rigidbody))]
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
        private Rigidbody _rigidbody;

        private void Start()
        {
            _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj, 0);
            _leftMaxPos = _moveRangeCalculator.LeftMaxPos;
            _rightMaxPos = _moveRangeCalculator.RightMaxPos;

            _launchPos = transform.position;
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStatePaddle;
            _abilityReceiver = AbilityReceiver.Instance;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStatePaddle;
        }

        public void MoveReceive(Vector3 movement)
        {
            if (_abilityReceiver.CurrentCondition == AbilityReceiver.Condition.Stan) return;

            var targetPosition = transform.position + movement;
            if (targetPosition.x <= _leftMaxPos)
                targetPosition.x = _leftMaxPos;
            if (targetPosition.x >= _rightMaxPos)
                targetPosition.x = _rightMaxPos;

            _rigidbody.MovePosition(targetPosition);
        }

        private void ChangeStatePaddle(GameStateHandler.GameState newState)
        {
            if (newState == GameStateHandler.GameState.Launch)
                _rigidbody.MovePosition(_launchPos);
        }
    }
}