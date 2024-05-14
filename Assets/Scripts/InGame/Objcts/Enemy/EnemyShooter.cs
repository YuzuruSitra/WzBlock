using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField]
    private EnemySurviveManager _enemySurviveManager;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private GameObject _bullet;
    private Vector3 _insOffSet = new Vector3(0, -0.45f, 0);
    [SerializeField]
    private Vector3 _rayOffSet;
    [SerializeField]
    private float _maxThreeTime = 5.0f;
    private float _threeBlockTime;
    [SerializeField]
    private BulletPool _bulletPool;
    private const int MAX_BULLET_COUNT = 1;
    private int _bulletCount;
    
    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateShooter;
    }

    void Update()
    {
        if (!_enemySurviveManager.IsActive) return;
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
        // î≠ñCèàóù
        if (IsBlocking()) 
        {   
            _bulletCount = 0;
            return;
        }
        _threeBlockTime += Time.deltaTime;
        if (_threeBlockTime <= _maxThreeTime) return;
        if (MAX_BULLET_COUNT <= _bulletCount) return;
        _bulletPool.GetBullet(transform.position + _insOffSet);
        _bulletCount ++;
        _threeBlockTime = 0;
        
    }

    private void ChangeStateShooter(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        _threeBlockTime = 0;
        _bulletCount = 0;
        _bulletPool.ReturnALLBlock();
    }

    private bool IsBlocking()
    {
        bool hit = false;
        Debug.DrawRay(transform.position + _rayOffSet, Vector3.down, Color.red);
        Debug.DrawRay(transform.position - _rayOffSet, Vector3.down, Color.red);
        if (Physics.Raycast(transform.position + _rayOffSet, Vector3.down, out RaycastHit hit1, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
                hit = true;
            
        if (Physics.Raycast(transform.position - _rayOffSet, Vector3.down, out RaycastHit hit2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
                hit = true;
        return hit;
    }

}
