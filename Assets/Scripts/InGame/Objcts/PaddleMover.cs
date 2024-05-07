using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField]
    private float Speed = 1f;
    private Vector3 _launchPos;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private GameObject _leftObj;
    [SerializeField]
    private GameObject _rightObj;
    private float _leftMaxPos => _moveRangeCalculator.LeftMaxPos;
    private float _rightMaxPos  => _moveRangeCalculator.RightMaxPos;
    private MoveRangeCalculator _moveRangeCalculator;

    void Start()
    {
        _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj);
        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePaddle;
    }

    void Update()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        var horizontal = Input.GetAxis("Horizontal");
        // â¬ìÆàÊÇÃêßå¿
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
        transform.position += Vector3.right * horizontal * Speed * Time.deltaTime;
    }

    private void ChangeStatePaddle(GameStateHandler.GameState newState)
    {
        if (newState == GameStateHandler.GameState.Launch)
            transform.position = _launchPos;
    }

}
