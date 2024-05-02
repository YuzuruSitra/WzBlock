using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private GameStateHandler _gameStateHandler;

    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.SetGameState(GameStateHandler.GameState.Launch);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _gameStateHandler.CurrentState == GameStateHandler.GameState.Launch)
            _gameStateHandler.SetGameState(GameStateHandler.GameState.InGame);
    }
}
