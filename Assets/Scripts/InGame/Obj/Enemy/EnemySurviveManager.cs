using System;
using System.Collections;
using InGame.Event;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class EnemySurviveManager : MonoBehaviour
    {
        private ScoreHandler _scoreHandler;
        [SerializeField] private int _score;
        private GameStateHandler _gameStateHandler;
        [SerializeField] private float _generateInterVal;
        private float _currentInsTime;
        private bool _isAlive;
        public bool IsActive => _activateWaitT <= _currentWaitT;
        [SerializeField] private float _activateWaitT;
        private float _currentWaitT;
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Collider _col;

        [SerializeField] private GameObject _insEffectPrefab;
        private GameObject _insEffect;
        private ParticleSystem _ps;
        private WaitForSeconds _waitIns;

        [SerializeField] private GameObject _breakEffectPrefab;
        private GameObject _breakEffect;
        private ParticleSystem _psBreak;
        private WaitForSeconds _waitBreak;
        private Coroutine _insCoroutine;
        private Coroutine _destCoroutine;

        [SerializeField]
        private ShakeByDOTween _shakeByDoTween;
        [SerializeField]
        private float _shakePower = 2.0f;
        
        [SerializeField] private MetaAIManipulator _metaAIManipulator;
        private int _currentBoredomLevel;
        private readonly float[] _boredomScaleFactor = { 0.55f, 0.65f, 0.75f, 0.85f, 0.95f, 1.05f, 1.15f, 1.25f, 1.35f, 1.45f};
        [SerializeField] private EnemyShield _enemyShield;
        [SerializeField] private AllBreakEvent _allBreakEvent;
        private SoundHandler _soundHandler;
        [SerializeField] protected AudioClip _breakSound;
        
        private void Start()
        {
            _scoreHandler = ScoreHandler.Instance;
            _insEffect = Instantiate(_insEffectPrefab);
            _ps = _insEffect.GetComponent<ParticleSystem>();
            _waitIns = new WaitForSeconds(_ps.main.duration / 3.0f);

            _breakEffect = Instantiate(_breakEffectPrefab);
            _psBreak = _breakEffect.GetComponent<ParticleSystem>();
            _waitBreak = new WaitForSeconds(_psBreak.main.duration);
        
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateEnemySurvive;
            
            _metaAIManipulator.ChangeBoredomLevel += ChangeBoredomLevel;
            _currentBoredomLevel = _metaAIManipulator.InitialBoredomLevel;
            _soundHandler = GameObject.FindWithTag("SoundHandler").GetComponent<SoundHandler>();
        }
        
        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStateEnemySurvive;
            _metaAIManipulator.ChangeBoredomLevel -= ChangeBoredomLevel;
        }
        
        private void FixedUpdate()
        {
            if (_gameStateHandler.CurrentInGameState != GameStateHandler.GameState.InGame) return;
            _enemyShield.EnemyActive = IsActive;
            if (_isAlive)
            {
                _currentWaitT += Time.deltaTime;
                return;
            }
            _currentWaitT = 0;
            var factor = _boredomScaleFactor[_currentBoredomLevel];
            _currentInsTime += Time.deltaTime * factor;
            if (_currentInsTime <= _generateInterVal) return;
            ResetCoroutine(ref _destCoroutine);
            _insCoroutine = StartCoroutine(GenerateEnemy());
        }

        private IEnumerator GenerateEnemy()
        {
            _insEffect.transform.position = transform.position;
            _ps.Play();
            yield return _waitIns;
            _ps.Stop();
            _ps.Clear();
            ChangeLook(true);
            _currentInsTime = 0;
            _insCoroutine = null;
            _enemyShield.IsActive();
        }

        private IEnumerator DestroyEnemy()
        {
            _scoreHandler.AddScore(_score);
            _allBreakEvent.DoAllBreak();
            ChangeLook(false);
            _soundHandler.PlaySe(_breakSound);
            _breakEffect.SetActive(true);
            _breakEffect.transform.position = transform.position;
            yield return _waitBreak;
            _breakEffect.SetActive(false);
            _destCoroutine = null;
        }

        private void ChangeLook(bool state)
        {
            _isAlive = state;
            _mesh.enabled = state;
            _col.enabled = state;
        }
        private void ChangeStateEnemySurvive(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            ChangeLook(false);
            _enemyShield.Inactive();
            _currentInsTime = 0;
            _currentWaitT = 0;
            ResetCoroutine(ref _insCoroutine);
            ResetCoroutine(ref _destCoroutine);
        }

        private void ResetCoroutine(ref Coroutine coroutine)
        {
            if (coroutine == null) return;
            StopCoroutine(coroutine);
            coroutine = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            if (!IsActive) return;
            ResetCoroutine(ref _insCoroutine);
            _destCoroutine = StartCoroutine(DestroyEnemy());
            _shakeByDoTween.StartShake(_shakePower);
        }
        
        private void ChangeBoredomLevel(int level)
        {
            _currentBoredomLevel = level;
        }
    }
}
