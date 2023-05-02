using System;
using Events;
using Score;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private BaseEvent _gameOverEvent;
        [SerializeField] private BaseEvent _restartGame;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private RewardsConfiguration _rewardsConfiguration;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private TextMeshProUGUI _weeksText;
        [SerializeField] private string _weeksString = "{0} WEEKS";
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Start()
        {
            _gameOverEvent.Register(GameOver);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _gameOverEvent.UnRegister(GameOver);
        }

        private void GameOver()
        {
            if(gameObject.activeSelf)
                return;
            
            _timeManager.PauseGame(GetHashCode());
            gameObject.SetActive(true);
            _weeksText.text = string.Format(_weeksString, _rewardsConfiguration.CurrentCycle);
            _scoreText.text = _scoreSystem.Score.ToString();
        }

        public void RestartGame()
        {
            _timeManager.ResumeGame(GetHashCode());
            _restartGame.Fire();
            SceneManager.LoadScene(0);
        }
    }
}