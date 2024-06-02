using System;
using UnityEngine;

namespace InGame.Gimmick
{
    public class GimmickWall : MonoBehaviour
    {
        public event Action<GimmickWall> OnReturnToPool;

        [SerializeField] 
        private MeshRenderer _mesh;
        [SerializeField]
        private BoxCollider _col;
        [SerializeField]
        private float _remainTime = 3.0f;
        
        private bool _isActive;
        private float _currentTime;
        private Vector3 _initialScale;
        private Vector3 _currentScale;

        private void Start()
        {
            _initialScale = transform.localScale;
            _currentScale = _initialScale;
        }

        private void Update()
        {
            if (!_isActive) return;

            _currentTime -= Time.deltaTime;

            // スケールを変更する
            UpdateScale();

            if (!(_currentTime <= 0)) return;
            _isActive = false;
            _mesh.enabled = false;
            _col.enabled = false;
            OnReturnToPool?.Invoke(this);
        }

        public void ChangeLookActive(bool newActive)
        {
            _isActive = newActive;
            _mesh.enabled = newActive;
            _col.enabled = newActive;
            if (!newActive) return;
            _currentScale = _initialScale;
            transform.localScale = _initialScale;
            _currentTime = _remainTime;
        }

        private void UpdateScale()
        {
            if (_currentTime < 0) _currentTime = 0;
            var scaleFactor = _currentTime / _remainTime;
            _currentScale.x = _initialScale.x * scaleFactor;
            transform.localScale = _currentScale;
        }
    }
}