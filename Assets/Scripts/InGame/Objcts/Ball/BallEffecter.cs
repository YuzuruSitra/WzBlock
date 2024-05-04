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
        // エフェクトサイズの変更
        for (int i = 0; i < _changeSizeEffects.Length; i++)
            _changeSizeEffects[i].transform.localScale = _originSizeEffects[i] * _ballMover.CurrentSpeedRatio;

        // リジッドボディの速度を取得
        Vector3 velocity = _rigidBody.velocity;

        // XY平面上での進行方向から角度を計算
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // 目標のZ軸回転を作成
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // 新しい回転にスムーズに補間
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
