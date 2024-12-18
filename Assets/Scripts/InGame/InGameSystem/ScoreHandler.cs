using System;

namespace InGame.InGameSystem
{
    public class ScoreHandler
    {
        // シングルトン
        private static ScoreHandler _instance;
        public static ScoreHandler Instance => _instance ??= new ScoreHandler();
        public int CurrentScore { get; private set; }

        public event Action<int> AddScoreEvent;
        public event Action<int> ChangeScore;
        public int MaxScore { get; private set; }

        public int TodayMaxScore { get; private set; }

        private readonly PlayDataIO _playDataIO;
        private readonly PlayerInfoHandler _playerInfoHandler;
        private readonly ComboCounter _comboCounter;

        private ScoreHandler()
        {
            _playDataIO = PlayDataIO.Instance;
            _playDataIO.DeleteDataEvent += LoadScores;
            LoadScores();
            _playerInfoHandler = PlayerInfoHandler.Instance;
            GameStateHandler.Instance.ChangeGameState += ScoreStateFunction;
            _comboCounter = ComboCounter.Instance;
        }

        public void AddScore(int addValue)
        {
            var addScore = addValue * _comboCounter.ComboCount;
            CurrentScore += addScore;
            AddScoreEvent?.Invoke(addScore);
            ChangeScore?.Invoke(CurrentScore);
        }

        private void ScoreStateFunction(GameStateHandler.GameState newState)
        {
            switch (newState)
            {
                case GameStateHandler.GameState.Launch:
                    CurrentScore = 0;
                    break;
                case GameStateHandler.GameState.FinGame:
                    UpdateMaxScores();
                    _playerInfoHandler.CalcLevel(CurrentScore);
                    break;
            }

            if (newState != GameStateHandler.GameState.Launch) return;
            CurrentScore = 0;
        }

        private void UpdateMaxScores()
        {
            if (MaxScore < CurrentScore)
            {
                _playDataIO.SaveMaxScore(CurrentScore);
                MaxScore = CurrentScore;
            }

            if (TodayMaxScore >= CurrentScore) return;
            _playDataIO.SaveTodayScore(CurrentScore);
            TodayMaxScore = CurrentScore;
        }

        private void LoadScores()
        {
            MaxScore = _playDataIO.LoadMaxScore();
            TodayMaxScore = _playDataIO.LoadMaxTodayScore(DateTime.Now.Date);
        }

    }
}
