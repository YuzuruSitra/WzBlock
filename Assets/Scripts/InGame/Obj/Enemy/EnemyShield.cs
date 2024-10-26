using System;
using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class EnemyShield : MonoBehaviour
    {
        private bool _isActiveShield;
        public bool EnemyActive { set; private get; }
        [SerializeField] 
        private float _fadeDuration = 1.5f;
        
        [SerializeField] 
        private float _maxSize = 1.15f;
        [SerializeField]
        private float _shakePower = 0.5f;
        private Material _material;
        private Coroutine _fadeCoroutine;
        [SerializeField]
        private ShakeByDOTween _shakeByDoTween;

        [SerializeField] 
        private Collider _col;

        private Color _initialColor;
        private Color _afterColor;
        private Vector3 _initialScale;
        
        private SoundHandler _soundHandler;
        [SerializeField] protected AudioClip _breakSound;

        public EnemyShield(bool enemyActive)
        {
            EnemyActive = enemyActive;
        }
        
        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _afterColor = _material.color;
            var color = _material.color;
            color.a = 1;
            _initialColor = color;
            _initialScale = transform.localScale;
            _soundHandler = GameObject.FindWithTag("SoundHandler").GetComponent<SoundHandler>();
        }
        
        public void IsActive()
        {
            _material.color = _initialColor;
            _col.enabled = true;
            transform.localScale = _initialScale;
            _isActiveShield = true;
            if (_fadeCoroutine == null) return;
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
        
        public void Inactive()
        {
            _material.color = _afterColor;
            _col.enabled = false;
        }

        private IEnumerator FadeOutCoroutine()
        {            
            var startColor = _material.color;
            var elapsedTime = 0f;
    
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / _fadeDuration;
        
                // アルファ値の更新
                startColor.a = Mathf.Lerp(1, 0, t);
                _material.color = startColor;
        
                // スケールの更新
                transform.localScale = Vector3.Lerp(_initialScale, _initialScale * _maxSize, t);

                yield return null;
            }
            
            startColor.a = 0;
            _material.color = startColor;
            transform.localScale = _initialScale * _maxSize;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            if (!_isActiveShield) return;
            if (!EnemyActive) return;
            _isActiveShield = false;
            _col.enabled = false;
            _soundHandler.PlaySe(_breakSound);
            _fadeCoroutine = StartCoroutine(FadeOutCoroutine());
            _shakeByDoTween.StartShake(_shakePower);
        }
        
    }
}
