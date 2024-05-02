using System;

public class GameStateHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static GameStateHandler instance;
    public static GameStateHandler Instance => instance ?? (instance = new GameStateHandler());
    public enum GameState
    {
        Launch,
        InGame,
        FinGame,
        Settings
    }
    private GameState _currentState = GameState.Launch;
    public GameState CurrentState => _currentState;
    public event Action<GameState> ChangeGameState;

    private GameStateHandler(){}

    public void SetGameState(GameState newState)
    {
        _currentState = newState;
        ChangeGameState?.Invoke(_currentState);
    }
}
