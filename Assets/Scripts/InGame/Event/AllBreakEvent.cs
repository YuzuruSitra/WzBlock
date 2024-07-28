using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InGame.InGameSystem;
using InGame.Obj.Block;
using UnityEngine;

namespace InGame.Event
{
    public class AllBreakEvent : MonoBehaviour
    {
        [SerializeField] private BlockPool _blockPool;
        private Coroutine _breakCoroutine;
        private GameStateHandler _gameStateHandler;
        private TimeScaleHandler _timeScaleHandler;
        [Header("Transition time on TimeScale")]
        [SerializeField] private float _durationTime;
        [Header("TimeScale on breaking")]
        [SerializeField] private float _targetTimeScale;
        [Header("1Block break time")]
        [SerializeField] private float _breakTime;
        [SerializeField] private float _shakePower;
        private WaitForSecondsRealtime _waitForSecondsRealtime;
        [SerializeField] private ShakeByDOTween _shakeByDoTween;
        public bool IsBreaking { get; private set; }
        
        private void Start()
        {
            _gameStateHandler = GameStateHandler.Instance;
            _timeScaleHandler = TimeScaleHandler.Instance;
            _gameStateHandler.ChangeGameState += StopBreak;
            _waitForSecondsRealtime = new WaitForSecondsRealtime(_breakTime);
            IsBreaking = false;
        }
        
        private void OnDestroy()
        {
            _gameStateHandler.ChangeGameState -= StopBreak;
        }

        private void StopBreak(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.FinGame) return;
            if (_breakCoroutine == null) return;
            StopCoroutine(_breakCoroutine);
            _breakCoroutine = null;
            IsBreaking = false;
        }

        public void DoAllBreak()
        {
            var blockList = _blockPool.UsedBlocks1;
            blockList.AddRange(_blockPool.UsedBlocks2);
            var sortedBlocks = blockList.OrderByDescending(block => block.transform.position.y)
                .ThenBy(block => block.transform.position.x)
                .ToList();
            _breakCoroutine = StartCoroutine(BreakBlocks(sortedBlocks));
        }

        IEnumerator BreakBlocks(List<BlockBase> blockList)
        {
            IsBreaking = true;
            var elapsedTime = 0f;

            while (elapsedTime < _durationTime)
            {
                var tScale = Mathf.Lerp(Time.timeScale, _targetTimeScale, elapsedTime / _durationTime);
                _timeScaleHandler.ChangeTimeScale(tScale);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = _targetTimeScale;
            
            foreach (var block in blockList)
            {
                block.ReceiveBreak();
                _shakeByDoTween.StartShake(_shakePower);
                yield return _waitForSecondsRealtime; 
            }
            _timeScaleHandler.ChangeTimeScale(1f);
            _breakCoroutine = null;
            IsBreaking = false;
        }
    }
}
