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
        // �����ʒu��ێ�
        _initPosition = transform.position;
    }

    public void StartShake(float _powerFactor)
    {
        // �O��̏������c���Ă���Β�~���ď����ʒu�ɖ߂�
        if (_shakeTweener != null)
        {
            _shakeTweener.Kill();
            gameObject.transform.position = _initPosition;
        }
        // �h��J�n
        _shakeTweener = gameObject.transform.DOShakePosition(_duration, _strength * _powerFactor, _vibrato, _randomness, _fadeOut);
    }
}
