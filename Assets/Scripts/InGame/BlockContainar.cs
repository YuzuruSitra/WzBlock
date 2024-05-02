using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockContainar : MonoBehaviour
{
    private Block[] _blocks;
    public Block[] Blocks => _blocks;
    
    public void ChangeBlockIndex(Block[] newBlocks)
    {
        _blocks = newBlocks;
    }
}
