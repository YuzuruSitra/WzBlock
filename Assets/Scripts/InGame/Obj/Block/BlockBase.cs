using System;
using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Block
{
    public abstract class BlockBase : MonoBehaviour
    {
        public BlockPool.Blocks Kind { get; private set; }
        public event Action<BlockBase> OnReturnToPool;

        [SerializeField] private int _score;
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private BoxCollider _col;
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _speed;

        private GameStateHandler _gameStateHandler;
        private ScoreHandler _scoreHandler;
        private ParticleSystem _ps;
        private WaitForSeconds _returnWait;
        private float _yLimit;
        private Coroutine _coroutine;
        private ComboCounter _comboCounter;
        private bool _isActive;
        protected SoundHandler SoundHandler;
        [SerializeField] protected AudioClip _hitSound;
        
        protected virtual void Start()
        {
            _scoreHandler = ScoreHandler.Instance;
            _ps = _hitEffect.GetComponent<ParticleSystem>();
            _gameStateHandler = GameStateHandler.Instance;
            _comboCounter = ComboCounter.Instance;
            _returnWait = new WaitForSeconds(_ps.main.duration);
            _yLimit = GameObject.FindWithTag("CubeLim").transform.position.y + transform.localScale.y / 2.0f;
            _gameStateHandler.ChangeGameState += ChangeState;
            SoundHandler = GameObject.FindWithTag("SoundHandler").GetComponent<SoundHandler>();
        }

        private void Update()
        {
            if (_gameStateHandler.CurrentInGameState != GameStateHandler.GameState.InGame) return;
            if (!_isActive || !(transform.position.y < _yLimit)) return;
            ReturnBlock();
        }

        private void FixedUpdate()
        {
            if (_gameStateHandler.CurrentInGameState != GameStateHandler.GameState.InGame) return;
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
                    if (_isActive)
                    {
                        ReturnBlock();
                    }
                    break;
            }
        }

        public void Activate()
        {
            SetActiveState(true);
        }

        public void SetKind(BlockPool.Blocks kind)
        {
            Kind = kind;
        }

        public void ReceiveBreak()
        {
            if (!_mesh.enabled) return;
            HitBall();
        }

        protected virtual void HitBall()
        {
            _comboCounter.ChangeCount(_comboCounter.ComboCount + 1);
            _scoreHandler.AddScore(_score);
            _coroutine = StartCoroutine(BreakWallAnim());
        }

        private IEnumerator BreakWallAnim()
        {
            SetActiveState(false);
            ToggleParticleSystem(true);
            yield return _returnWait;
            ToggleParticleSystem(false);
            _coroutine = null;
            OnReturnToPool?.Invoke(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball")) HitBall();
        }
        
        private void ReturnBlock()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            SetActiveState(false);
            OnReturnToPool?.Invoke(this);
        }

        private void SetActiveState(bool isActive)
        {
            _isActive = isActive;
            _col.enabled = isActive;
            _mesh.enabled = isActive;
            _rb.isKinematic = !isActive;
            if (isActive) ToggleParticleSystem(false);
        }

        private void ToggleParticleSystem(bool isPlay)
        {
            if (!_ps) return;

            switch (isPlay)
            {
                case true when !_ps.isPlaying:
                    _ps.Play();
                    break;
                case false when _ps.isPlaying:
                    _ps.Stop();
                    _ps.Clear();
                    break;
            }
        }
    }
}