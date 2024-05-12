using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreUIChanger : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _resultText;
    private ScoreHandler _scoreHandler;
    private GameStateHandler _gameStateHandler;
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
    private BallMover _ballMover;

    void Start()
    {
        _scoreHandler = ScoreHandler.Instance;
        _scoreHandler.ChangeScore += ChangeScoreUI;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateScoreUI;
        _countUpWait = new WaitForSeconds(_countUpWaitTime);
        _addScoreAnim = _addScoreText.GetComponent<Animator>();
        _scoreHandler.AddScoreEvent += AddScoreAnim;
        _ballMover = GameObject.FindWithTag("Ball").GetComponent<BallMover>();
        _ballMover.ChangeHitCount += ChangeComboText;
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
        int updateScore = currentValue;
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
        switch(newState)
        {
            case GameStateHandler.GameState.Launch:
                _scoreText.text = "Score : 0";
                break;
            case GameStateHandler.GameState.FinGame:
                ChangeResultScore();
                break;
        }
    }

    private void ChangeResultScore()
    {
        _resultText.text = "Score : " + _scoreHandler.CurrentScore;
    }

}
