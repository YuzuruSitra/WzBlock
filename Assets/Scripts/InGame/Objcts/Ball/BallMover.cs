using UnityEngine;
using System.Collections.Generic;

public class BallMover : MonoBehaviour
{
    private Vector3 _launchPos;
    [Range(1, 10)]
    [SerializeField]
    private float _launchSpeed = 3f;
    [SerializeField]
    private float _minSpeed = 1f;
    [SerializeField]
    private float _maxSpeed = 7f;
    [SerializeField]
    private float _paddleAddForce = 100f;
    [SerializeField]
    private float _hitReduceForce = 50f;
    [SerializeField]
    private float _explotionForce = 175f;
    private Vector2 _launchDirection = new Vector2(1, 0.5f);
    private Rigidbody _rigidBody;
    private GameStateHandler _gameStateHandler;
    private Vector3 _currentVelocity;
    private Vector3 _currentAngular;
    private float _currentSpeed;
    public float CurrentSpeedRatio => Mathf.Clamp(_currentSpeed / _maxSpeed, 0, _maxSpeed);

    private int _hitFrameCount;
    // 基準となる角度
    private List<float> _baseAngles = new List<float> { 0f, 90f, 180f, 270f, 360f };
    Vector3 _randomDirection = Vector3.zero;
    private const int MAX_STACK_COUNT = 5;
    private const float TILT_THRESHOLD = 10;
    [SerializeField]
    private Transform _BottomCenterPos;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateBall;
    }

    private void ChangeStateBall(GameStateHandler.GameState newState)
    {
        switch (newState)
        {
            case GameStateHandler.GameState.InGame:
                _rigidBody.AddForce(_launchDirection.normalized * _launchSpeed * Time.deltaTime, ForceMode.Impulse);
                break;
            case GameStateHandler.GameState.Launch:
                _hitFrameCount = 0;
                transform.position = _launchPos;
                break;
            case GameStateHandler.GameState.FinGame:
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
                _currentSpeed = 0;
                break;
            default:
                _rigidBody.velocity = _currentVelocity;
                _rigidBody.angularVelocity = _currentAngular;
                break;
        }
    }

    private void Update()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        Vector3 velocity = _rigidBody.velocity;
        _currentSpeed = Mathf.Clamp(velocity.magnitude, _minSpeed, _maxSpeed);
        _rigidBody.velocity = velocity.normalized * _currentSpeed;
        _currentVelocity = _rigidBody.velocity;
        _currentAngular = _rigidBody.angularVelocity;
    }

    // 壁反射でのスタック回避
    private void AvoidFrameStack(GameObject hitObj)
    {
        // フレームやパドルでない場合は、カウンターをリセット
        if (!hitObj.CompareTag("Frame") && !hitObj.CompareTag("Paddle"))
        {
            _hitFrameCount = 0;
            return;
        }

        // 速度を取得して傾きを計算
        Vector3 velocity = _rigidBody.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // 基本角度の中に該当する角度があるか確認
        bool isWithinThreshold = IsAngleWithinThreshold(angle);
        // 角度が基準内でない場合は終了
        if (!isWithinThreshold) return;

        // フレームに当たったらカウンターを増加
        if (hitObj.CompareTag("Frame")) _hitFrameCount++;

        // フレームのカウントが限界に達した場合、ランダムな方向にリダイレクト
        if (_hitFrameCount < MAX_STACK_COUNT) return;

        // 力を加える
        _randomDirection.x = Random.Range(-1f, 1f);
        _randomDirection.y = Random.Range(-1f, 1f);
        _randomDirection.Normalize(); 
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.AddForce(_randomDirection * _explotionForce * Time.deltaTime, ForceMode.Impulse);
        _hitFrameCount = 0;
    }

    private bool IsAngleWithinThreshold(float angle)
    {
        foreach (float baseAngle in _baseAngles)
            if (Mathf.Abs(Mathf.Abs(angle) - baseAngle) <= TILT_THRESHOLD)
                return true;
        return false;
    }


    // プレイヤーに当たったときに、跳ね返る方向を変える
    private void ChangeRefrection(GameObject hitObj)
    {
        if (!hitObj.CompareTag("Paddle")) return;
        Vector3 playerPos = hitObj.transform.position;
        Vector3 ballPos = transform.position;
        Vector3 direction = (ballPos - playerPos).normalized;
        _rigidBody.AddForce(direction * _paddleAddForce * Time.deltaTime, ForceMode.Impulse);
    }

    // キューブに当たったときに、減速する
    private void ReduceForce(GameObject hitObj)
    {
        if (!hitObj.CompareTag("Block")) return;
        _rigidBody.AddForce(-_rigidBody.velocity.normalized * _hitReduceForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AvoidFrameStack(collision.gameObject);
        ChangeRefrection(collision.gameObject);
        ReduceForce(collision.gameObject);
    }
}
