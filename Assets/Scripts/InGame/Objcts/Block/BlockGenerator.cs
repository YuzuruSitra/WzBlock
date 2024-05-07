using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockPool _blockPool;
    [SerializeField]
    private float _insInterval = 7.5f;
    private float _currentInsTime;
    [SerializeField]
    private int _insMaxCount = 5;
    private GameStateHandler _gameStateHandler;
    
    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ReStartIns;
    }

    void Update()
    {
        InsCountDown();
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
        if (_blockPool.AvailableBlocksCount <= 0) return;
        _currentInsTime += Time.deltaTime;
        if (_currentInsTime <= _insInterval) return;
        PeriodicSpawne();
        _currentInsTime = 0;
    }

    private void PeriodicSpawne()
    {
        
        int insCount = Random.Range(1, _insMaxCount);
        int clampedValue = Mathf.Clamp(insCount, 1, _blockPool.AvailableBlocksCount);
        for (int i = 0; i < insCount; i++)
            if (_blockPool.GetBlock() == null) return;
    }

}
