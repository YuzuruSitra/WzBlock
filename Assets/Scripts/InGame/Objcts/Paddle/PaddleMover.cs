using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField]
    private float _speed = 1f;
    private Vector3 _launchPos;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private GameObject _leftObj;
    [SerializeField]
    private GameObject _rightObj;
    private float _leftMaxPos => _moveRangeCalculator.LeftMaxPos;
    private float _rightMaxPos  => _moveRangeCalculator.RightMaxPos;
    private MoveRangeCalculator _moveRangeCalculator;
    private AbilityReceiver _abilityReceiver;

    void Start()
    {
        _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj);
        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePaddle;
        _abilityReceiver = AbilityReceiver.Instance;
    }

    void Update()
    {
        if (_abilityReceiver.CurrentCondition == AbilityReceiver.Condition.Stan) return;
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        var horizontal = Input.GetAxis("Horizontal");
        // 可動域の制限
        Vector3 posX = transform.position;
        if (posX.x <= _leftMaxPos && horizontal < 0)
        {
            horizontal = 0;
            posX.x = _leftMaxPos;
            transform.position = posX;
        }
        if (posX.x >= _rightMaxPos && horizontal > 0) 
        {
            horizontal = 0;
            posX.x = _rightMaxPos;
            transform.position = posX;
        }
        transform.position += Vector3.right * horizontal * _speed * Time.deltaTime;
    }

    private void ChangeStatePaddle(GameStateHandler.GameState newState)
    {
        if (newState == GameStateHandler.GameState.Launch)
            transform.position = _launchPos;
    }

}
