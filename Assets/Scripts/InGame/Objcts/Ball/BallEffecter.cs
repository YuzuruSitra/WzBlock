using UnityEngine;

public class BallEffecter : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField]
    private GameObject _effect;
    [SerializeField]
    private GameObject[] _changeSizeEffects;
    private Vector3[] _originSizeEffects;
    [SerializeField]
    private BallMover _ballMover;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _originSizeEffects = new Vector3[_changeSizeEffects.Length];
        for (int i = 0; i < _changeSizeEffects.Length; i++)
            _originSizeEffects[i] = _changeSizeEffects[i].transform.localScale;
    }

    void Update()
    {
        // �G�t�F�N�g�T�C�Y�̕ύX
        for (int i = 0; i < _changeSizeEffects.Length; i++)
            _changeSizeEffects[i].transform.localScale = _originSizeEffects[i] * _ballMover.CurrentSpeedRatio;

        // ���W�b�h�{�f�B�̑��x���擾
        Vector3 velocity = _rigidBody.velocity;

        // XY���ʏ�ł̐i�s��������p�x���v�Z
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // �ڕW��Z����]���쐬
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // �V������]�ɃX���[�Y�ɕ��
        _effect.transform.rotation = Quaternion.Lerp(_effect.transform.rotation, targetRotation, Time.deltaTime * 50f);
    }
}
