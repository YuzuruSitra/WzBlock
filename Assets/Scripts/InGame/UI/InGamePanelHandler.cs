using System;
using System.Collections;
using InGame.InGameSystem;
using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class InGamePanelHandler : MonoBehaviour
    {
        private ScoreHandler _scoreHandler;

        [SerializeField]
        private TMP_Text _scoreText;
        private Coroutine _changeScoreCoroutine = null;
        private int _currentSetScore;
        [SerializeField]
        private float _countUpWaitTime;
        private WaitForSeconds _countUpWait;

        [SerializeField]
        private TMP_Text _addScoreText;
        private Animator _addScoreAnim;
        [SerializeField]
        private TMP_Text _comboText;
        private GameStateHandler _gameStateHandler;
        private ComboCounter _comboCounter;

        private void Start()
        {
            _scoreHandler = ScoreHandler.Instance;
            _scoreHandler.ChangeScore += ChangeScoreUI;
            _addScoreAnim = _addScoreText.GetComponent<Animator>();
            _countUpWait = new WaitForSeconds(_countUpWaitTime);
            _scoreHandler.AddScoreEvent += AddScoreAnim;
            _comboCounter = ComboCounter.Instance;
            _comboCounter.ChangeComboCount += ChangeComboText;
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateScoreUI;
        }

        private void OnDestroy()
        {
            _scoreHandler.ChangeScore -= ChangeScoreUI;
            _scoreHandler.AddScoreEvent -= AddScoreAnim;
            _comboCounter.ChangeComboCount -= ChangeComboText;
            _gameStateHandler.ChangeGameState -= ChangeStateScoreUI;
        }

        private void ChangeScoreUI(int newValue)
        {
            if (_changeScoreCoroutine != null)
            {
                StopCoroutine(_changeScoreCoroutine);
                _scoreText.text = "Score : " + _currentSetScore;
            }
            _changeScoreCoroutine = StartCoroutine(UpdateScoreAnimation(_currentSetScore, newValue));
            _currentSetScore = newValue;
        }

        private IEnumerator UpdateScoreAnimation(int currentValue, int newValue)
        {
            var updateScore = currentValue;
            while (updateScore < newValue)
            {
                updateScore += 2;
                _scoreText.text = "Score : " + updateScore;
                yield return _countUpWait;
            }
            _changeScoreCoroutine = null;
        }

        private void AddScoreAnim(int newValue)
        {
            _addScoreText.text = "+" + newValue;
            _addScoreAnim.Rebind();
            _addScoreAnim.Play("AddScoreAnim");
        }

        private void ChangeComboText(int newValue)
        {
            if (newValue == 0)
            {
                _comboText.enabled = false;
                return;
            }
            if (!_comboText.enabled) _comboText.enabled = true;
            _comboText.text = "Combo " + newValue;
        }

        private void ChangeStateScoreUI(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            if (_changeScoreCoroutine != null)
            {
                StopCoroutine(_changeScoreCoroutine);
                _changeScoreCoroutine = null;
                _addScoreAnim.Rebind();
            }
            _currentSetScore = 0;
            _scoreText.text = "Score : 0";
        }


    }
}
