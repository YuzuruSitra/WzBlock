namespace System
{
    public class SensiHandler
    {
        private static SensiHandler _instance;
        public static SensiHandler Instance => _instance ??= new SensiHandler();
        private readonly PlayDataIO _playDataIO;
        public int Sensitivity { get; private set; }

        private SensiHandler()
        {
            _playDataIO = PlayDataIO.Instance;
            _playDataIO.DeleteDataEvent += LoadData;
            LoadData();
        }

        public void ChangeSensitivity(int value)
        {
            Sensitivity = value;
            _playDataIO.SaveSensitivity(value);
        }

        private void LoadData()
        {
            Sensitivity = _playDataIO.LoadSensitivity();
        }

    }
}
