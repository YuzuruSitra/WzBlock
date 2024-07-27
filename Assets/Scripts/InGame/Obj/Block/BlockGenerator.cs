using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.Obj.Block
{
    public class BlockGenerator : MonoBehaviour
    {
        [SerializeField] 
        private Transform _ball;
        [SerializeField] 
        private BlockPool _blockPool;
        private float _insInterval = 1.5f;
        private float _currentInsTime;
        private GameStateHandler _gameStateHandler;

        public void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ReStartIns;
        }

        private void Update()
        {
            InsCountDown();
        }

        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ReStartIns;
        }

        private void ReStartIns(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            _currentInsTime = 0;
        }

        private void InsCountDown()
        {
            if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
            if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
            _currentInsTime += Time.deltaTime;
            if (_currentInsTime <= _insInterval) return;
            _currentInsTime = 0;
            PeriodicSpawn();
        }

        private void PeriodicSpawn()
        {
            for (var i = 0; i < _blockPool.BlockSlot.Count; i++)
            {
                var isIns = Random.Range(0, 2);
                if (isIns == 0) continue;
                var block =  _blockPool.GetBlock();
                block.transform.position = _blockPool.BlockSlot[i].Position;
                block.Activate();
            }
        }

    }
}

