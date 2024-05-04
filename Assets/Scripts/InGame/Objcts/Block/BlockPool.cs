using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField]
    private Transform _blockParent;
    [SerializeField]
    private Block _blockPrefab; // �v�[������v���n�u
    [SerializeField]
    private BlockPositions _blockPositions;

    private List<Block> _availableBlocks; // �v�[������Ă���u���b�N
    public int AvailableBlocksCount => _availableBlocks.Count;
    private List<Block> _usedBlocks; // �g�p���̃u���b�N

    private void Awake()
    {
        _availableBlocks = new List<Block>();
        _usedBlocks = new List<Block>();

        // �����v�[���̍쐬
        for (int i = 0; i < _blockPositions.SlotInfo.Count; i++)
        {
            Block block = Instantiate(_blockPrefab, _blockPositions.SlotInfo[i].Position, Quaternion.identity, _blockParent);
            block.OnReturnToPool += ReturnBlock;
            block.transform.position = _blockPositions.SlotInfo[i].Position;
            block.ChangeLookActive(true);
            _usedBlocks.Add(block);
        }
    }

    public void AllGetPool()
    {
        // �ύX�O�Ƀ��X�g�̃R�s�[���쐬
        List<Block> blocksToActivate = new List<Block>(_availableBlocks);
        foreach (Block block in blocksToActivate)
        {
            block.ChangeLookActive(true);

            // ���[�v��Ƀ��X�g��ύX
            _availableBlocks.Remove(block);
            _usedBlocks.Add(block);
        }
    }

    public Block GetBlock()
    {
        Block block;
        // �v�[������擾
        if (_availableBlocks.Count <= 0) return null;
        block = _availableBlocks[0];
        _availableBlocks.RemoveAt(0);
        block.ChangeLookActive(true);
        _usedBlocks.Add(block);
        return block;
    }

    public void ReturnBlock(Block block)
    {
        block.ChangeLookActive(false);
        _usedBlocks.Remove(block);
        _availableBlocks.Add(block);
    }
}
