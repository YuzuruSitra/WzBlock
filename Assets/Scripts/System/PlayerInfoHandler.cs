using System;

public class PlayerInfoHandler
{
    // シングルトン
    private static PlayerInfoHandler instance;
    public static PlayerInfoHandler Instance => instance ?? (instance = new PlayerInfoHandler());
    private PlayDataIO _playDataIO;
    private string _playerName;
    public string PlayerName => _playerName;
    public event Action<string> ChangeName;
    private int _playerRank;
    public int PlayerRank => _playerRank;
    private int _currentExp;
    public int CurrentExp => _currentExp;
    private int _currentGetExp;
    public int CurrentGetExp => _currentGetExp;
    private int _currentRank;
    public int CurrentRank => _currentRank;
    public event Action<int> ChangeRank;
    public event Action CalculatedEvent;
    private const int MAX_LEVEL = 500;
    private int _playerHaveExp;
    public int PlayerHaveExp => _playerHaveExp;
    private const int EXP_FACTOR = 1000;
    public const string INITIAL_NAME = "NoName";

    private PlayerInfoHandler()
    {
        _playDataIO = PlayDataIO.Instance;
        _playerName = _playDataIO.LoadPlayerName();
        _playerRank = _playDataIO.LoadPlayerRank();
        _playerHaveExp = _playDataIO.LoadPlayerExp();
    }

    public void ChangePlayerName(string newName)
    {
        if (newName == "") newName = INITIAL_NAME;
        _playerName = newName;
        ChangeName?.Invoke(_playerName);
        _playDataIO.SavePlayerName(newName);
    }

    public void CalcLevel(int getExp)
    {
        _currentExp = _playerHaveExp;
        _currentGetExp = getExp;
        _currentRank = _playerRank;
        _playerHaveExp += getExp;
        int needExp = PlayerNeedExp(_playerRank);
        while (_playerHaveExp >= needExp)
        {
            if (_playerRank >= MAX_LEVEL) return;
            _playerHaveExp -= needExp;
            _playerRank++;
            needExp = PlayerNeedExp(_playerRank);
        }

        CalculatedEvent?.Invoke();
        // セーブ処理
        if (_currentExp != _playerHaveExp)
            _playDataIO.SavePlayerExp(_playerHaveExp);
        if (_currentRank != _playerRank) return;
        ChangeRank?.Invoke(_playerRank);
        _playDataIO.SavePlayerRank(_playerRank);
    }

    public int PlayerNeedExp(int rank)
    {
        return rank * EXP_FACTOR;
    }

}
