using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandlerPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform _uiElement; // 追従させるUI要素
    private Vector2 _offset; // オフセット
    private bool _isDragging = false; // ドラッグ中かどうかのフラグ
    private float _initialY; // 固定するY座標
    private RectTransform _parentRect; // 制限範囲の親オブジェクト
    [Header("戻る時間")]
    [SerializeField]
    private float _smoothTime = 0.3f; // スムーズに戻すための時間
    public float GetRelativePosition => CalcRelativePosition();

    void Start()
    {
        _uiElement = GetComponent<RectTransform>();
        _parentRect = transform.parent.GetComponent<RectTransform>();
        _initialY = _uiElement.anchoredPosition.y; // Y軸の初期値
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;

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
        StartCoroutine(SmoothReturnToCenter());
    }

    private IEnumerator SmoothReturnToCenter()
    {
        // 親の中心座標を計算
        float halfWidth = _uiElement.rect.width / 2f + 20.0f;
        Rect parentBounds = _parentRect.rect;
        float centerX = (parentBounds.xMin + parentBounds.xMax) / 2f;

        // スムーズに移動するためのタイミング
        float elapsedTime = 0f;
        Vector2 startPosition = _uiElement.anchoredPosition;

        while (elapsedTime < _smoothTime)
        {
            // 中心座標へスムーズに補間
            float t = elapsedTime / _smoothTime;
            Vector2 newPosition = Vector2.Lerp(startPosition, new Vector2(centerX, _initialY), t);

            _uiElement.anchoredPosition = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null; // フレームごとに更新
        }

        // 最後に完全に中央に配置
        _uiElement.anchoredPosition = new Vector2(centerX, _initialY);
    }

    private float CalcRelativePosition()
    {
        float halfWidth = _uiElement.rect.width / 2f + 20.0f; // UI要素の半分の幅
        Rect parentBounds = _parentRect.rect;

        float totalWidth = parentBounds.width - halfWidth * 2;
        float relativePosition = (_uiElement.anchoredPosition.x - parentBounds.xMin - halfWidth) / totalWidth * 2 - 1;

        return relativePosition;
    }
}
