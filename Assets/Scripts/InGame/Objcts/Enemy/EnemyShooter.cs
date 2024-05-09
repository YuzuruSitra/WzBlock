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
    private int _maxThreeBlock = 0;
    [SerializeField]
    private BulletPool _bulletPool;
    private int _threeBlockCount;
    private int _rayHitCount = 0;
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
        // î≠ñCèàóù
        if (IsBlocking()) 
        {   
            _threeBlockCount ++;
            _bulletCount = 0;
        }
        if (_threeBlockCount <= _maxThreeBlock) return;
        if (MAX_BULLET_COUNT <= _bulletCount) return;
        _bulletPool.GetBullet(transform.position + _insOffSet);
        _bulletCount ++;
        _threeBlockCount = 0;
        
    }

    private void ChangeStateShooter(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        _threeBlockCount = -1;
        _rayHitCount = 0;
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
        if (hit) 
            _rayHitCount ++;
        else 
            _rayHitCount = 0;
        if (_rayHitCount > 1) return false;
        return hit;
    }

}
