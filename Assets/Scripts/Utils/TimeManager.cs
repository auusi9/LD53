using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(menuName = "Time/Time Manager", fileName = "TimeManager", order = 0)]
    public class TimeManager : ScriptableObject
    {
        [SerializeField] private float[] _velocityPresets = new []{1f, 1.25f, 1.5f, 2f};
        [SerializeField] private BaseEvent _restartGame;

        private int _currentVelocityIndex = 0;

        private List<int> _pauseViews = new List<int>();

        private void OnEnable()
        {
            _currentVelocityIndex = 0;
            _pauseViews.Clear();
            _restartGame.Register(RestartGame);
        }

        private void RestartGame()
        {
            _currentVelocityIndex = 0;
            _pauseViews.Clear();
        }

        private void OnDisable()
        {
            _restartGame.UnRegister(RestartGame);
        }

        public void NextSpeed()
        {
            _currentVelocityIndex++;

            if (_currentVelocityIndex >= _velocityPresets.Length)
            {
                _currentVelocityIndex = 0;
            }
            
            SetCurrentSpeed();
        }

        public void SetDefaultSpeed()
        {
            _currentVelocityIndex = 0;
            
            SetCurrentSpeed();
        }

        public void PauseGame(int hashPause)
        {
            if(_pauseViews.Contains(hashPause))
                return;
            
            _pauseViews.Add(hashPause);
            Time.timeScale = 0f;
        }

        public void ResumeGame(int hashPause)
        {
            if(_pauseViews.Contains(hashPause))
                _pauseViews.Remove(hashPause);

            SetCurrentSpeed();
        }

        private void SetCurrentSpeed()
        {
            if (_pauseViews.Count == 0)
            {
                Time.timeScale = _velocityPresets[_currentVelocityIndex];
            }
        }
    }
}