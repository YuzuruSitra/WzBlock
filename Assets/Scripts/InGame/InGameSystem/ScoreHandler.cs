using System;
using UnityEngine;

public class ScoreHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static ScoreHandler instance;
    public static ScoreHandler Instance => instance ?? (instance = new ScoreHandler());
    private int _currentScore;
    public int CurrentScore => _currentScore;
    public event Action<int> AddScoreEvent;
    public event Action<int> ChangeScore;
    private BallMover _ballMover;
    private int _maxScore;
    public int MaxScore => _maxScore;
    private int _todayMaxScore;
    public int TodayMaxScore => _todayMaxScore;
    private PlayDataIO _playDataIO;

    private ScoreHandler ()
    {
        _playDataIO = new PlayDataIO();
        LoadScores();
        GameStateHandler.Instance.ChangeGameState += ScoreStateFunction;
        _ballMover = GameObject.FindWithTag("Ball").GetComponent<BallMover>();
    }

    public void AddScore(int addValue)
    {
        int addScore = addValue * _ballMover.HitCount;
        _currentScore += addScore;
        AddScoreEvent?.Invoke(addScore);
        ChangeScore?.Invoke(_currentScore);
    }

    private void ScoreStateFunction(GameStateHandler.GameState newState)
    {
        switch (newState)
        {
            case GameStateHandler.GameState.Launch:
                _currentScore = 0;
                break;
            case GameStateHandler.GameState.FinGame:
                UpdateMaxScores();
                break;
        }
        if (newState != GameStateHandler.GameState.Launch) return;
        _currentScore = 0;
    }

    private void UpdateMaxScores()
    {
        if (_maxScore < _currentScore)
        {
            _playDataIO.SaveMaxScore(_currentScore);
            _maxScore = _currentScore;
        }
        if (_todayMaxScore < _currentScore)
        {
            _playDataIO.SaveTodayScore(_currentScore);
            _todayMaxScore = _currentScore;
        }
    }

    private void LoadScores()
    {
        _maxScore = _playDataIO.LoadMaxScore();
        _todayMaxScore = _playDataIO.LoadMaxTodayScore(DateTime.Now.Date);
    }

}
