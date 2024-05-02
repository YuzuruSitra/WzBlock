using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockPositions _blockPositions;
    [SerializeField]
    private Block _blockPrefab;
    [SerializeField]
    private Transform _blockParent;
    [SerializeField]
    private BlockContainar _blockContainar;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupInsBlocks();
    }

    void SetupInsBlocks()
    {
        Block[] blocks = new Block[_blockPositions.SlotInfo.Count];
        for (int i = 0; i < _blockPositions.SlotInfo.Count; i++)
        {
            blocks[i] = Instantiate(_blockPrefab, _blockPositions.SlotInfo[i].Position, Quaternion.identity, _blockParent);
            blocks[i].SetID(i);
        }
        _blockContainar.ChangeBlockIndex(blocks);
    }
}
