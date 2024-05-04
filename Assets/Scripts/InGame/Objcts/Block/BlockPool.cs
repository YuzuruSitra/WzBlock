using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField]
    private Block _blockPrefab; // プールするプレハブ
    [SerializeField]
    private BlockPositions _blockPositions;
    private int _initialSize => _blockPositions.SlotInfo.Count; // 初期のプールサイズ

    private List<Block> _availableBlocks; // プールされているブロック
    private List<Block> _usedBlocks; // 使用中のブロック

    private void Awake()
    {
        _availableBlocks = new List<Block>();
        _usedBlocks = new List<Block>();

        // 初期プールの作成
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

        // プールから取得
        if (_availableBlocks.Count > 0)
        {
            block = _availableBlocks[0];
            _availableBlocks.RemoveAt(0);
        }
        else
        {
            // プールが空の場合、新規インスタンスを作成
            block = Instantiate(_blockPrefab);
        }

        // ブロックの初期化
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
