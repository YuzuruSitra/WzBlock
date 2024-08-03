using System;
using InGame.InGameSystem;
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
        private ComboCounter _comboCounter;
        public int LastCombo { get; private set; }
        private GameStateHandler _gameStateHandler;
        
        private void Start()
        {
            RallyTime = 0;
            AvoidEnemyBullet = 0;
            _comboCounter = ComboCounter.Instance;
            _gameStateHandler = GameStateHandler.Instance;
            _comboCounter.ChangeComboCount += ComboUpdate;
        }
        
        public void Update()
        {
            if (_gameStateHandler.CurrentInGameState != GameStateHandler.GameState.InGame) return;
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

        private void ComboUpdate(int count)
        {
            if (count == 0) return;
            LastCombo = (LastCombo < count) ? count : LastCombo;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Ball")) return;
            DoLearnEvent?.Invoke();
            RallyTime = 0;
            AvoidEnemyBullet = 0;
            LastCombo = 0;
        }

    }
}
