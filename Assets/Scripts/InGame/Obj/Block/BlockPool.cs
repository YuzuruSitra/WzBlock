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

        private List<Block> _availableBlocks;
        public int AvailableBlocksCount => _availableBlocks.Count;
        [SerializeField] 
        private List<Block> _usedBlocks = new List<Block>();

        private void Awake()
        {
            var pureAssetLoader = new PureAssetLoader();
            _blockPositions = pureAssetLoader.LoadScriptableObject<BlockPositions>("BlockPositions");
            _availableBlocks = new List<Block>();
            for (var i = 0; i < _blockPositions.SlotInfo.Count; i++)
            {
                var block = Instantiate(_blockPrefab, _blockPositions.SlotInfo[i].Position, Quaternion.identity,
                    _blockParent);
                block.OnReturnToPool += ReturnBlock;
                block.transform.position = _blockPositions.SlotInfo[i].Position;
                block.ChangeLookActive(true);
                _usedBlocks.Add(block);
            }
        }

        public void AllGetPool()
        {
            var blocksToActivate = new List<Block>(_availableBlocks);
            foreach (var block in blocksToActivate)
            {
                block.ChangeLookActive(true);
                
                _availableBlocks.Remove(block);
                _usedBlocks.Add(block);
            }
        }

        public Block GetBlock()
        {
            if (_availableBlocks.Count <= 0) return null;
            var block = _availableBlocks[0];
            _availableBlocks.RemoveAt(0);
            block.ChangeLookActive(true);
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
