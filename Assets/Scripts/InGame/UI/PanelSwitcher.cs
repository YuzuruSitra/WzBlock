using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace InGame.UI
{
    public class PanelSwitcher : MonoBehaviour
    {
        [SerializeField]
        private GameObject _launchPanel;
        [SerializeField]
        private GameObject _inGamePanel;
        [SerializeField]
        private GameObject _finPanel;
        [SerializeField]
        private GameObject _headerPanel;
        [SerializeField]
        private GameObject _settingsPanel;
        private GameObject _currentPanel;

        private void Start()
        {
            GameStateHandler.Instance.ChangeGameState += ChangePanel;
            _currentPanel = _launchPanel;
            _inGamePanel.SetActive(false);
            _finPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            GameStateHandler.Instance.ChangeGameState -= ChangePanel;
        }

        private void ChangePanel(GameStateHandler.GameState newState)
        {
            if (_currentPanel != null) _currentPanel.SetActive(false);
            switch (newState)
            {
                case GameStateHandler.GameState.Launch:
                    _launchPanel.SetActive(true);
                    _headerPanel.SetActive(true);
                    _currentPanel = _launchPanel;
                    break;
                case GameStateHandler.GameState.InGame:
                    _inGamePanel.SetActive(true);
                    _currentPanel = _inGamePanel;
                    break;
                case GameStateHandler.GameState.FinGame:
                    _finPanel.SetActive(true);
                    _headerPanel.SetActive(false);
                    _currentPanel = _finPanel;
                    break;
                case GameStateHandler.GameState.Settings:
                    _settingsPanel.SetActive(true);            
                    _currentPanel = _settingsPanel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

    }
}
