using UnityEngine;
using System.Collections;

public class BallEffecter : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField]
    private GameObject _prjEffect;
    [SerializeField]
    private GameObject[] _changeSizeEffects;
    private Vector3[] _originSizeEffects;
    [SerializeField]
    private GameObject _explosionEffect;
    private WaitForSeconds _explosionEffectDurtion;
    [SerializeField]
    private BallMover _ballMover;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _explosionEffectDurtion = new WaitForSeconds(_explosionEffect.GetComponent<ParticleSystem>().main.duration);
        _ballMover.ExplotionEvent += LaunchExplotion;
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
        _prjEffect.transform.rotation = Quaternion.Lerp(_prjEffect.transform.rotation, targetRotation, Time.deltaTime * 50f);
    }

    private void LaunchExplotion()
    {
        StartCoroutine(ExplotionAnim());
    }

    private IEnumerator ExplotionAnim()
    {
        _explosionEffect.SetActive(true);
        yield return _explosionEffectDurtion;
        _explosionEffect.SetActive(false);
    }

}
