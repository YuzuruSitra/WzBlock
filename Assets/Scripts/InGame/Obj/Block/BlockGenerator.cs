using UnityEngine;

namespace InGame.Obj.Block
{
    public class BlockGenerator : MonoBehaviour
    {
        [SerializeField] 
        private Transform _ball;
        [SerializeField] 
        private BlockPool _blockPool;
        [SerializeField] 
        private float _insInterval = 7.5f;
        private float _currentInsTime;
        [SerializeField] 
        private int _insMaxCount = 5;
        private GameStateHandler _gameStateHandler;

        public void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ReStartIns;
        }

        private void Update()
        {
            InsCountDown();
        }

        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ReStartIns;
        }

        private void ReStartIns(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            _currentInsTime = 0;
            _blockPool.AllGetPool();
        }

        private void InsCountDown()
        {
            if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
            if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
            if (_blockPool.AvailableBlocksCount <= 0) return;
            if (_ball.position.y >= transform.position.y) return;
            _currentInsTime += Time.deltaTime * _blockPool.AvailableBlocksCount;
            if (_currentInsTime <= _insInterval) return;
            PeriodicSpawn();
            _currentInsTime = 0;
        }

        private void PeriodicSpawn()
        {
            var insCount = Random.Range(3, _insMaxCount);
            var clampedValue = Mathf.Clamp(insCount, 3, _blockPool.AvailableBlocksCount);
            for (var i = 0; i < clampedValue; i++)
                if (_blockPool.GetBlock() is null)
                    return;
        }

    }
}

