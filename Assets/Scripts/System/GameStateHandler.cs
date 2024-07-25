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

        private GameStateHandler()
        {
            Application.targetFrameRate = 60;
            SetGameState(GameState.Launch);
        }

        public void SetGameState(GameState newState)
        {
            CurrentState = newState;
            ChangeGameState?.Invoke(CurrentState);
            if (newState != GameState.Settings) CurrentInGameState = newState;
            Time.timeScale = newState == GameState.InGame ? 1f : 0f;
        }
    }
}
