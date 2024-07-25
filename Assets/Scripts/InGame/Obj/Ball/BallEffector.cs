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

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _hitEffect = Instantiate(_paddleHitEffect);
            _hitEffectDuration = new WaitForSeconds(_hitEffect.GetComponent<ParticleSystem>().main.duration);
            _ballMover.HitPaddleEvent += LaunchHitEffect;

            _expEffect = Instantiate(_explosionEffect);
            _explosionEffectDuration = new WaitForSeconds(_expEffect.GetComponent<ParticleSystem>().main.duration);
            _ballSmasher.ExplosionEvent += LaunchExplosion;
            _originSizeEffects = new Vector3[_changeSizeEffects.Length];
            for (var i = 0; i < _changeSizeEffects.Length; i++)
                _originSizeEffects[i] = _changeSizeEffects[i].transform.localScale;
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

        private void LaunchExplosion()
        {
            StartCoroutine(ExplotionAnim());
        }

        private IEnumerator ExplotionAnim()
        {
            _expEffect.transform.position = transform.position;
            _expEffect.SetActive(true);
            yield return _explosionEffectDuration;
            _expEffect.SetActive(false);
        }
    }
}
