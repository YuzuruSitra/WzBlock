using System;

public class PlayerInfoHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static PlayerInfoHandler instance;
    public static PlayerInfoHandler Instance => instance ?? (instance = new PlayerInfoHandler());
    private PlayDataIO _playDataIO;
    private string _playerName;
    public string PlayerName => _playerName;
    private int _playerLevel;
    public int PlayerLevel => _playerLevel;
    private const int MAX_LEVEL = 500;
    private int _playerCurrentExp;
    private const int EXP_FACTOR = 1000;
    public int PlayerNeedExp  => _playerLevel * EXP_FACTOR;

    private PlayerInfoHandler()
    {
        _playDataIO = PlayDataIO.Instance;
        _playerName = _playDataIO.LoadPlayerName();
        _playerLevel = _playDataIO.LoadPlayerLevel();
        _playerCurrentExp = _playDataIO.LoadPlayerExp();
    }

    public void ChangePlayerName(string newName)
    {
        _playerName = newName;
        _playDataIO.SavePlayerName(newName);
    }

    public void CalcLevel(int getExp)
    {
        int currentExp = _playerCurrentExp;
        int currentLevel = _playerLevel;
        _playerCurrentExp += getExp;
        while (_playerCurrentExp >= PlayerNeedExp)
        {
            if (_playerLevel >= MAX_LEVEL) return;
            _playerCurrentExp -= PlayerNeedExp;
            _playerLevel++;
        }

        if (currentExp != _playerCurrentExp)
            _playDataIO.SavePlayerExp(_playerCurrentExp);
        if (currentLevel != _playerLevel)
            _playDataIO.SavePlayerLevel(_playerLevel);

    }

}
