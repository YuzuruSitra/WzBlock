using System;
using System.Collections;
using InGame.Gimmick;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class BindBullet : MonoBehaviour
    {
        public event Action<BindBullet> OnReturnToPool;
        private bool _isActive;
        [SerializeField]
        private GameObject _hitEffectPrefab;
        private GameObject _hitEffect;
        private ParticleSystem _ps;
        private WaitForSeconds _wait;
    
        [SerializeField]
        private float _speed;
        [SerializeField]
        private GameObject _prjEffect;
        [SerializeField]
        private BoxCollider _col;

        private AbilityReceiver _abilityReceiver;
        [SerializeField]
        private AbilityReceiver.Condition _abilityType;
        [SerializeField]
        private float _abilityTime;
        private WaitForSeconds _waitAbility;

        private void Start()
        {
            _hitEffect = Instantiate(_hitEffectPrefab, transform, true);
            _ps = _hitEffect.GetComponent<ParticleSystem>();
            _wait = new WaitForSeconds(_ps.main.duration);
            _waitAbility = new WaitForSeconds(_abilityTime);
            _abilityReceiver = AbilityReceiver.Instance;
        }

        public void Update()
        {
            if (!_isActive) return;
            transform.position += Vector3.down * (_speed * Time.deltaTime);
        }

        public void ChangeLookActive(bool newActive)
        {
            _col.enabled = newActive;
            _prjEffect.SetActive(newActive);
            _isActive = newActive;
        }

        private IEnumerator BreakBulletAnim()
        {
            OnReturnToPool?.Invoke(this);
            _hitEffect.transform.position = transform.position;
            _hitEffect.SetActive(true);
            yield return _wait;
            _hitEffect.SetActive(false);
        }

        private IEnumerator SendAbility()
        {
            AbilityReceiver.Instance.ChangeCondition(_abilityType);
            yield return _waitAbility;
            AbilityReceiver.Instance.ChangeCondition(AbilityReceiver.Condition.Default);
        }

        private void HitPaddle()
        {
            if (!_abilityReceiver.IsEnable) return;
            StartCoroutine(BreakBulletAnim());
            StartCoroutine(SendAbility());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DestroyArea"))
                OnReturnToPool?.Invoke(this);
        }

        private void OnCollisionEnter (Collision collision)
        {
            if (collision.gameObject.CompareTag("Frame")
                || collision.gameObject.layer == LayerMask.NameToLayer("Block")
                || collision.gameObject.CompareTag("Ball"))
                StartCoroutine(BreakBulletAnim());
            if (collision.gameObject.CompareTag("Paddle"))
                HitPaddle();
        }
    }
}
