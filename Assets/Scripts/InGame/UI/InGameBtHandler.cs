using UnityEngine;
using UnityEngine.UI;

public class InGameBtHandler : MonoBehaviour
{
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private Button _reStartBt;

    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _reStartBt.onClick.AddListener(SetLaunchState);
    }

    private void SetLaunchState()
    {
        _gameStateHandler.SetGameState(GameStateHandler.GameState.Launch);
    }
}
