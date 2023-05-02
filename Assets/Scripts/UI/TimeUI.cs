using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private Image _pauseImage;
        [SerializeField] private Image _playImage;
        [SerializeField] private Image _forwardImage;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _deactivatedColor;

        private void Start()
        {
            ResumeGame();
        }

        public void PauseGame()
        {
            _timeManager.PauseGame(GetHashCode());
            _pauseImage.color = _selectedColor;
            _playImage.color = _deactivatedColor;
            _forwardImage.color = _deactivatedColor;
        }

        public void ResumeGame()
        {
            _timeManager.SetDefaultSpeed();
            _timeManager.ResumeGame(GetHashCode());
            _pauseImage.color = _deactivatedColor;
            _playImage.color = _selectedColor;
            _forwardImage.color = _deactivatedColor;
        }

        public void SpeedGame()
        {
            _timeManager.NextSpeed();
            _timeManager.ResumeGame(GetHashCode());
            _pauseImage.color = _deactivatedColor;
            _playImage.color = _deactivatedColor;
            _forwardImage.color = _selectedColor;
        }
    }
}