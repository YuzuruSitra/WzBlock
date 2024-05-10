using UnityEngine;
using UnityEngine.UI;

public class InGameBtHandler : MonoBehaviour
{
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private Button _reStartBt;
    [SerializeField]
    private Button _StartBt;

    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _reStartBt.onClick.AddListener(SetLaunchState);
        _StartBt.onClick.AddListener(StartGame);
    }

    private void SetLaunchState()
    {
        _gameStateHandler.SetGameState(GameStateHandler.GameState.Launch);
    }

    private void StartGame()
    {
        _gameStateHandler.SetGameState(GameStateHandler.GameState.InGame);
    }
}
