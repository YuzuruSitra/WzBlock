using UnityEngine;

public class BallStateChanger : MonoBehaviour
{
    private GameStateHandler _gameStateHandler;

    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestroyArea"))
            _gameStateHandler.SetGameState(GameStateHandler.GameState.FinGame);
    }
}
