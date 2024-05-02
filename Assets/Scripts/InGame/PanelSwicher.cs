using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwicher : MonoBehaviour
{
    [SerializeField]
    private GameObject _launchPanel;
    [SerializeField]
    private GameObject _inGamePanel;
    [SerializeField]
    private GameObject _finPanel;
    private GameObject _currentPanel;

    void Start()
    {
        GameStateHandler.Instance.ChangeGameState += ChangePanel;
    }

    private void ChangePanel(GameStateHandler.GameState newState)
    {
        if (_currentPanel != null) _currentPanel.SetActive(false);
        switch (newState)
        {
            case GameStateHandler.GameState.Launch:
                _launchPanel.SetActive(true);
                _currentPanel = _launchPanel;
                break;
            case GameStateHandler.GameState.InGame:
                _inGamePanel.SetActive(true);
                _currentPanel = _inGamePanel;
                break;
            case GameStateHandler.GameState.FinGame:
                _finPanel.SetActive(true);
                _currentPanel = _finPanel;
                break;
        }
    }
}
