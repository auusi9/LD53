using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioSource _buttonAudioSource;
        [SerializeField] private AudioClip _hoverButtonAudio;
        [SerializeField] private AudioClip _pressButtonAudio;
        private Vector2 _pitchRange = new Vector2(0.3f, 0.6f);
        private float _initialVolume;

        private void Start()
        {
            _initialVolume = _buttonAudioSource.volume;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_button.interactable)
                PlayAudioClip(_hoverButtonAudio, 0.75f);
        }

        public void PlayClickSound()
        {
            PlayAudioClip(_pressButtonAudio, 1f);
        }

        private void PlayAudioClip(AudioClip audioClip, float volume)
        {
            _buttonAudioSource.clip = audioClip;
            _buttonAudioSource.volume = _initialVolume * volume;
            _buttonAudioSource.pitch = (Random.Range(_pitchRange.x, _pitchRange.y));
            _buttonAudioSource.Play();
        }
    }
}