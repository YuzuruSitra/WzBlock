using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandlerPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private GameStateHandler _gameStateHandler;
    private RectTransform _uiElement; // 追従させるUI要素
    private Vector2 _offset; // オフセット
    private bool _isDragging = false; // ドラッグ中かどうかのフラグ
    private float _initialY; // 固定するY座標
    private Vector2 _centerPos;
    private RectTransform _parentRect; // 制限範囲の親オブジェクト
    [Header("戻る時間")]
    [SerializeField]
    private float _smoothTime = 0.3f; // スムーズに戻すための時間
    public float GetRelativePosition => CalcRelativePosition();
    private Coroutine _returnCoroutine;

    void Start()
    {
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePad;
        _uiElement = GetComponent<RectTransform>();
        _parentRect = transform.parent.GetComponent<RectTransform>();
        _initialY = _uiElement.anchoredPosition.y; // Y軸の初期値
        Rect parentBounds = _parentRect.rect;
        float centerX = (parentBounds.xMin + parentBounds.xMax) / 2f;
        _centerPos = new Vector2(centerX, _initialY);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        _isDragging = true;
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        ))
        {
            _offset = localPoint - _uiElement.anchoredPosition; // 位置とUI要素の差を計算
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            ))
            {
                Vector2 newPosition = localPoint - _offset;
                newPosition.y = _initialY; // Y軸は固定

                float halfWidth = _uiElement.rect.width / 2f + 20.0f; // UI要素の半分の幅
                Rect parentBounds = _parentRect.rect;

                newPosition.x = Mathf.Clamp(
                    newPosition.x,
                    parentBounds.xMin + halfWidth,
                    parentBounds.xMax - halfWidth
                );

                _uiElement.anchoredPosition = newPosition;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;

        // コルーチンを開始して中央に戻るアニメーションを実行
        _returnCoroutine = StartCoroutine(SmoothReturnToCenter());
    }

    private IEnumerator SmoothReturnToCenter()
    {
        // スムーズに移動するためのタイミング
        float elapsedTime = 0f;
        Vector2 startPosition = _uiElement.anchoredPosition;

        while (elapsedTime < _smoothTime)
        {
            // 中心座標へスムーズに補間
            float t = elapsedTime / _smoothTime;
            Vector2 newPosition = Vector2.Lerp(startPosition, _centerPos, t);

            _uiElement.anchoredPosition = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null; // フレームごとに更新
        }

        // 最後に完全に中央に配置
        _uiElement.anchoredPosition = _centerPos;
        _returnCoroutine = null;
    }

    private float CalcRelativePosition()
    {
        float halfWidth = _uiElement.rect.width / 2f + 20.0f; // UI要素の半分の幅
        Rect parentBounds = _parentRect.rect;

        float totalWidth = parentBounds.width - halfWidth * 2;
        float relativePosition = (_uiElement.anchoredPosition.x - parentBounds.xMin - halfWidth) / totalWidth * 2 - 1;

        return relativePosition;
    }

    private void ChangeStatePad(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.Launch) return;
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }
        _uiElement.anchoredPosition = _centerPos;
    }

}
