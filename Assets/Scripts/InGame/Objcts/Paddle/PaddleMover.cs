using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    private Vector3 _launchPos;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private GameObject _leftObj;
    [SerializeField]
    private GameObject _rightObj;
    private float _leftMaxPos;
    private float _rightMaxPos;
    private MoveRangeCalculator _moveRangeCalculator;
    private AbilityReceiver _abilityReceiver;

    void Start()
    {
        _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj);
        _leftMaxPos = _moveRangeCalculator.LeftMaxPos;
        _rightMaxPos  = _moveRangeCalculator.RightMaxPos;

        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePaddle;
        _abilityReceiver = AbilityReceiver.Instance;
    }

    public void MoveReceive(Vector3 movement)
    {
        if (_abilityReceiver.CurrentCondition == AbilityReceiver.Condition.Stan) return;
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        // êVÇµÇ¢à íuÇåvéZ
        Vector3 newPosition = transform.position + movement;

        // à⁄ìÆÇÃêßå¿Çê›íË
        if (newPosition.x <= _leftMaxPos)
            newPosition.x = _leftMaxPos;
        if (newPosition.x >= _rightMaxPos)
            newPosition.x = _rightMaxPos;

        // à⁄ìÆÇÃèàóù
        transform.position = newPosition;
    }

    private void ChangeStatePaddle(GameStateHandler.GameState newState)
    {
        if (newState == GameStateHandler.GameState.Launch)
            transform.position = _launchPos;
    }

}
