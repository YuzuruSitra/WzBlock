using DG.Tweening;
using UnityEngine;

public class ShakeByDOTween : MonoBehaviour
{
    [SerializeField]
    private float _duration, _strength, _randomness;
    [SerializeField]
    private int _vibrato;
    [SerializeField]
    private bool _fadeOut;

    private Tweener _shakeTweener;
    private Vector3 _initPosition;

    private void Start()
    {
        // 初期位置を保持
        _initPosition = transform.position;
    }

    public void StartShake(float _powerFactor)
    {
        // 前回の処理が残っていれば停止して初期位置に戻す
        if (_shakeTweener != null)
        {
            _shakeTweener.Kill();
            gameObject.transform.position = _initPosition;
        }
        // 揺れ開始
        _shakeTweener = gameObject.transform.DOShakePosition(_duration, _strength * _powerFactor, _vibrato, _randomness, _fadeOut);
    }
}
