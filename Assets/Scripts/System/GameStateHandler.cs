using InGame.InGameSystem;
using UnityEngine;

namespace System
{
    public class GameStateHandler
    {
        private static GameStateHandler _instance;
        public static GameStateHandler Instance => _instance ??= new GameStateHandler();
        public enum GameState
        {
            Launch,
            InGame,
            FinGame,
            Settings
        }

        public GameState CurrentInGameState { get; private set; }

        public GameState CurrentState { get; private set; }

        public event Action<GameState> ChangeGameState;
        private readonly TimeScaleHandler _timeScaleHandler;

        private GameStateHandler()
        {
            SetGameState(GameState.Launch);
            _timeScaleHandler = TimeScaleHandler.Instance;
        }

        public void SetGameState(GameState newState)
        {
            CurrentState = newState;
            ChangeGameState?.Invoke(CurrentState);
            if (newState != GameState.Settings) CurrentInGameState = newState;
            TimeScaleHandler.ChangeTimeScale(newState == GameState.InGame ? 1f : 0f);
        }
    }
}
