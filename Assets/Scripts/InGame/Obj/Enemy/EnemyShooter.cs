using System;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class EnemyShooter : MonoBehaviour
    {
        [SerializeField]
        private EnemySurviveManager _enemySurviveManager;
        private GameStateHandler _gameStateHandler;
        [SerializeField]
        private GameObject _bullet;
        private readonly Vector3 _insOffSet = new Vector3(0, -0.45f, 0);
        [SerializeField]
        private Vector3 _rayOffSet;
        [SerializeField]
        private float _maxThreeTime = 5.0f;

        [SerializeField] 
        private MetaAIManipulator _metaAIManipulator;
        private readonly float[] _boredomScaleFactor = { 0.55f, 0.65f, 0.75f, 0.85f, 0.95f, 1.05f, 1.15f, 1.25f, 1.35f, 1.45f};
        private float _threeBlockTime;
        [SerializeField]
        private BulletPool _bulletPool;
        private const int MaxBulletCount = 1;
        private int _bulletCount;

        private void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateShooter;
        }

        public void Update()
        {
            if (!_enemySurviveManager.IsActive) return;
            if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
            if (_gameStateHandler.CurrentState == GameStateHandler.GameState.Settings) return;
            if (IsBlocking()) 
            {   
                _bulletCount = 0;
                return;
            }
            float factor = _boredomScaleFactor[_metaAIManipulator.CurrentBoredomLevel];
            _threeBlockTime += Time.deltaTime * factor;
            if (_threeBlockTime <= _maxThreeTime) return;
            if (MaxBulletCount <= _bulletCount) return;
            _bulletPool.GetBullet(transform.position + _insOffSet);
            _bulletCount ++;
            _threeBlockTime = 0;
        
        }

        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStateShooter;
        }

        private void ChangeStateShooter(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            _threeBlockTime = 0;
            _bulletCount = 0;
            _bulletPool.ReturnAllBlock();
        }

        private bool IsBlocking()
        {
            var hit = false;
            Debug.DrawRay(transform.position + _rayOffSet, Vector3.down, Color.red);
            Debug.DrawRay(transform.position - _rayOffSet, Vector3.down, Color.red);
            if (Physics.Raycast(transform.position + _rayOffSet, Vector3.down, out RaycastHit hit1, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
                hit = true;
            
            if (Physics.Raycast(transform.position - _rayOffSet, Vector3.down, out RaycastHit hit2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
                hit = true;
            return hit;
        }

    }
}
