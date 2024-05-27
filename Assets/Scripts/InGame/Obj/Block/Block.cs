using System;
using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Block
{
    public class Block : MonoBehaviour
    {
        public event Action<Block> OnReturnToPool;

        [SerializeField] 
        private int _score;
        private ScoreHandler _scoreHandler;
        [SerializeField] private GameObject _hitEffect;
        private ParticleSystem _ps;
        private WaitForSeconds _wait;
        [SerializeField] 
        private BoxCollider _col;
        [SerializeField] 
        private MeshRenderer _mesh;


        private void Start()
        {
            _scoreHandler = ScoreHandler.Instance;
            _ps = _hitEffect.GetComponent<ParticleSystem>();

            _wait = new WaitForSeconds(_ps.main.duration);
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
            OnReturnToPool?.Invoke(this);
            _hitEffect.SetActive(true);
            yield return _wait;
            _hitEffect.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
                HitBall();
        }
    }
}
