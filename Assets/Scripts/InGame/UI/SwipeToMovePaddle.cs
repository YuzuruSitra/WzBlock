using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeToMovePaddle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public PaddleMover _paddleMover; // �������Ώۂ�3D�I�u�W�F�N�g
    [SerializeField]
    private int _sensitivity = 5;  // �ړ����x
    private float[] _speedFactor = { -0.2f, -0.15f, -0.1f, -0.05f , 0 , 0.05f, 0.1f, 0.15f, 0.2f };
    private const float MOVE_SPEED_BASE = 0.25f;
    private Vector2 _startTouchPosition;
    private Vector2 _currentTouchPosition;
    private Vector2 _touchDelta;

    public void OnPointerDown(PointerEventData eventData)
    {
        // �^�b�`���n�܂����ʒu���L�^
        _startTouchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // �^�b�`�����ꂽ���̏���
        // �K�v�Ȃ炱���ɏ�����ǉ�
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���݂̃^�b�`�ʒu���X�V
        _currentTouchPosition = eventData.position;
        
        // �^�b�`�̈ړ��ʂ��v�Z
        _touchDelta = _currentTouchPosition - _startTouchPosition;
        
        // X�����̃^�b�`�̈ړ��ʂɉ�����3D�I�u�W�F�N�g�𓮂���
        Vector3 movementVector = Vector3.zero;
        movementVector.x = _touchDelta.x;
        MoveObject(movementVector);
        
        // �X�^�[�g�ʒu�����݂̈ʒu�ɍX�V
        _startTouchPosition = _currentTouchPosition;
    }

    private void MoveObject(Vector3 vector)
    {    
        // �X���C�v�̈ړ��ʂɊ�Â��ăI�u�W�F�N�g��X�������ɓ�����
        Vector3 movement = vector * (MOVE_SPEED_BASE + _speedFactor[_sensitivity - 1]) * Time.deltaTime;
        _paddleMover.MoveReceive(movement);
    }

    public void ChangeSpeedFactor(int newSensi)
    {
        _sensitivity = newSensi;
    }
}
