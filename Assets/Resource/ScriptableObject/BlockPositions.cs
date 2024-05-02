using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlockPositions", menuName = "ScriptableObjects/CreateBlockPositions")]
public class BlockPositions : ScriptableObject
{
    [SerializeField]
    private List<BlockSlotInfo> _slotList = new List<BlockSlotInfo>();
    public List<BlockSlotInfo> SlotInfo => _slotList;

    [System.Serializable]
    public struct BlockSlotInfo
    {
        public int RowNum;
        public Vector3 Position;
    }
}
