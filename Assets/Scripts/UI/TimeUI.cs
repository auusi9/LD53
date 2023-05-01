using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private Outline _pauseSelected;
        [SerializeField] private Outline _playSelected;
        [SerializeField] private Outline _forwardSelected;

        private void Start()
        {
            _pauseSelected.enabled = (false);
            _playSelected.enabled = (true);
            _forwardSelected.enabled = (false);
        }

        public void PauseGame()
        {
            _timeManager.PauseGame(GetHashCode());
            _pauseSelected.enabled = (true);
            _playSelected.enabled = (false);
            _forwardSelected.enabled = (false);
        }

        public void ResumeGame()
        {
            _timeManager.SetDefaultSpeed();
            _timeManager.ResumeGame(GetHashCode());
            _pauseSelected.enabled = (false);
            _playSelected.enabled = (true);
            _forwardSelected.enabled = (false);
        }

        public void SpeedGame()
        {
            _timeManager.NextSpeed();
            _pauseSelected.enabled = (false);
            _playSelected.enabled = (false);
            _forwardSelected.enabled = (true);
        }
    }
}