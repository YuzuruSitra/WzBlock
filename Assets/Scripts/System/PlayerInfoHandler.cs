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
    private int _playerLevel;
    public int PlayerLevel => _playerLevel;
    public event Action<int> ChangeLevel;
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
        if (newName == "") newName = "NoName";
        _playerName = newName;
        ChangeName?.Invoke(_playerName);
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
        if (currentLevel == _playerLevel) return;
        ChangeLevel?.Invoke(_playerLevel);
        _playDataIO.SavePlayerLevel(_playerLevel);
    }

}
