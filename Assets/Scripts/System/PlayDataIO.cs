using UnityEngine;
using CI.QuickSave;
using System; 

public class PlayDataIO
{
    // �V���O���g��
    private static PlayDataIO instance;
    public static PlayDataIO Instance => instance ?? (instance = new PlayDataIO());
    private QuickSaveWriter _writer;
    private QuickSaveReader _reader;

    public PlayDataIO()
    {
        // �f�[�^�̕ۑ����Application.persistentDataPath�ɕύX
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
        QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
        // QuickSaveSettings�̃C���X�^���X���쐬
        QuickSaveSettings settings = new QuickSaveSettings();
        // �Í����̕��@
        settings.SecurityMode = SecurityMode.Aes;
        // �Í����L�[
        settings.Password = "Password";
        // ���k�̕��@
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