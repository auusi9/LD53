using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public class FXSingleton : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _achievementSound;
        [SerializeField] private AudioClip _pressSound;

        private Vector2 _pitchRange = new Vector2(0.3f, 0.6f);
        private static FXSingleton _fxSingleton;

        public static FXSingleton Get() => _fxSingleton;

        private void Awake()
        {
            if (_fxSingleton == null)
            {
                _fxSingleton = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayAchievementSound()
        {
            PlayAudioClip(_achievementSound, false);
        }

        public void PlayPressButton()
        {
            PlayAudioClip(_pressSound);
        }

        private void PlayAudioClip(AudioClip audioClip, bool variablePitch = true)
        {
            _audioSource.clip = audioClip;
            if(variablePitch)
                _audioSource.pitch = (Random.Range(_pitchRange.x, _pitchRange.y));
            else
                _audioSource.pitch = 1f;
            _audioSource.Play();
        }
    }
}