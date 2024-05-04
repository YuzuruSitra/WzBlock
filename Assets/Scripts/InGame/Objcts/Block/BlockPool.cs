using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField]
    private Block _blockPrefab; // �v�[������v���n�u
    [SerializeField]
    private BlockPositions _blockPositions;
    private int _initialSize => _blockPositions.SlotInfo.Count; // �����̃v�[���T�C�Y

    private List<Block> _availableBlocks; // �v�[������Ă���u���b�N
    private List<Block> _usedBlocks; // �g�p���̃u���b�N

    private void Awake()
    {
        _availableBlocks = new List<Block>();
        _usedBlocks = new List<Block>();

        // �����v�[���̍쐬
        for (int i = 0; i < _initialSize; i++)
        {
            Block block = Instantiate(_blockPrefab);
            block.gameObject.SetActive(false);
            _availableBlocks.Add(block);
        }
    }

    public Block GetBlock(Vector3 position, Transform parent = null)
    {
        Block block;

        // �v�[������擾
        if (_availableBlocks.Count > 0)
        {
            block = _availableBlocks[0];
            _availableBlocks.RemoveAt(0);
        }
        else
        {
            // �v�[������̏ꍇ�A�V�K�C���X�^���X���쐬
            block = Instantiate(_blockPrefab);
        }

        // �u���b�N�̏�����
        block.transform.position = position;
        block.transform.SetParent(parent);
        block.gameObject.SetActive(true);

        _usedBlocks.Add(block);
        return block;
    }

    public void ReturnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        _usedBlocks.Remove(block);
        _availableBlocks.Add(block);
    }
}
