using System;

public class ScoreHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static ScoreHandler instance;
    public static ScoreHandler Instance => instance ?? (instance = new ScoreHandler());
    private int _currentScore;
    public int CurrentScore => _currentScore;
    public event Action<int> ChangeScore;
    private ScoreHandler ()
    {
        GameStateHandler.Instance.ChangeGameState += ReserScore;
    }

    public void AddScore(int addValue)
    {
        _currentScore += addValue;
        ChangeScore?.Invoke(_currentScore);
    }

    private void ReserScore(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        _currentScore = 0;
    }

}
