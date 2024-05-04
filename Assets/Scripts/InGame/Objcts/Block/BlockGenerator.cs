using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockPositions _blockPositions;
    [SerializeField]
    private BlockPool _blockPool;
    [SerializeField]
    private Transform _blockParent;
    
    void Start()
    {
        SetupInsBlocks();
    }

    void SetupInsBlocks()
    {
        for (int i = 0; i < _blockPositions.SlotInfo.Count; i++)
        {
            Vector3 position = _blockPositions.SlotInfo[i].Position;
            Block block = _blockPool.GetBlock(position, _blockParent); // �v�[������u���b�N���擾
            // �u���b�N���v�[���ɖ߂�Ƃ��̃C�x���g��ݒ�
            block.OnReturnToPool += _blockPool.ReturnBlock;
        }
    }
}
