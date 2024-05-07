using UnityEngine;
using CI.QuickSave;
using System; 

public class PlayDataIO
{
    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;

    public PlayDataIO()
    {
        // データの保存先をApplication.persistentDataPathに変更
       QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // 暗号化の方法
        settings.SecurityMode = SecurityMode.Aes;
        // 暗号化キー
        settings.Password = "Password";
        // 圧縮の方法
        settings.CompressionMode = CompressionMode.Gzip;

        _writer = QuickSaveWriter.Create("PlayData", settings);
        _reader = QuickSaveReader.Create("PlayData", settings);
    }

    // Save
    public void SaveMaxScore(int value)
    {
        _writer.Write("MaxScore", value);
        _writer.Commit();
    }

     public void SaveTodayScore(int value)
    {
        DateTime time1 = DateTime.Now.Date;
         _writer.Write("MaxTodayScore", value);
              _writer.Commit();
    }

    // Load
    public int LoadMaxScre()
    {
        try
        {
            return _reader.Read<int>("MaxScore");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);
            return 0; 
        }
    }

    public int LoadMaxTodayScre()
    {
        try
        {
            return _reader.Read<int>("MaxTodayScore");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);
            return 0; 
        }
    }

}