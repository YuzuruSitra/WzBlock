using System.Collections;
using UnityEngine;

public class EnemySurviveManager : MonoBehaviour
{
    private ScoreHandler _scoreHandler;
    [SerializeField]
    private int _score;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private float _generateInterVal;
    private float _currentInsTime;
    private bool _isAllive;
    public bool IsActive => _activateWaitT <= _currentWaitT;
    [SerializeField]
    private float _activateWaitT;
    private float _currentWaitT;
    [SerializeField]
    private MeshRenderer _mesh;
    [SerializeField]
    private Collider _col;

    [SerializeField]
    private GameObject _insEffectPrefab;
    private GameObject _insEffect;
    private ParticleSystem _psIns;
    private WaitForSeconds _waitIns;

    [SerializeField]
    private GameObject _breakEffectPrefab;
    private GameObject _breakEffect;
    private ParticleSystem _psBreak;
    private WaitForSeconds _waitBreak;
    private Coroutine _insCoroutine;
    private Coroutine _destCoroutine;

    void Start()
    {
        _scoreHandler = ScoreHandler.Instance;
        _insEffect = Instantiate(_insEffectPrefab);
        _psIns = _insEffect.GetComponent<ParticleSystem>();
        _waitIns = new WaitForSeconds(_psIns.main.duration / 3.0f);

        _breakEffect = Instantiate(_breakEffectPrefab);
        _psBreak = _breakEffect.GetComponent<ParticleSystem>();
        _waitBreak = new WaitForSeconds(_psBreak.main.duration);
        
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateEnemySurvive;
    }

    void FixedUpdate()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;

        if (_isAllive)
        {
            _currentWaitT += Time.deltaTime;
            return;
        }
        _currentWaitT = 0;

        _currentInsTime += Time.deltaTime;
        if (_currentInsTime <= _generateInterVal) return;
        ResetCoroutine(ref _destCoroutine);
        _insCoroutine = StartCoroutine(GenerateEnemy());
    }

    private IEnumerator GenerateEnemy()
    {
        _insEffect.SetActive(true);
        _insEffect.transform.position = transform.position;
        yield return _waitIns;
        _insEffect.SetActive(false);
        ChangeLook(true);
        _currentInsTime = 0;
        _insCoroutine = null;
    }

    private IEnumerator DestroyEnemy()
    {
        _scoreHandler.AddScore(_score);
        ChangeLook(false);
        _breakEffect.SetActive(true);
        _breakEffect.transform.position = transform.position;
        yield return _waitBreak;
        _breakEffect.SetActive(false);
        _destCoroutine = null;
    }

    private void ChangeLook(bool state)
    {
        _isAllive = state;
        _mesh.enabled = state;
        _col.enabled = state;
    }
    private void ChangeStateEnemySurvive(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        ChangeLook(false);
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


    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        if (!IsActive) return;
        ResetCoroutine(ref _insCoroutine);
        _destCoroutine = StartCoroutine(DestroyEnemy());
    }
}
