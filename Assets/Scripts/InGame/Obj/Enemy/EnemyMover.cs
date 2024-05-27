using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField]
    private EnemySurviveManager _enemySurviveManager;
    private Vector3 _launchPos;
    private Vector3 _direction = Vector3.right;
    [Range(0, 100)]
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _leftObj;
    [SerializeField]
    private GameObject _rightObj;
    private MoveRangeCalculator _moveRangeCalculator;
    private float _leftMaxPos => _moveRangeCalculator.LeftMaxPos;
    private float _rightMaxPos  => _moveRangeCalculator.RightMaxPos;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private float _moveWaitTime;
    void Start()
    {
        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateEnemyMover;
        _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj);
    }

    void Update()
    {
        if (!_enemySurviveManager.IsActive) return;
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
        // â¬ìÆàÊÇÃêßå¿
        Vector3 posX = transform.position;
        if (posX.x <= _leftMaxPos)
        {
            _direction = Vector3.right;
            posX.x = _leftMaxPos;
            transform.position = posX;
        }
        if (posX.x >= _rightMaxPos) 
        {
            _direction = Vector3.left;
            posX.x = _rightMaxPos;
            transform.position = posX;
        }
        transform.position += _direction * _speed * Time.deltaTime;
    }

    void OnDestroy()
    {
        _gameStateHandler.ChangeGameState -= ChangeStateEnemyMover;
    }

    private void ChangeStateEnemyMover(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        transform.position = _launchPos;
    }
}
