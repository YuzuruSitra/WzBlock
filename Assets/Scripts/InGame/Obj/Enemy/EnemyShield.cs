using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class EnemyShield : MonoBehaviour
    {
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
        private Vector3 _initialScale;
        
        
        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            var color = _material.color;
            color.a = 1;
            _initialColor = color;
            _initialScale = transform.localScale;
        }
        
        public void IsActive()
        {
            _material.color = _initialColor;
            _col.enabled = true;
            transform.localScale = _initialScale;
            if (_fadeCoroutine == null) return;
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        private IEnumerator FadeOutCoroutine()
        {
            var startColor = _material.color;
            var elapsedTime = 0f;
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var alpha = Mathf.Lerp(startColor.a, 0, elapsedTime / _fadeDuration);
                _material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                
                var newScale = Vector3.Lerp(_initialScale, _initialScale * _maxSize, elapsedTime / _fadeDuration);
                transform.localScale = newScale;

                yield return null;
            }
            _material.color = new Color(startColor.r, startColor.g, startColor.b, 0);
            transform.localScale = _initialScale * _maxSize;
            _col.enabled = false;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            _fadeCoroutine = StartCoroutine(FadeOutCoroutine());
            _shakeByDoTween.StartShake(_shakePower);
        }
        
    }
}
