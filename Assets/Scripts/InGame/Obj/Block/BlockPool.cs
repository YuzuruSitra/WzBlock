using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Obj.Block
{
    public class BlockPool : MonoBehaviour
    {
        [SerializeField] 
        private Transform _blockParent;
        [SerializeField] 
        private Block _blockPrefab;
        private BlockPositions _blockPositions;
        public List<BlockPositions.BlockSlotInfo> BlockSlot => _blockPositions.SlotInfo;

        private List<Block> _availableBlocks = new List<Block>();
        [SerializeField] 
        private List<Block> _usedBlocks = new List<Block>();

        private void Awake()
        {
            var pureAssetLoader = new PureAssetLoader();
            _blockPositions = pureAssetLoader.LoadScriptableObject<BlockPositions>("BlockPositions");
            if (_blockPositions != null) return;
            Debug.LogError("Failed to load BlockPositions");
        }

        public Block GetBlock()
        {
            Block block;
            if (_availableBlocks.Count <= 0) 
            {
                block = Instantiate(_blockPrefab, _blockParent, true);
                block.OnReturnToPool += ReturnBlock;
            }
            else
            {
                block = _availableBlocks[0];
                _availableBlocks.RemoveAt(0);
            }
            _usedBlocks.Add(block);
            return block;
        }

        private void ReturnBlock(Block block)
        {
            block.ChangeLookActive(false);
            _usedBlocks.Remove(block);
            _availableBlocks.Add(block);
        }
    }
}
