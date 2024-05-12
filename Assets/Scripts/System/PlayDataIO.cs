using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System; 

public class PlayDataIO
{
    private QuickSaveWriter _writer;
    private QuickSaveReader _reader;

    public PlayDataIO()
    {
        // データの保存先をApplication.persistentDataPathに変更
        QuickSaveGlobalSettings.StorageLocation = Application.dataPath;
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // 暗号化の方法
        settings.SecurityMode = SecurityMode.Aes;
        // 暗号化キー
        settings.Password = "Password";
        // 圧縮の方法
        settings.CompressionMode = CompressionMode.Gzip;

        _writer = QuickSaveWriter.Create("SaveData", settings);
        if (FileAccess.Exists("SaveData"))
        {
            SaveMaxScore(0);
            SaveTodayScore(0);
        }
        _reader = QuickSaveReader.Create("SaveData", settings);
    }

    // Save
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

}