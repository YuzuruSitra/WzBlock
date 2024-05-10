using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandlerPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform _uiElement; // �Ǐ]������UI�v�f
    private Vector2 _offset; // �I�t�Z�b�g
    private bool _isDragging = false; // �h���b�O�����ǂ����̃t���O
    private float _initialY; // �Œ肷��Y���W
    private RectTransform _parentRect; // �����͈͂̐e�I�u�W�F�N�g
    [Header("�߂鎞��")]
    [SerializeField]
    private float _smoothTime = 0.3f; // �X���[�Y�ɖ߂����߂̎���
    public float GetRelativePosition => CalcRelativePosition();

    void Start()
    {
        _uiElement = GetComponent<RectTransform>();
        _parentRect = transform.parent.GetComponent<RectTransform>();
        _initialY = _uiElement.anchoredPosition.y; // Y���̏����l
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
            _offset = localPoint - _uiElement.anchoredPosition; // �ʒu��UI�v�f�̍����v�Z
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
                newPosition.y = _initialY; // Y���͌Œ�

                float halfWidth = _uiElement.rect.width / 2f + 20.0f; // UI�v�f�̔����̕�
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

        // �R���[�`�����J�n���Ē����ɖ߂�A�j���[�V���������s
        StartCoroutine(SmoothReturnToCenter());
    }

    private IEnumerator SmoothReturnToCenter()
    {
        // �e�̒��S���W���v�Z
        float halfWidth = _uiElement.rect.width / 2f + 20.0f;
        Rect parentBounds = _parentRect.rect;
        float centerX = (parentBounds.xMin + parentBounds.xMax) / 2f;

        // �X���[�Y�Ɉړ����邽�߂̃^�C�~���O
        float elapsedTime = 0f;
        Vector2 startPosition = _uiElement.anchoredPosition;

        while (elapsedTime < _smoothTime)
        {
            // ���S���W�փX���[�Y�ɕ��
            float t = elapsedTime / _smoothTime;
            Vector2 newPosition = Vector2.Lerp(startPosition, new Vector2(centerX, _initialY), t);

            _uiElement.anchoredPosition = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null; // �t���[�����ƂɍX�V
        }

        // �Ō�Ɋ��S�ɒ����ɔz�u
        _uiElement.anchoredPosition = new Vector2(centerX, _initialY);
    }

    private float CalcRelativePosition()
    {
        float halfWidth = _uiElement.rect.width / 2f + 20.0f; // UI�v�f�̔����̕�
        Rect parentBounds = _parentRect.rect;

        float totalWidth = parentBounds.width - halfWidth * 2;
        float relativePosition = (_uiElement.anchoredPosition.x - parentBounds.xMin - halfWidth) / totalWidth * 2 - 1;

        return relativePosition;
    }
}
