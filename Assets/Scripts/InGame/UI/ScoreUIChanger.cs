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

    
    void Start()
    {
        _scoreHandler = ScoreHandler.Instance;
        _scoreHandler.ChangeScore += ChangeScoreUI;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStateScoreUI;
    }

    private void ChangeScoreUI(int newValue)
    {
        _scoreText.text = "Score : " + newValue;
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
