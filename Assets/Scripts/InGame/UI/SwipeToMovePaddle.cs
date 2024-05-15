using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeToMovePaddle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public PaddleMover _paddleMover; // �������Ώۂ�3D�I�u�W�F�N�g
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
        Vector3 movement = vector * (MOVE_SPEED_BASE + _speedFactor[_sensiHandler.Sensitivity]) * Time.deltaTime;
        _paddleMover.MoveReceive(movement);
    }
}
