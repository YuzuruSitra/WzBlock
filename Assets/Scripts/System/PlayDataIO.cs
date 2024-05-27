using CI.QuickSave;
using UnityEngine;

namespace System
{
    public class PlayDataIO
    {
        private static PlayDataIO _instance;
        public static PlayDataIO Instance => _instance ??= new PlayDataIO();
        private readonly QuickSaveWriter _writer;
        private readonly QuickSaveReader _reader;

        public event Action DeleteDataEvent;

        private PlayDataIO()
        {
#if UNITY_IOS
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
        QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
#elif UNITY_EDITOR
            QuickSaveGlobalSettings.StorageLocation = Application.dataPath + "/SaveData";
#endif
            var settings = new QuickSaveSettings
            {
                SecurityMode = SecurityMode.Aes,
                Password = "Password",
                CompressionMode = CompressionMode.Gzip
            };

            _writer = QuickSaveWriter.Create("SaveData", settings);
            if (!QuickSaveBase.RootExists("SaveData")) InitializingData();
            _reader = QuickSaveReader.Create("SaveData", settings);
        }

        // Save
        public void SavePlayerName(string name)
        {
            _writer.Write("Name", name);
            _writer.Commit();
        }

        public void SavePlayerRank(int level)
        {
            _writer.Write("Rank", level);
            _writer.Commit();
        }

        public void SavePlayerExp(int exp)
        {
            _writer.Write("Exp", exp);
            _writer.Commit();
        }

        public void SaveSensitivity(int value)
        {
            _writer.Write("Sensitivity", value);
            _writer.Commit();
        }

        public void SaveVolume(float volume)
        {
            _writer.Write("Volume", volume);
            _writer.Commit();
        }

        public void SaveMaxScore(int value)
        {
            _writer.Write("MaxScore", value);
            _writer.Commit();
        }

        public void SaveTodayScore(int value)
        {
            var data = DateTime.Now.Date;
            _writer.Write("Date", data);
            _writer.Write("MaxTodayScore", value);
            _writer.Commit();
        }

        // Load
        public string LoadPlayerName()
        {
            return _reader.Read<string>("Name");
        }
        public int LoadPlayerRank()
        {
            return _reader.Read<int>("Rank");
        }
        public int LoadPlayerExp()
        {
            return _reader.Read<int>("Exp");
        }

        public int LoadSensitivity()
        {
            return _reader.Read<int>("Sensitivity");
        }
        public float LoadVolume()
        {
            return _reader.Read<float>("Volume");
        }
        public int LoadMaxScore()
        {
            return _reader.Read<int>("MaxScore");
        }

        public int LoadMaxTodayScore(DateTime data)
        {
            var dateData = _reader.Read<DateTime>("Date");
        
            return data != dateData ? 0 : _reader.Read<int>("MaxTodayScore");
        }

        // Delete
        public void DeleteData()
        {
            QuickSaveBase.DeleteRoot("SaveData");
            InitializingData();
            DeleteDataEvent?.Invoke();
        }

        private void InitializingData()
        {
            var pureAssetLoader = new PureAssetLoader();
            var initializingValues = pureAssetLoader.LoadScriptableObject<InitializingValues>("InitializingValues");
            SavePlayerName(initializingValues.PlayerName);
            SavePlayerRank(initializingValues.Rank);
            SavePlayerExp(initializingValues.Exp);
            SaveSensitivity(initializingValues.Sensitivity);
            SaveVolume(initializingValues.Volume);
            SaveMaxScore(initializingValues.MaxScore);
            SaveTodayScore(initializingValues.TodayMaxScore);
        }

    }
}