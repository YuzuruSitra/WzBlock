using DG.Tweening;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class ShakeByDOTween : MonoBehaviour
    {
        [SerializeField]
        private float _duration, _strength, _randomness;
        [SerializeField]
        private int _vibrato;
        [SerializeField]
        private bool _fadeOut;

        private Tweener _shaker;
        private Vector3 _initPosition;

        private void Start()
        {
            _initPosition = transform.position;
        }

        public void StartShake(float powerFactor)
        {
            if (_shaker != null)
            {
                _shaker.Kill();
                gameObject.transform.position = _initPosition;
            }
            _shaker = gameObject.transform.DOShakePosition(_duration, _strength * powerFactor, _vibrato, _randomness, _fadeOut);
        }
    }
}
