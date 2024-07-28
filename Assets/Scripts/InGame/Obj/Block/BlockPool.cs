using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Obj.Block
{
    public class BlockPool : MonoBehaviour
    {
        public enum Blocks
        {
            Default,
            Bomb
        }
        
        [SerializeField] private Transform _blockParent;
        private BlockPositions _blockPositions;
        public List<BlockPositions.BlockSlotInfo> BlockSlot => _blockPositions.SlotInfo;
        
        [SerializeField] private BlockBase[] _blocks;
        private readonly List<BlockBase> _availableBlocks1 = new List<BlockBase>();
        public List<BlockBase> UsedBlocks1 { get; private set; } = new List<BlockBase>();

        private readonly List<BlockBase> _availableBlocks2 = new List<BlockBase>();
        public List<BlockBase> UsedBlocks2 { get; private set; } = new List<BlockBase>();
        
        private void Awake()
        {
            var pureAssetLoader = new PureAssetLoader();
            _blockPositions = pureAssetLoader.LoadScriptableObject<BlockPositions>("BlockPositions");
            if (_blockPositions != null) return;
            Debug.LogError("Failed to load BlockPositions");
        }

        public BlockBase GetBlock(Blocks kind)
        {
            var num = (int)kind;
            BlockBase block = null;
            
            switch (kind)
            {
                case Blocks.Default:
                    if (_availableBlocks1.Count <= 0) 
                    {
                        block = Instantiate(_blocks[num], _blockParent, true);
                        block.OnReturnToPool += ReturnBlock;
                        block.SetKind(kind);
                    }
                    else
                    {
                        block = _availableBlocks1[0];
                        _availableBlocks1.RemoveAt(0);
                    }
                    UsedBlocks1.Add(block);
                    break;
                case Blocks.Bomb:
                    if (_availableBlocks2.Count <= 0) 
                    {
                        block = Instantiate(_blocks[num], _blockParent, true);
                        block.OnReturnToPool += ReturnBlock;
                        block.SetKind(kind);
                    }
                    else
                    {
                        block = _availableBlocks2[0];
                        _availableBlocks2.RemoveAt(0);
                    }
                    UsedBlocks2.Add(block);
                    break;
            }
            return block;
        }

        private void ReturnBlock(BlockBase block)
        {
            var kind = block.Kind;
            switch (kind)
            {
                case Blocks.Default:
                    UsedBlocks1.Remove(block);
                    _availableBlocks1.Add(block);
                    break;
                case Blocks.Bomb:
                    UsedBlocks2.Remove(block);
                    _availableBlocks2.Add(block);
                    break;
            }
        }
    }
}
