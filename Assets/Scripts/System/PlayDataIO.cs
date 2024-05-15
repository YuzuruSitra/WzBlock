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
            SaveSensitivity(5);
            SaveMaxScore(0);
            SaveTodayScore(0);
        }
        _reader = QuickSaveReader.Create("SaveData", settings);
    }

    // Save
    public void SaveSensitivity(int value)
    {
        _writer.Write("Sensitivity", value);
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
    public int LoadSensitivity()
    {
        return _reader.Read<int>("Sensitivity");
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