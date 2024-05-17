using System;

public class PlayerInfoHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static PlayerInfoHandler instance;
    public static PlayerInfoHandler Instance => instance ?? (instance = new PlayerInfoHandler());
    private PlayDataIO _playDataIO;
    private string _playerName;
    public string PlayerName => _playerName;
    public event Action<string> ChangeName;
    private int _playerRank;
    public int PlayerRank => _playerRank;
    public event Action<int> ChangeLevel;
    private const int MAX_LEVEL = 500;
    private int _playerCurrentExp;
    private const int EXP_FACTOR = 1000;
    public int PlayerNeedExp  => _playerRank * EXP_FACTOR;

    private PlayerInfoHandler()
    {
        _playDataIO = PlayDataIO.Instance;
        _playerName = _playDataIO.LoadPlayerName();
        _playerRank = _playDataIO.LoadPlayerRank();
        _playerCurrentExp = _playDataIO.LoadPlayerExp();
    }

    public void ChangePlayerName(string newName)
    {
        if (newName == "") newName = "NoName";
        _playerName = newName;
        ChangeName?.Invoke(_playerName);
        _playDataIO.SavePlayerName(newName);
    }

    public void CalcLevel(int getExp)
    {
        int currentExp = _playerCurrentExp;
        int currentLevel = _playerRank;
        _playerCurrentExp += getExp;
        while (_playerCurrentExp >= PlayerNeedExp)
        {
            if (_playerRank >= MAX_LEVEL) return;
            _playerCurrentExp -= PlayerNeedExp;
            _playerRank++;
        }

        if (currentExp != _playerCurrentExp)
            _playDataIO.SavePlayerExp(_playerCurrentExp);
        if (currentLevel == _playerRank) return;
        ChangeLevel?.Invoke(_playerRank);
        _playDataIO.SavePlayerRank(_playerRank);
    }

}
