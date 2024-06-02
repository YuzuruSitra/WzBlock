using System;
using UnityEngine;

namespace InGame.Obj.Paddle
{
    public class PaddleInfo : MonoBehaviour
    {
        public event Action DoLearnEvent;
        [SerializeField] 
        private float _rayDistance = 1.5f;
        public float RallyTime { get; private set; }
        public int AvoidEnemyBullet { get; private set; }
        private GameObject _currentAvoid;
        public int ReceiveEnemyBullet { get; private set; }

        private void Start()
        {
            RallyTime = 0;
            AvoidEnemyBullet = 0;
            ReceiveEnemyBullet = 0;
        }
        
        public void Update()
        {
            RallyTime += Time.deltaTime;
            AvoidanceJudgment();
        }

        private void AvoidanceJudgment()
        {
            Debug.DrawRay(transform.position, transform.up * _rayDistance, Color.red);
            if (!Physics.Raycast(transform.position, transform.up, out var hit, _rayDistance)) return;
            if (!hit.collider.CompareTag("EnemyBullet")) return;
            if (_currentAvoid == hit.collider.gameObject) return;
            _currentAvoid = hit.collider.gameObject;
            AvoidEnemyBullet ++;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("EnemyBullet"))
            {
                ReceiveEnemyBullet++;
            }
            if (other.gameObject.CompareTag("Ball"))
            {
                DoLearnEvent?.Invoke();
                RallyTime = 0;
                AvoidEnemyBullet = 0;
                ReceiveEnemyBullet = 0;
            }
        }

    }
}
