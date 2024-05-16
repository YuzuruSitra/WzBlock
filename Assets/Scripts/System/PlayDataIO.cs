using UnityEngine;
using CI.QuickSave;
using System; 

public class PlayDataIO
{
    // シングルトン
    private static PlayDataIO instance;
    public static PlayDataIO Instance => instance ?? (instance = new PlayDataIO());
    private QuickSaveWriter _writer;
    private QuickSaveReader _reader;

    public PlayDataIO()
    {
        // データの保存先をApplication.persistentDataPathに変更
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
        QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // 暗号化の方法
        settings.SecurityMode = SecurityMode.Aes;
        // 暗号化キー
        settings.Password = "Password";
        // 圧縮の方法
        settings.CompressionMode = CompressionMode.Gzip;

        _writer = QuickSaveWriter.Create("SaveData", settings);
        if (!QuickSaveBase.RootExists("SaveData"))
        {
            SavePlayerName("NoName");
            SavePlayerLevel(1);
            SavePlayerExp(0);
            SaveSensitivity(5);
            SaveVolume(0.5f);
            SaveMaxScore(0);
            SaveTodayScore(0);
        }
        _reader = QuickSaveReader.Create("SaveData", settings);
    }

    // Save
    public void SavePlayerName(string name)
    {
        _writer.Write("Name", name);
        _writer.Commit();
    }

    public void SavePlayerLevel(int level)
    {
        _writer.Write("Level", level);
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
        DateTime data = DateTime.Now.Date;
        _writer.Write("Date", data);
        _writer.Write("MaxTodayScore", value);
        _writer.Commit();
    }

    // Load
    public string LoadPlayerName()
    {
        return _reader.Read<string>("Name");
    }
    public int LoadPlayerLevel()
    {
        return _reader.Read<int>("Level");
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
        DateTime dateData = _reader.Read<DateTime>("Date");
        
        if (data != dateData) return 0;
        return _reader.Read<int>("MaxTodayScore");
    }

    // Delete
    public void DeleteData()
    {
        QuickSaveWriter.DeleteRoot("SaveData");
    }

}