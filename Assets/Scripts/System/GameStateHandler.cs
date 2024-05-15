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
    private GameState _currentInGameState;
    public GameState CurrentInGameState => _currentInGameState;
    private GameState _currentState;
    public GameState CurrentState => _currentState;
    public event Action<GameState> ChangeGameState;

    private GameStateHandler()
    {
        SetGameState(GameState.Launch);
    }

    public void SetGameState(GameState newState)
    {
        _currentState = newState;
        ChangeGameState?.Invoke(_currentState);
        
        if (newState == GameState.Settings) return;
        _currentInGameState = newState;
    }
}
