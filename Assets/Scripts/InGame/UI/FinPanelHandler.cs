using System;
using System.Collections;
using InGame.InGameSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class FinPanelHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _todayMaxScoreText, _todaySeemText;
        [SerializeField] private TMP_Text _maxScoreText, _maxSeemText;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _emphasizedPink;
        [SerializeField] private Color _emphasizedBlue;
        private ScoreHandler _scoreHandler;
        private PlayerInfoHandler _playerInfoHandler;
        private GameStateHandler _gameStateHandler;

        [SerializeField] private Slider _rankSlider;
        [SerializeField] private float _animationSpeed = 50f; // 1秒あたりの増加量
        private Coroutine _rankUpCoroutine;
        private const float WaitTime = 0.1f;
        private WaitForSeconds _countUpWait;
        [SerializeField] private TMP_Text _expText;
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private TMP_Text _getExpText;

        private void Start()
        {
            _scoreHandler = ScoreHandler.Instance;
            _gameStateHandler = GameStateHandler.Instance;
            _playerInfoHandler = PlayerInfoHandler.Instance;
            _countUpWait = new WaitForSeconds(WaitTime);
            _playerInfoHandler.CalculatedEvent += LaunchRankUpAnim;
            _gameStateHandler.ChangeGameState += ChangeStateScoreUI;
        }

        private void OnDestroy()
        {
            _playerInfoHandler.CalculatedEvent -= LaunchRankUpAnim;
            _gameStateHandler.ChangeGameState -= ChangeStateScoreUI;
        }

        private void ChangeStateScoreUI(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.FinGame) return;
            ChangeResultScore();
        }

        private void ChangeResultScore()
        {
            _resultText.text = "Score : " + _scoreHandler.CurrentScore;

            if (_scoreHandler.TodayMaxScore == 0) _todayMaxScoreText.text = "---";
            else _todayMaxScoreText.text = "" + _scoreHandler.TodayMaxScore;

            if (_scoreHandler.MaxScore == 0) _maxScoreText.text = "---";
            else _maxScoreText.text = "" + _scoreHandler.MaxScore;

            if (_scoreHandler.TodayMaxScore == _scoreHandler.CurrentScore)
            {
                _todayMaxScoreText.color = _emphasizedPink;
                _todaySeemText.color = _emphasizedBlue;
            }
            else
            {
                _todayMaxScoreText.color = _defaultColor;
                _todaySeemText.color = _defaultColor;
            }

            if (_scoreHandler.MaxScore == _scoreHandler.CurrentScore)
            {
                _maxScoreText.color = _emphasizedPink;
                _maxSeemText.color = _emphasizedBlue;
            }
            else
            {
                _maxScoreText.color = _defaultColor;
                _maxSeemText.color = _defaultColor;
            }
        }

        private void LaunchRankUpAnim()
        {
            if (_rankUpCoroutine != null) StopCoroutine(_rankUpCoroutine);
            _rankUpCoroutine = StartCoroutine(RankUpAnim());
        }

        private IEnumerator RankUpAnim()
        {
            while (_gameStateHandler.CurrentState != GameStateHandler.GameState.FinGame) yield return null;

            float getExp = _playerInfoHandler.CurrentGetExp;
            _getExpText.text = "+" + getExp;
            float currentExp = _playerInfoHandler.CurrentExp;
            var currentRank = _playerInfoHandler.CurrentRank;
            var needExp = _playerInfoHandler.PlayerNeedExp(currentRank);

            _rankText.text = currentRank.ToString();
            _rankSlider.maxValue = needExp;
            _rankSlider.value = currentExp;

            while (getExp > 0)
            {
                float increase = _animationSpeed * WaitTime;
                currentExp += increase;
                getExp -= increase;
                _rankSlider.value = currentExp;

                if (currentExp >= needExp)
                {
                    currentExp -= needExp;
                    currentRank++;
                    needExp = _playerInfoHandler.PlayerNeedExp(currentRank);
                    _rankSlider.maxValue = needExp;
                    _rankText.text = currentRank.ToString();
                    _rankSlider.value = currentExp;
                }

                _expText.text = "Exp: " + Mathf.CeilToInt(currentExp) + "/" + needExp;
                yield return _countUpWait;
            }

            _rankUpCoroutine = null;
        }
    }
}
