namespace System
{
    public class PlayerInfoHandler
    {
        private static PlayerInfoHandler _instance;
        public static PlayerInfoHandler Instance => _instance ??= new PlayerInfoHandler();
        private readonly PlayDataIO _playDataIO;
        public string PlayerName { get; private set; }

        public event Action<string> ChangeName;
        public int PlayerRank { get; private set; }

        public int CurrentExp { get; private set; }

        public int CurrentGetExp { get; private set; }

        public int CurrentRank { get; private set; }

        public event Action<int> ChangeRank;
        public event Action CalculatedEvent;
        private const int MaxLevel = 500;
        private int _playerHaveExp;
        private const int ExpFactor = 1000;
        public const string InitialName = "NoName";

        private PlayerInfoHandler()
        {
            _playDataIO = PlayDataIO.Instance;
            _playDataIO.DeleteDataEvent += LoadData;
            LoadData();
        }

        public void ChangePlayerName(string newName)
        {
            if (newName == "") newName = InitialName;
            PlayerName = newName;
            ChangeName?.Invoke(PlayerName);
            _playDataIO.SavePlayerName(newName);
        }

        public void CalcLevel(int getExp)
        {
            CurrentExp = _playerHaveExp;
            CurrentGetExp = getExp;
            CurrentRank = PlayerRank;
            _playerHaveExp += getExp;
            int needExp = PlayerNeedExp(PlayerRank);
            while (_playerHaveExp >= needExp)
            {
                if (PlayerRank >= MaxLevel)
                {
                    _playerHaveExp = needExp;
                    break;
                }
                _playerHaveExp -= needExp;
                PlayerRank++;
                needExp = PlayerNeedExp(PlayerRank);
            }

            CalculatedEvent?.Invoke();
            if (CurrentExp != _playerHaveExp)
                _playDataIO.SavePlayerExp(_playerHaveExp);
            if (CurrentRank == PlayerRank) return;
            ChangeRank?.Invoke(PlayerRank);
            _playDataIO.SavePlayerRank(PlayerRank);
        }

        public int PlayerNeedExp(int rank)
        {
            return rank * ExpFactor;
        }

        private void LoadData()
        {
            PlayerName = _playDataIO.LoadPlayerName();
            ChangeName?.Invoke(PlayerName);
            PlayerRank = _playDataIO.LoadPlayerRank();
            ChangeRank?.Invoke(PlayerRank);
            _playerHaveExp = _playDataIO.LoadPlayerExp();
        }

    }
}
