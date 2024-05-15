using UnityEngine;

// SE��BGM�̍Đ���~
public class SaundHandler : MonoBehaviour
{
    private PlayDataIO _playDataIO;
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [SerializeField]
    private AudioSource _seAudioSource;
    private float _currentVolume;
    public float CurrentVolume => _currentVolume;
    void Start()
    {
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;

        if (checkResult) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _playDataIO = PlayDataIO.Instance;
        _currentVolume = _playDataIO.LoadVolume();
        SetNewVolume(_currentVolume);
    }

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SaundHandler");
    }

    // �Đ�
    public void PlayBgm(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        if(clip == null) return;
        _bgmAudioSource.Play();
    }

    public void PlaySe(AudioClip clip)
    {
        if(clip == null) return;
        _seAudioSource.PlayOneShot(clip);
    }

    // ��~
    public void StopSaund()
    {
        _bgmAudioSource.Stop();
        _seAudioSource.Stop();
    }

    // ���ʕύX
    public void SetNewVolume(float newVolume)
    {
        _currentVolume = newVolume;
        _bgmAudioSource.volume = newVolume;
        _seAudioSource.volume = newVolume;
    }

    public void FixedVolume()
    {
        _playDataIO.SaveVolume(_currentVolume);
    }
}