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
        private const float InsInterval = 1.5f;
        private float _currentInsTime;
        private GameStateHandler _gameStateHandler;
        [Serializable]
        public struct BlockStackInfo
        {
            public BlockPool.Blocks _type;
            public int _needStackCount;
            public int _currentStackCount;
        }
        [SerializeField] private BlockStackInfo[] _blocksStackInfo;
        
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
            if (_currentInsTime <= InsInterval) return;
            _currentInsTime = 0;
            PeriodicSpawn();
        }

        private void PeriodicSpawn()
        {
            for (var i = 0; i < _blockPool.BlockSlot.Count; i++)
            {
                var isIns = Random.Range(0, 2);
                if (isIns == 0) continue;
                var kind = ChooseBlockKind();
                var block =  _blockPool.GetBlock(kind);
                block.transform.position = _blockPool.BlockSlot[i].Position;
                block.Activate();
            }
        }

        private BlockPool.Blocks ChooseBlockKind()
        {
            var selectNum = 0;
            for (var i = 0; i < _blocksStackInfo.Length; i++)
            {
                if (i == 0) continue;
                _blocksStackInfo[i]._currentStackCount++;
                if (_blocksStackInfo[i]._currentStackCount >= _blocksStackInfo[i]._needStackCount) selectNum = i;
            }
            _blocksStackInfo[selectNum]._currentStackCount = 0;
            return _blocksStackInfo[selectNum]._type;
        }
    }
}

