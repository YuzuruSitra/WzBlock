using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField]
    private float Speed = 1f;
    private Vector3 _launchPos;
    private GameStateHandler _gameStateHandler;

    void Start()
    {
        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePaddle;
    }

    void Update()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        var horizontal = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * horizontal * Speed * Time.deltaTime;
    }

    private void ChangeStatePaddle(GameStateHandler.GameState newState)
    {
        if (newState == GameStateHandler.GameState.Launch)
            transform.position = _launchPos;
    }

}
