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
    private GameObject _paddleHitEffect;
    private GameObject _hitEffect;
    private WaitForSeconds _hitEffectDurtion;

    [SerializeField]
    private GameObject _explosionEffect;
    private GameObject _expEffect;
    private WaitForSeconds _explosionEffectDurtion;
    [SerializeField]
    private BallMover _ballMover;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _hitEffect = Instantiate(_paddleHitEffect);
        _hitEffectDurtion = new WaitForSeconds(_hitEffect.GetComponent<ParticleSystem>().main.duration);
        _ballMover.HitPaddleEvent += LaunchHitEffect;

        _expEffect = Instantiate(_explosionEffect);
        _explosionEffectDurtion = new WaitForSeconds(_expEffect.GetComponent<ParticleSystem>().main.duration);
        _ballMover.ExplotionEvent += LaunchExplotion;
        _originSizeEffects = new Vector3[_changeSizeEffects.Length];
        for (int i = 0; i < _changeSizeEffects.Length; i++)
            _originSizeEffects[i] = _changeSizeEffects[i].transform.localScale;
    }

    void Update()
    {
        // エフェクトサイズの変更
        for (int i = 0; i < _changeSizeEffects.Length; i++)
            _changeSizeEffects[i].transform.localScale = _originSizeEffects[i] * _ballMover.CurrentSpeedRatio;

        // リジッドボディの速度を取得
        Vector3 velocity = _rigidBody.velocity;
        if (velocity.magnitude <= 0)
            for (int i = 0; i < _changeSizeEffects.Length; i++)
            _changeSizeEffects[i].transform.localScale = Vector3.zero;
        // XY平面上での進行方向から角度を計算
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // 目標のZ軸回転を作成
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // 新しい回転にスムーズに補間
        _prjEffect.transform.rotation = Quaternion.Lerp(_prjEffect.transform.rotation, targetRotation, Time.deltaTime * 50f);
    }

    private void LaunchHitEffect()
    {
        StartCoroutine(HitPaddleAnim());
    }

    private IEnumerator HitPaddleAnim()
    {
        _hitEffect.transform.position = transform.position;
        _hitEffect.SetActive(true);
        yield return _hitEffectDurtion;
        _hitEffect.SetActive(false);
    }

    private void LaunchExplotion()
    {
        StartCoroutine(ExplotionAnim());
    }

    private IEnumerator ExplotionAnim()
    {
        _expEffect.transform.position = transform.position;
        _expEffect.SetActive(true);
        yield return _explosionEffectDurtion;
        _expEffect.SetActive(false);
    }

}
