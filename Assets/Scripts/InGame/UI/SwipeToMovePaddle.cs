using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeToMovePaddle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public PaddleMover _paddleMover; // 動かす対象の3Dオブジェクト
    private SensiHandler _sensiHandler;
    private float[] _speedFactor = { -0.2f, -0.16f, -0.12f, -0.08f, -0.04f , 0 , 0.04f, 0.08f, 0.12f, 0.16f, 0.2f };
    private const float MOVE_SPEED_BASE = 0.2f;
    private Vector2 _startTouchPosition;
    private Vector2 _currentTouchPosition;
    private Vector2 _touchDelta;

    void Start()
    {
        _sensiHandler = SensiHandler.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // タッチが始まった位置を記録
        _startTouchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // タッチが離れた時の処理
        // 必要ならここに処理を追加
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 現在のタッチ位置を更新
        _currentTouchPosition = eventData.position;
        
        // タッチの移動量を計算
        _touchDelta = _currentTouchPosition - _startTouchPosition;
        
        // X方向のタッチの移動量に応じて3Dオブジェクトを動かす
        Vector3 movementVector = Vector3.zero;
        movementVector.x = _touchDelta.x;
        MoveObject(movementVector);
        
        // スタート位置を現在の位置に更新
        _startTouchPosition = _currentTouchPosition;
    }

    private void MoveObject(Vector3 vector)
    {    
        // スワイプの移動量に基づいてオブジェクトをX軸方向に動かす
        Vector3 movement = vector * (MOVE_SPEED_BASE + _speedFactor[_sensiHandler.Sensitivity]) * Time.deltaTime;
        _paddleMover.MoveReceive(movement);
    }
}
