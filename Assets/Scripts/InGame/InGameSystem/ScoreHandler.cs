using System;
using UnityEngine;

public class ScoreHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static ScoreHandler instance;
    public static ScoreHandler Instance => instance ?? (instance = new ScoreHandler());
    private int _currentScore;
    public int CurrentScore => _currentScore;
    public event Action<int> ChangeScore;
    private BallMover _ballMover;
    private ScoreHandler ()
    {
        GameStateHandler.Instance.ChangeGameState += ResetScore;
        _ballMover = GameObject.FindWithTag("Ball").GetComponent<BallMover>();
    }

    public void AddScore(int addValue)
    {
        _currentScore += addValue * _ballMover.HitCount;
        ChangeScore?.Invoke(_currentScore);
    }

    private void ResetScore(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        _currentScore = 0;
    }

}
