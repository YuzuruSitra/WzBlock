using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private int _score;
    private int _blockID;
    public int BlockID => _blockID;
    private bool _isActive = false;
    public bool IsActive => _isActive;
    private ScoreHandler _scoreHandler;
    [SerializeField]
    private GameObject _hitEffect;
    private ParticleSystem _ps;
    private WaitForSeconds _wait;
    [SerializeField]
    private BoxCollider _col;
    [SerializeField]
    private MeshRenderer _mesh;


    void Start()
    {
        _scoreHandler = ScoreHandler.Instance;
        _ps = _hitEffect.GetComponent<ParticleSystem>();

        _wait = new WaitForSeconds(_ps.main.duration);
    }

    public void SetID(int newID)
    {
        _blockID = newID;
    }

    public void ChangeActive(bool newActive)
    {
        _isActive = newActive;
        gameObject.SetActive(newActive);
        if (!_isActive) return;
        _col.enabled = _isActive;
        _mesh.enabled = _isActive;
    }

    public void ChangeLookActive(bool newActive)
    {
        _col.enabled = newActive;
        _mesh.enabled = newActive;
    }

    private void HitBall()
    {
        _scoreHandler.AddScore(_score);
        StartCoroutine(BreakWallAnim());
    }

    private IEnumerator BreakWallAnim()
    {
        _hitEffect.SetActive(true);
        ChangeLookActive(false);
        yield return _wait;
        _hitEffect.SetActive(false);
        ChangeLookActive(true);
        ChangeActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            HitBall();
    }
}
