using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Block
{
    public class BlockBomb : BlockBase
    {
        [Header("Bomb Col")] [SerializeField] private SphereCollider _bombCol;
        [Header("Shake Cam Power")] [SerializeField] private float _shakePower;
        private WaitForSeconds _bombDuration;
        private ShakeByDOTween _shakeByDoTween;
        
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

        protected override void SetActiveState(bool isActive)
        {
            base.SetActiveState(isActive);
            if (isActive) _bombCol.enabled = false;
        }

        private IEnumerator DoBomb()
        {
            yield return _bombDuration;
            _bombCol.enabled = true;
            _shakeByDoTween.StartShake(_shakePower);
            yield return null;
            _bombCol.enabled = false;
        }

    }
}
