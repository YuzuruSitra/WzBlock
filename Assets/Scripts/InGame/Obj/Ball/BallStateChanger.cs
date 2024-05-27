using System;
using UnityEngine;

namespace InGame.Obj.Ball
{
    public class BallStateChanger : MonoBehaviour
    {
        private GameStateHandler _gameStateHandler;

        public void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DestroyArea"))
                _gameStateHandler.SetGameState(GameStateHandler.GameState.FinGame);
        }
    }
}
