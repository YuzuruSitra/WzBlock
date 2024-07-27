using System;
using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Block
{
    public class Block : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        public event Action<Block> OnReturnToPool;
        
        [SerializeField] private int _score;
        private ScoreHandler _scoreHandler;
        [SerializeField] private GameObject _hitEffect;
        private ParticleSystem _ps;
        private WaitForSeconds _wait;
        [SerializeField] 
        private BoxCollider _col;
        [SerializeField] 
        private MeshRenderer _mesh;
        [SerializeField]
        private Rigidbody _rb;
        [SerializeField]
        private float _speed;
        private float _yLimit;
        private Coroutine _coroutine;
        private ComboCounter _comboCounter;
        private bool _isActive;

        private void Start()
        {
            _scoreHandler = ScoreHandler.Instance;
            _ps = _hitEffect.GetComponent<ParticleSystem>();
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeState;
            _wait = new WaitForSeconds(_ps.main.duration);
            _yLimit = GameObject.FindWithTag("CubeLim").transform.position.y + transform.localScale.y / 2.0f;
            _comboCounter = ComboCounter.Instance;
        }

        private void Update()
        {
            if (!_isActive) return;
            if (_yLimit > transform.position.y) ReturnBlock();
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
            _rb.velocity = Vector3.down * _speed;
        }

        private void ChangeState(GameStateHandler.GameState state)
        {
            switch (state)
            {
                case GameStateHandler.GameState.FinGame:
                    _rb.velocity = Vector3.zero;
                    break;
                case GameStateHandler.GameState.Launch:
                    if (_isActive) ReturnBlock();
                    break;
            }
        }

        public void Activate()
        {
            ChangeLookActive(true);
        }

        private void ChangeLookActive(bool newActive)
        {
            _isActive = newActive;
            _col.enabled = newActive;
            _mesh.enabled = newActive;
            if (newActive) ChangeParticleSystem(false);
        }

        private void HitBall()
        {
            _comboCounter.ChangeCount(_comboCounter.ComboCount + 1);
            _scoreHandler.AddScore(_score);
            _coroutine = StartCoroutine(BreakWallAnim());
        }

        private IEnumerator BreakWallAnim()
        {
            ChangeLookActive(false);
            ChangeParticleSystem(true);
            yield return _wait;
            ChangeParticleSystem(false);
            _coroutine = null;
            OnReturnToPool?.Invoke(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
                HitBall();
        }

        private void ReturnBlock()
        {
            if (_coroutine != null) 
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                ChangeParticleSystem(false);
            }
            ChangeLookActive(false);
            OnReturnToPool?.Invoke(this);    
        }

        private void ChangeParticleSystem(bool isPlay)
        {
            if (!_ps) return;
            if (isPlay)
            {
                if (_ps.isPlaying) return;
                _ps.Play();
            }
            else
            {
                if (!_ps.isPlaying) return;
                _ps.Stop();
                _ps.Clear();
            }
        }

    }
}
