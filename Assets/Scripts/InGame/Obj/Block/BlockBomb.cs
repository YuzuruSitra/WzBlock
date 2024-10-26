using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Block
{
    public class BlockBomb : BlockBase
    {
        [Header("Shake Cam Power")] [SerializeField] private float _shakePower;
        private WaitForSeconds _bombDuration;
        private ShakeByDOTween _shakeByDoTween;
        [SerializeField] private float _bombRay;
        private readonly Vector3[] _directions = {
            Vector3.right,                 // x軸正方向
            Vector3.left,                  // x軸負方向
            Vector3.up,               // z軸正方向
            Vector3.down,                  // z軸負方向
            (Vector3.right + Vector3.up).normalized,   // 右奥方向（x+z）
            (Vector3.right + Vector3.down).normalized,      // 右手前方向（x-z）
            (Vector3.left + Vector3.up).normalized,    // 左奥方向（-x+z）
            (Vector3.left + Vector3.down).normalized        // 左手前方向（-x-z）
        };
        
        protected override void Start()
        {
            base.Start();
            _bombDuration = new WaitForSeconds(0.25f);
            _shakeByDoTween = GameObject.FindWithTag("MainCamera").GetComponent<ShakeByDOTween>();
        }
            
        protected override void HitBall()
        {
            base.HitBall();
            SoundHandler.PlaySe(_hitSound);
            StartCoroutine(DoBomb());
        }

        private IEnumerator DoBomb()
        {
            yield return _bombDuration;
            
            _shakeByDoTween.StartShake(_shakePower);
            
            foreach (var direction in _directions)
            {
                // Rayを飛ばす
                if (!Physics.Raycast(transform.position, direction, out var hit, _bombRay)) continue;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Block"))
                    HandleHitObject(hit.collider.gameObject);
            }
        }
        
        private void HandleHitObject(GameObject hitObject)
        {
            var block = hitObject.GetComponent<BlockBase>();
            block.ReceiveBreak();
        }
    }
}
