using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArea : MonoBehaviour
{
    private GameStateHandler _gameStateHandler;

    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
            _gameStateHandler.SetGameState(GameStateHandler.GameState.FinGame);
    }
}
