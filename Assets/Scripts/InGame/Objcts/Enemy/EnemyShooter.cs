using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    private Vector3 _insOffSet = new Vector3(0, -0.4f, 0);
    [SerializeField]
    private Vector3 _rayOffSet;
    [SerializeField]
    private float _rayDistance;
    [SerializeField]
    private int _maxThreeBlock = 0;
    private int _threeBlockCount;
    private int _rayHitCount = 0;
    private const int MAX_BULLET_COUNT = 1;
    private int _bulletCount;
    void Start()
    {
        _threeBlockCount = 0;
        _rayHitCount = 0;
        _bulletCount = 0;
    }

    void Update()
    {
        if (IsBlocking()) 
        {   
            _threeBlockCount ++;
            _bulletCount = 0;
        }
        if (_threeBlockCount <= _maxThreeBlock) return;
        if (MAX_BULLET_COUNT <= _bulletCount) return;
        Instantiate(_bullet, transform.position + _insOffSet, Quaternion.identity);
        _bulletCount ++;
        _threeBlockCount = 0;
        
    }

    private bool IsBlocking()
    {
        bool hit = false;
        Debug.DrawRay(transform.position + _rayOffSet, Vector3.down * _rayDistance, Color.red);
        Debug.DrawRay(transform.position - _rayOffSet, Vector3.down * _rayDistance, Color.red);
        if (Physics.Raycast(transform.position + _rayOffSet, Vector3.down, out RaycastHit hit1, _rayDistance))
            if ((hit1.collider.CompareTag("Block")))
                hit = true;
            
        if (Physics.Raycast(transform.position - _rayOffSet, Vector3.down, out RaycastHit hit2, _rayDistance))
            if ((hit2.collider.CompareTag("Block")))
                hit = true;
        if (hit) 
            _rayHitCount ++;
        else 
            _rayHitCount = 0;
        if (_rayHitCount > 1) return false;
        return hit;
    }

}
