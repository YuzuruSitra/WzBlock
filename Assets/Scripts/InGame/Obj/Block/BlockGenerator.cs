using System;
using InGame.Event;
using InGame.InGameSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.Obj.Block
{
    public class BlockGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _ball;
        [SerializeField] private BlockPool _blockPool;
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
        [SerializeField] private AllBreakEvent _allBreakEvent;
        [SerializeField] private MetaAIManipulator _metaAIManipulator;
        private int _currentBoredomLevel;
        private int[] _boredomInsFactor;
        [SerializeField] private int _minProbability;
        [SerializeField] private int _maxProbability;
        public void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ReStartIns;
            _metaAIManipulator.ChangeBoredomLevel += ChangeBoredomLevel;
            _currentBoredomLevel = _metaAIManipulator.InitialBoredomLevel;
            // Calc boredomInsFactor.
            var size = _metaAIManipulator.MaxLevel;
            _boredomInsFactor = new int[size];
            _boredomInsFactor[0] = _minProbability;
            _boredomInsFactor[size - 1] = _maxProbability;
            var interval = (_maxProbability - _minProbability) / (float)(size - 1);
            for (var i = 1; i < size - 1; i++)
            {
                _boredomInsFactor[i] = _minProbability + (int)(interval * i);
            }
        }

        private void Update()
        {
            if (_gameStateHandler.CurrentInGameState != GameStateHandler.GameState.InGame) return;
            InsCountDown();
        }

        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ReStartIns;
            _metaAIManipulator.ChangeBoredomLevel -= ChangeBoredomLevel;
        }

        private void ReStartIns(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            _currentInsTime = 0;
        }

        private void InsCountDown()
        {
            if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
            if (_allBreakEvent.IsBreaking) return;
            _currentInsTime += Time.deltaTime;
            if (_currentInsTime <= InsInterval) return;
            _currentInsTime = 0;
            PeriodicSpawn();
        }

        private void PeriodicSpawn()
        {
            var spawned = false;
            for (var i = 0; i < _blockPool.BlockSlot.Count; i++)
            {
                var rnd = Random.Range(0, 100);
                if (rnd < _boredomInsFactor[_currentBoredomLevel]) continue;
                var kind = ChooseBlockKind();
                var block =  _blockPool.GetBlock(kind);
                block.transform.position = _blockPool.BlockSlot[i].Position;
                block.Activate();
                spawned = true;
            }
            
            if (!spawned)
            {
                var randomSlotIndex = Random.Range(0, _blockPool.BlockSlot.Count);
                var kind = ChooseBlockKind();
                var block = _blockPool.GetBlock(kind);
                block.transform.position = _blockPool.BlockSlot[randomSlotIndex].Position;
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

        private void ChangeBoredomLevel(int level)
        {
            _currentBoredomLevel = level;
        }
        
    }
}

