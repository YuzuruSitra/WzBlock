using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FinPanelHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _resultText;
    [SerializeField]
    private TMP_Text _todayMaxScoreText, _todaySeemText;
    [SerializeField]
    private TMP_Text _maxScoreText, _maxSeemText;
    [SerializeField]
    private Color _defaultColor;
    [SerializeField]
    private Color _emphasizedPink;
    [SerializeField]
    private Color _emphasizedBlue;
    private ScoreHandler _scoreHandler;
    private PlayerInfoHandler _playerInfoHandler;
    private GameStateHandler _gameStateHandler;
    
    [SerializeField]
    private Slider _rankSlider;
    [SerializeField]
    float _animationSpeed = 0.1f;
    [SerializeField]
    private float _animationDuration = 3f;
    private WaitForSeconds _countUpWait;
    [SerializeField]
    private TMP_Text _expText;
    [SerializeField]
    private TMP_Text _rankText;
    [SerializeField]
    private TMP_Text _getExpText;

    void Start()
    {
        _scoreHandler = ScoreHandler.Instance;
        _gameStateHandler = GameStateHandler.Instance;
        _playerInfoHandler = PlayerInfoHandler.Instance;
        // ���X�i�[�o�^
        _playerInfoHandler.CalculatedEvent += LaunchRankUpAnim;
        _gameStateHandler.ChangeGameState += ChangeStateScoreUI;
    }

    private void ChangeStateScoreUI(GameStateHandler.GameState newState)
    {
        if (newState != GameStateHandler.GameState.FinGame) return;
        ChangeResultScore();
    }

    private void ChangeResultScore()
    {
        // �e�L�X�g�̕ύX
        _resultText.text = "Score : " + _scoreHandler.CurrentScore;
        
        if (_scoreHandler.TodayMaxScore == 0) _todayMaxScoreText.text = "---";
        else _todayMaxScoreText.text = "" + _scoreHandler.TodayMaxScore;

        if (_scoreHandler.MaxScore == 0) _maxScoreText.text = "---";
        else _maxScoreText.text = "" + _scoreHandler.MaxScore;
        // �e�L�X�g�̐F�ύX
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
        StartCoroutine(RankUpAnim());
    }

    private IEnumerator RankUpAnim()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.FinGame) yield return null;

        int getExp = _playerInfoHandler.PlayerHaveExp - _playerInfoHandler.CurrentExp;
        _getExpText.text = "+" + getExp;
        int targetExp = _playerInfoHandler.PlayerHaveExp;
        int currentExp = _playerInfoHandler.CurrentExp;
        int currentRank = _playerInfoHandler.CurrentRank;
        int needExp = _playerInfoHandler.PlayerNeedExp(currentRank);
        _rankText.text = currentRank.ToString();
        _rankSlider.maxValue = needExp;

        int steps = getExp / 5;
        if (steps == 0) steps = 1;
        float waitTime = _animationDuration / steps;
        _countUpWait = new WaitForSeconds(waitTime);

        while (currentExp < targetExp)
        {
            // �X���C�_�[�̒l�����X�ɑ���
            currentExp += 5;
            _rankSlider.value = currentExp;

            // �K�v�o���l�𒴂����ꍇ
            if (currentExp >= needExp)
            {
                currentExp -= needExp; // ���݂̌o���l�����Z�b�g
                currentRank++;
                needExp = _playerInfoHandler.PlayerNeedExp(currentRank);
                _rankSlider.maxValue = needExp;
                _rankText.text = currentRank.ToString();
                targetExp = _playerInfoHandler.PlayerHaveExp;
                _rankSlider.value = 0;
            }

            _expText.text = "Exp: " + currentExp + "/" + needExp;

            // �A�j���[�V�����X�s�[�h�ɉ����đҋ@
            yield return _countUpWait;
        }
    }

}
