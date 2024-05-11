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
    private float _centerPosX => _moveRangeCalculator.CenterPosX;
    private float _padding;
    private Vector3 _setPos;
    private MoveRangeCalculator _moveRangeCalculator;
    private AbilityReceiver _abilityReceiver;
    [SerializeField]
    private DragHandlerPad _dragHandlerPad;

    void Start()
    {
        _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj);
        _padding = (_rightMaxPos - _leftMaxPos) / 2.0f;

        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePaddle;
        _abilityReceiver = AbilityReceiver.Instance;
        _setPos = transform.position;
    }

    void Update()
    {
        if (_abilityReceiver.CurrentCondition == AbilityReceiver.Condition.Stan) return;
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        _setPos.x = _centerPosX + _padding * _dragHandlerPad.GetRelativePosition;
        transform.position = _setPos;
    }

    private void ChangeStatePaddle(GameStateHandler.GameState newState)
    {
        if (newState == GameStateHandler.GameState.Launch)
            transform.position = _launchPos;
    }

}
