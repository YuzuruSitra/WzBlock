using UnityEngine;
using CI.QuickSave;
using System; 

public class PlayDataIO
{
    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;

    public PlayDataIO()
    {
        // �f�[�^�̕ۑ����Application.persistentDataPath�ɕύX
       QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
        // QuickSaveSettings�̃C���X�^���X���쐬
        QuickSaveSettings settings = new QuickSaveSettings();
        // �Í����̕��@
        settings.SecurityMode = SecurityMode.Aes;
        // �Í����L�[
        settings.Password = "Password";
        // ���k�̕��@
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
            Debug.LogWarning("�f�[�^�̓ǂݍ��݃G���[: " + ex.Message);
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
            Debug.LogWarning("�f�[�^�̓ǂݍ��݃G���[: " + ex.Message);
            return 0; 
        }
    }

}