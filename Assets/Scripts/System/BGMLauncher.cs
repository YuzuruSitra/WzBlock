using UnityEngine;

namespace System
{
    public class BGMLauncher : MonoBehaviour
    {
        private SoundHandler _soundHandler;
        [SerializeField] protected AudioClip _hitSound;
        
        private void Start()
        {
            _soundHandler = GameObject.FindWithTag("SoundHandler").GetComponent<SoundHandler>();
            _soundHandler.PlayBgm(_hitSound);
        }
    }
}
