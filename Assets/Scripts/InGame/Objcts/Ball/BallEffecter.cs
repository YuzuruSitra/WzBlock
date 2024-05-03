using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffecter : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField]
    private GameObject _effect;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ���W�b�h�{�f�B�̑��x���擾
        Vector3 velocity = _rigidBody.velocity;

        // ���x���[���ɋ߂��Ȃ����m�F
        if (velocity.sqrMagnitude > 0.01f)
        {
            // XY���ʏ�ł̐i�s��������p�x���v�Z
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // �ڕW��Z����]���쐬
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // �V������]�ɃX���[�Y�ɕ��
            _effect.transform.rotation = Quaternion.Lerp(_effect.transform.rotation, targetRotation, Time.deltaTime * 50f);
        }
    }
}
