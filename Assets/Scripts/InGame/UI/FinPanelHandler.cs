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
    private float _animationDuration = 3f;
    private Coroutine _rankUpCoroutine;
    private const float WAIT_TIME = 0.1f;
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
        _countUpWait = new WaitForSeconds(WAIT_TIME);
        // リスナー登録
        _playerInfoHandler.CalculatedEvent += LaunchRankUpAnim;
        _gameStateHandler.ChangeGameState += ChangeStateScoreUI;
    }

    void OnDestroy()
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
        // テキストの変更
        _resultText.text = "Score : " + _scoreHandler.CurrentScore;
        
        if (_scoreHandler.TodayMaxScore == 0) _todayMaxScoreText.text = "---";
        else _todayMaxScoreText.text = "" + _scoreHandler.TodayMaxScore;

        if (_scoreHandler.MaxScore == 0) _maxScoreText.text = "---";
        else _maxScoreText.text = "" + _scoreHandler.MaxScore;
        // テキストの色変更
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
        if (_rankUpCoroutine != null ) StopCoroutine(_rankUpCoroutine);
        _rankUpCoroutine = StartCoroutine(RankUpAnim());
    }

    private IEnumerator RankUpAnim()
    {
        // パネルが開くまで待機
        while (_gameStateHandler.CurrentState != GameStateHandler.GameState.FinGame) yield return null;
        float getExp = _playerInfoHandler.CurrentGetExp;
        _getExpText.text = "+" + getExp;
        float currentExp = _playerInfoHandler.CurrentExp;
        int currentRank = _playerInfoHandler.CurrentRank;
        int needExp = _playerInfoHandler.PlayerNeedExp(currentRank);
        _rankText.text = currentRank.ToString();
        _rankSlider.maxValue = needExp;
        _rankSlider.value = currentExp;

        // _animationDurationの時間で終わるように増加量を計算
        float increase = getExp / _animationDuration * WAIT_TIME;
        while (getExp > 0)
        {            
            currentExp += increase;
            getExp -= increase;
            _rankSlider.value = currentExp;
            // 必要経験値を超えた場合
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

        // コルーチンの参照をクリア
        _rankUpCoroutine = null;
    }
}
