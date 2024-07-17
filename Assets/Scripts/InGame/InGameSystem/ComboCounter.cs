using System;
public class ComboCounter
{
    private static ComboCounter _instance;
    public static ComboCounter Instance => _instance ??= new ComboCounter();
    private GameStateHandler _gameStateHandler;
    public int ComboCount { get; private set; }
    public event Action<int> ChangeComboCount;

    private ComboCounter()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeState;
    }

    private void OnDestroy()
    {
        _gameStateHandler.ChangeGameState -= ChangeState;
    }

    private void ChangeState(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        ChangeCount(0);
    }

    public void ChangeCount(int value)
    {
        ComboCount = value;
        ChangeComboCount?.Invoke(ComboCount);
    }

}
