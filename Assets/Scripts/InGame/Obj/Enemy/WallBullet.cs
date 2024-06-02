using System;
using System.Collections;
using InGame.Gimmick;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.Obj.Enemy
{
    public class WallBullet : MonoBehaviour
    {
        public event Action<WallBullet> OnReturnToPool;
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
        private GameStateHandler _gameStateHandler;

        [SerializeField] 
        private float _destroyMinTime;
        [SerializeField] 
        private float _destroyMaxTime;
        private float _destroyTIme;
        private Coroutine _abilityCoroutine;

        private int _isMisfireCount;
        
        private void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _hitEffect = Instantiate(_hitEffectPrefab, transform, true);
            _ps = _hitEffect.GetComponent<ParticleSystem>();
            _wait = new WaitForSeconds(_ps.main.duration);
            _abilityReceiver = AbilityReceiver.Instance;
        }

        public void Update()
        {
            if (!_isActive) return;
            if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
            transform.position += Vector3.down * (_speed * Time.deltaTime);
            if (_abilityCoroutine == null)
                _destroyTIme -= Time.deltaTime;
            if (_isMisfireCount != 0) return;
            if (_destroyTIme <= 0) 
                _abilityCoroutine = StartCoroutine(GenerateWall());
        }

        public void ChangeLookActive(bool newActive)
        {
            _col.enabled = newActive;
            _prjEffect.SetActive(newActive);
            _isActive = newActive;
            if (!newActive) return;
            _abilityCoroutine = null;
            _destroyTIme = Random.Range(_destroyMinTime, _destroyMaxTime);
        }
        
        private IEnumerator GenerateWall()
        {
            _abilityReceiver.GenerateWall(transform.position);
            OnReturnToPool?.Invoke(this);
            _hitEffect.transform.position = transform.position;
            _hitEffect.SetActive(true);
            yield return _wait;
            _hitEffect.SetActive(false);
        }
        
        private IEnumerator BreakBulletAnim()
        {
            OnReturnToPool?.Invoke(this);
            _hitEffect.transform.position = transform.position;
            _hitEffect.SetActive(true);
            yield return _wait;
            _hitEffect.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DestroyArea"))
                OnReturnToPool?.Invoke(this);
            if (other.gameObject.CompareTag("WallBulletCol"))
                _isMisfireCount += 1;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("WallBulletCol"))
                _isMisfireCount -= 1;
        }

        private void OnCollisionEnter (Collision collision)
        {
            if (collision.gameObject.CompareTag("Frame")
                || collision.gameObject.CompareTag("Block")
                || collision.gameObject.CompareTag("Ball"))
                StartCoroutine(BreakBulletAnim());
        }
        
    }
}
