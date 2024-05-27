using UnityEngine;

// SE��BGM�̍Đ���~
namespace System
{
    public class SoundHandler : MonoBehaviour
    {
        private PlayDataIO _playDataIO;
        [SerializeField]
        private AudioSource _bgmAudioSource;
        [SerializeField]
        private AudioSource _seAudioSource;

        public float CurrentVolume { get; private set; }

        private void Awake()
        {
            var soundManager = CheckOtherSoundManager();
            var checkResult = soundManager != null && soundManager != gameObject;

            if (checkResult) Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            _playDataIO = PlayDataIO.Instance;
            _playDataIO.DeleteDataEvent += LoadData;
            LoadData();
            SetNewVolume(CurrentVolume);
        }

        private static GameObject CheckOtherSoundManager()
        {
            return GameObject.FindGameObjectWithTag("SoundHandler");
        }
        
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
        
        public void SetNewVolume(float newVolume)
        {
            CurrentVolume = newVolume;
            _bgmAudioSource.volume = CurrentVolume;
            _seAudioSource.volume = CurrentVolume;
        }

        public void FixedVolume()
        {
            _playDataIO.SaveVolume(CurrentVolume);
        }

        private void LoadData()
        {
            CurrentVolume = _playDataIO.LoadVolume();
        }

    }
}