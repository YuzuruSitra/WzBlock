using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public event Action<Bullet> OnReturnToPool;
    private bool _isActive;
    // [SerializeField]
    // private GameObject _hitEffect;
    // private ParticleSystem _ps;
    // private WaitForSeconds _wait;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private GameObject _prjEffect;
    [SerializeField]
    private BoxCollider _col;

    void Start()
    {
        // _scoreHandler = ScoreHandler.Instance;
        // _ps = _hitEffect.GetComponent<ParticleSystem>();

        // _wait = new WaitForSeconds(_ps.main.duration);
    }

    void Update()
    {
        if (!_isActive) return;
        transform.position += Vector3.down * _speed * Time.deltaTime;
    }

    public void ChangeLookActive(bool newActive)
    {
        _col.enabled = newActive;
        _prjEffect.SetActive(newActive);
        _isActive = newActive;
    }

    private void HitDestroy()
    {
        StartCoroutine(BreakBulletAnim());
    }

    private IEnumerator BreakBulletAnim()
    {
        OnReturnToPool?.Invoke(this);
        yield return null;
        //_hitEffect.SetActive(true);
        //yield return _wait;
        //_hitEffect.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestroyArea"))
            HitDestroy();
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Frame")
            || collision.gameObject.CompareTag("Block")
            || collision.gameObject.CompareTag("Ball"))
            HitDestroy();
    }
}
