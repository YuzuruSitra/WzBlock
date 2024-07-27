using UnityEngine;

namespace InGame.Obj.Block
{
    public class BlockBomb : BlockBase
    {

        protected override void Awake()
        {
            base.Awake(); // BlockBaseのAwakeを呼び出す
        }
        
        protected override void Update()
        {
            base.Update(); // BlockBaseのUpdateを呼び出す
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate(); // BlockBaseのFixedUpdateを呼び出す
        }

    }
}
