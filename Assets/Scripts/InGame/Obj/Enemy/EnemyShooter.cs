using System;
using System.Collections;
using InGame.InGameSystem;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class EnemyShooter : MonoBehaviour
    {
        [SerializeField] private EnemySurviveManager _enemySurviveManager;
        private GameStateHandler _gameStateHandler;
        private readonly Vector3 _insOffSet = new Vector3(0, -0.45f, 0);
        [SerializeField] private Vector3 _rayOffSet;
        [SerializeField] private float _maxThreeTime = 5.0f;

        [SerializeField] private MetaAIManipulator _metaAIManipulator;
        private readonly float[] _boredomScaleFactor = { 0.55f, 0.65f, 0.75f, 0.85f, 0.95f, 1.05f, 1.15f, 1.25f, 1.35f, 1.45f};
        private float _currentScaleFactor;
        [SerializeField] private float _changeScaleDuration;
        private Coroutine _changeScaleCoroutine;
        
        private float _threeBlockTime;
        [SerializeField] private BulletPool _bulletPool;
        private const int MaxBulletCount = 1;
        private int _bulletCount;

        private void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateShooter;
            _metaAIManipulator.ChangeBoredomLevel += ChangeBoredomLevel;
            _currentScaleFactor = _boredomScaleFactor[_metaAIManipulator.InitialBoredomLevel];
        }
        
        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= ChangeStateShooter;
            _metaAIManipulator.ChangeBoredomLevel -= ChangeBoredomLevel;
        }

        public void Update()
        {
            if (_gameStateHandler.CurrentInGameState != GameStateHandler.GameState.InGame) return;
            if (!_enemySurviveManager.IsActive) return;
            if (IsBlocking()) 
            {   
                _bulletCount = 0;
                return;
            }
            var factor = _currentScaleFactor;
            _threeBlockTime += Time.deltaTime * factor;
            if (_threeBlockTime <= _maxThreeTime) return;
            if (MaxBulletCount <= _bulletCount) return;
            var pos = transform.position + _insOffSet;
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = pos;
            bullet.ChangeLookActive(true);
            _bulletCount ++;
            _threeBlockTime = 0;
        
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
            if (Physics.Raycast(transform.position + _rayOffSet, Vector3.down, out _, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
                hit = true;
            
            if (Physics.Raycast(transform.position - _rayOffSet, Vector3.down, out _, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
                hit = true;
            return hit;
        }
        
        private void ChangeBoredomLevel(int level)
        {
            if (_changeScaleCoroutine != null) StopCoroutine(_changeScaleCoroutine);
            _changeScaleCoroutine = StartCoroutine(ChangeScaleFactor(_boredomScaleFactor[level]));
        }

        private IEnumerator ChangeScaleFactor(float targetFactor)
        {
            var startValue = _currentScaleFactor;
            var elapsedTime = 0f;
            
            while (elapsedTime < _changeScaleDuration)
            {
                _currentScaleFactor = Mathf.Lerp(startValue, targetFactor, elapsedTime / _changeScaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _currentScaleFactor = targetFactor;
            _changeScaleCoroutine = null;
        }

    }
}
