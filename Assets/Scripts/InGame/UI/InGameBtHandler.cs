using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InGame.UI
{
    public class InGameBtHandler : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;
        [SerializeField]
        private Button _reStartBt;
        [SerializeField]
        private Button _startBt;
        [SerializeField]
        private Button _headerSettingBt;
        [SerializeField]
        private Button _settingResumeBt;
        [SerializeField]
        private Button _settingHomeBt;
        [SerializeField]
        private SettingPanelHandler _settingPanelHandler;

        private void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _reStartBt.onClick.AddListener(SetLaunchState);
            _startBt.onClick.AddListener(StartGame);
            _headerSettingBt.onClick.AddListener(SetSettingState);
            _settingResumeBt.onClick.AddListener(SetCurrentInGameState);
            _settingResumeBt.onClick.AddListener(_settingPanelHandler.CloseFixedAction);
            _settingHomeBt.onClick.AddListener(SceneHandler.Instance.GoTitleScene);
        }

        private void SetLaunchState()
        {
            _gameStateHandler.SetGameState(GameStateHandler.GameState.Launch);
        }

        private void StartGame()
        {
            _gameStateHandler.SetGameState(GameStateHandler.GameState.InGame);
        }

        private void SetSettingState()
        {
            _gameStateHandler.SetGameState(GameStateHandler.GameState.Settings);
        }
        private void SetCurrentInGameState()
        {
            _gameStateHandler.SetGameState(_gameStateHandler.CurrentInGameState);
        }
    }
}
