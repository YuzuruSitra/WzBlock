using UnityEngine;
using System.Collections;

namespace InGame.Obj.Ball
{
    public class BallEffector : MonoBehaviour
    {
        private Rigidbody _rigidBody;
        [SerializeField] private GameObject _prjEffect;
        [SerializeField] private GameObject[] _changeSizeEffects;
        private Vector3[] _originSizeEffects;

        [SerializeField] private GameObject _paddleHitEffect;
        private GameObject _hitEffect;
        private WaitForSeconds _hitEffectDuration;

        [SerializeField] private GameObject _explosionEffect;
        private GameObject _expEffect;
        private WaitForSeconds _explosionEffectDuration;
        [SerializeField] private BallMover _ballMover;
        
        [SerializeField] private BallSmasher _ballSmasher;
        private MeshRenderer _meshRenderer;
        private Color _initialColor;
        [SerializeField] private Color _maxColor;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _hitEffect = Instantiate(_paddleHitEffect);
            _hitEffectDuration = new WaitForSeconds(_hitEffect.GetComponent<ParticleSystem>().main.duration);
            _ballMover.HitPaddleEvent += LaunchHitEffect;

            _expEffect = Instantiate(_explosionEffect);
            _explosionEffectDuration = new WaitForSeconds(_expEffect.GetComponent<ParticleSystem>().main.duration);
            _ballSmasher.SmashEvent += LaunchSmash;
            _ballSmasher.ChangeCountEvent += ChangeColor;
            _originSizeEffects = new Vector3[_changeSizeEffects.Length];
            for (var i = 0; i < _changeSizeEffects.Length; i++)
                _originSizeEffects[i] = _changeSizeEffects[i].transform.localScale;
            _meshRenderer = GetComponent<MeshRenderer>();
            _initialColor = _meshRenderer.material.GetColor(EmissionColor);
        }
        
        private void OnDestroy()
        {
            _ballMover.HitPaddleEvent -= LaunchHitEffect;
            _ballSmasher.SmashEvent -= LaunchSmash;
            _ballSmasher.ChangeCountEvent -= ChangeColor;
        }
        
        private void Update()
        {
            for (var i = 0; i < _changeSizeEffects.Length; i++)
                _changeSizeEffects[i].transform.localScale = _originSizeEffects[i] * _ballMover.CurrentSpeedRatio;

            var velocity = _rigidBody.velocity;
            if (velocity.magnitude <= 0)
                foreach (var t in _changeSizeEffects)
                    t.transform.localScale = Vector3.zero;

            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            var targetRotation = Quaternion.Euler(0, 0, angle);

            _prjEffect.transform.rotation =
                Quaternion.Lerp(_prjEffect.transform.rotation, targetRotation, Time.deltaTime * 50f);
        }

        private void LaunchHitEffect()
        {
            StartCoroutine(HitPaddleAnim());
        }

        private IEnumerator HitPaddleAnim()
        {
            _hitEffect.transform.position = transform.position;
            _hitEffect.SetActive(true);
            yield return _hitEffectDuration;
            _hitEffect.SetActive(false);
        }

        private void LaunchSmash()
        {
            StartCoroutine(ExplosionAnim());
        }

        private IEnumerator ExplosionAnim()
        {
            _expEffect.transform.position = transform.position;
            _expEffect.SetActive(true);
            yield return _explosionEffectDuration;
            _expEffect.SetActive(false);
        }
        
        private void ChangeColor(int smashCount)
        {
            var factor = smashCount / (float)(BallSmasher.MaxSmashCount - 1);
            // カウントに応じて色を補間
            var currentColor = Color.Lerp(_initialColor, _maxColor, factor);
            // 例として、オブジェクトの色を変更
            _meshRenderer.material.SetColor(EmissionColor, currentColor);
        }
    }
}
